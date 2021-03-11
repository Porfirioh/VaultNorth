using System.Collections.Generic;

namespace BankApp.ViewModels
{
    public class DispositionsViewModel
    {
        public List<Customer> Customers { get; set; } = new List<Customer>();

        public class Customer
        {
            public string Givenname { get; set; }
            public int AccountId { get; set; }
            public string Type { get; set; }
        }
    }
}