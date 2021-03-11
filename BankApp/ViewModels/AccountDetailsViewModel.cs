using System;
using System.Collections.Generic;

namespace BankApp.ViewModels
{
    public class AccountDetailsViewModel
    {
        public int CustomerId { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public PagingViewModel PagingViewModel { get; set; } = new PagingViewModel();

        public AccountDetails Account { get; set; } = new AccountDetails();
        public class AccountDetails
        {
            public int AccountId { get; set; }
            public decimal Balance { get; set; }

        }
        public class Transaction
        {
            public DateTime Date { get; set; }
            public string Type { get; set; }
            public string Operation { get; set; }
            public decimal Amount { get; set; }
            public decimal Balance { get; set; }
            public string Symbol { get; set; }
            public string Bank { get; set; }
            public string Account { get; set; }
        }
    }
}