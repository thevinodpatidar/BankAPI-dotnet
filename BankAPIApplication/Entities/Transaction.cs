using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace BankAPIApplication.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Amount { get; set; }
        public string Mode { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

    }
}