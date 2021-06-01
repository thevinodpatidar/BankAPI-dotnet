using System;
using System.Collections.Generic;
using BankAPIApplication.Entities;

namespace BankAPIApplication.Models.Transactions
{
    public class UserTransactionsResponse
    {
        public int userId { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
