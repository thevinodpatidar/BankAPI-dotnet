using System.ComponentModel.DataAnnotations;
using BankAPIApplication.Entities;

namespace BankAPIApplication.Models.Transactions
{
    
    public class TransactionRequest
    {
        [Required]
        public int Amount { get; set; }

        [Required]
        public string Mode { get; set; }
    }
}
