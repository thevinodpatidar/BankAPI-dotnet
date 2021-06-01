using System;
namespace BankAPIApplication.Models.Transactions
{
    public class TransactionResponse
    {
        public int Amount { get; set; }
        public int TotalBalance { get; set; }
        public string Status { get; set; }
    }
}
