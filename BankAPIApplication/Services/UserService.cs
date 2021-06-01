using AutoMapper;
using BCryptNet = BCrypt.Net.BCrypt;
using System.Collections.Generic;
using System.Linq;
using BankAPIApplication.Authorization;
using BankAPIApplication.Entities;
using BankAPIApplication.Helpers;
using BankAPIApplication.Models.Users;
using BankAPIApplication.Models.Transactions;

namespace BankAPIApplication.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        TransactionResponse Transaction(int userId, TransactionRequest model);
        IEnumerable<Transaction> UserTransactions(int userId);
        Transaction UserTransaction(int userId,int id);
        IEnumerable<User> GetAll();
        User GetById(int id);
        void Register(RegisterRequest model);
        void Update(int id, UpdateRequest model);
        void Delete(int id);
    }

    public class UserService : IUserService
    {
        private DataContext _context;
        private IJwtUtils _jwtUtils;
        private readonly IMapper _mapper;

        public UserService(
            DataContext context,
            IJwtUtils jwtUtils,
            IMapper mapper)
        {
            _context = context;
            _jwtUtils = jwtUtils;
            _mapper = mapper;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _context.Users.SingleOrDefault(x => x.Username == model.Username);

            // validate
            if (user == null || !BCryptNet.Verify(model.Password, user.Password))
                throw new AppException("Username or password is incorrect");

            // authentication successful
            var response = _mapper.Map<AuthenticateResponse>(user);
            response.Token = _jwtUtils.GenerateToken(user);
            return response;
        }

        public TransactionResponse Transaction(int userId,TransactionRequest model)
        {
            System.Console.WriteLine(model);
            var user = getUser(userId);
            if (model.Mode == "DEPOSIT")
            {
                user.Balance = user.Balance + model.Amount;
                _context.Users.Update(user);
                _context.SaveChanges();
            }
            else
            {
                if (user.Balance < model.Amount)
                {
                    throw new AppException("Insufficient Balance");  
                }
                user.Balance = user.Balance - model.Amount;
                _context.Users.Update(user);
                _context.SaveChanges();
            }

            Transaction transaction = new Transaction
            {
                UserId = userId,
                Amount = model.Amount,
                Mode =  model.Mode
            };
            _context.Transactions.Add(transaction);
            _context.SaveChanges();


            // validate
            //if (user == null)
            //    throw new AppException("Username or password is incorrect");

            //// authentication successful
            var response = new TransactionResponse {
                TotalBalance = (int)user.Balance,
                Amount = model.Amount,
                Status = "Success"
            };
            //response.Token = _jwtUtils.GenerateToken(user);
            return response;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }

        public User GetById(int id)
        {
            return getUser(id);
        }

        public void Register(RegisterRequest model)
        {
            // validate
            if (_context.Users.Any(x => x.Username == model.Username))
                throw new AppException("Username '" + model.Username + "' is already taken");

            // map model to new user object
            var user = _mapper.Map<User>(model);

            // hash password
            user.Password = BCryptNet.HashPassword(model.Password);

            // save user
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(int id, UpdateRequest model)
        {
            var user = getUser(id);

            // validate
            if (model.Username != user.Username && _context.Users.Any(x => x.Username == model.Username))
                throw new AppException("Username '" + model.Username + "' is already taken");

            // hash password if it was entered
            if (!string.IsNullOrEmpty(model.Password))
                user.Password = BCryptNet.HashPassword(model.Password);

            // copy model to user and save

            _mapper.Map(model, user);
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = getUser(id);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        // helper methods

        private User getUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }

        public IEnumerable<Transaction> UserTransactions(int userId)
        {

            var transactions = _context.Transactions.Where(t => t.UserId == userId).ToArray();
            System.Console.WriteLine(transactions);
        if (transactions == null) throw new KeyNotFoundException("Transactions not found");
        return transactions;
        }

        public Transaction UserTransaction(int userId,int id)
        {

            var transaction = _context.Transactions.Where(t => t.Id == id).Where(t => t.UserId == userId).First();
            System.Console.WriteLine(transaction);
            if (transaction == null) throw new KeyNotFoundException("Transaction not found");
            return transaction;
        }
    }
}
