using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using BankAPIApplication.Authorization;
using BankAPIApplication.Helpers;
using BankAPIApplication.Models.Users;
using BankAPIApplication.Services;
using BankAPIApplication.Models.Transactions;
using System.Security.Claims;
using BankAPIApplication.Entities;

namespace BankAPIApplication.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UsersController(
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(RegisterRequest model)
        {
            _userService.Register(model);
            return Ok(new { message = "Registration successful" });
        }

        [HttpPost("transaction")]
        public IActionResult Transaction(TransactionRequest model)
        {
            var currentUser = (User)HttpContext.Items["User"];
            var response =  _userService.Transaction(currentUser.Id, model);
            return Ok(response);
        }

        [HttpGet("transactions")]
        public IActionResult UserTransactions()
        {
            var currentUser = (User)HttpContext.Items["User"];
            var transactions = _userService.UserTransactions(currentUser.Id);
            return Ok(transactions);
        }

        [HttpGet("transaction/{id}")]
        public IActionResult UserTransaction(int id)
        {
            var currentUser = (User)HttpContext.Items["User"];
            var transaction = _userService.UserTransaction(currentUser.Id,id);
            return Ok(transaction);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            return Ok(user);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateRequest model)
        {
            _userService.Update(id, model);
            return Ok(new { message = "User updated successfully" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok(new { message = "User deleted successfully" });
        }
    }
}
