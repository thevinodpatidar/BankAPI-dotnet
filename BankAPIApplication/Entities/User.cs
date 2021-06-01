using System.Collections.Generic;
using Newtonsoft.Json;

namespace BankAPIApplication.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public long Balance { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        [JsonIgnore]
        public IList<Transaction> Transactions { get; set; }
    }
}