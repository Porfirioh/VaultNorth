using System;
using System.Collections.Generic;

namespace BankApp.ViewModels
{
    public class EditCustomerViewModel
    {

        public Customer CurrentCustomer { get; set; } = new Customer();
        public int CustomerId { get; set; }
        public string Gender { get; set; }
        public string Givenname { get; set; }
        public string Surname { get; set; }
        public string Streetaddress { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public DateTime Birthday { get; set; }
        public string NationalId { get; set; }
        public string Telephonecountrycode { get; set; }
        public string Telephonenumber { get; set; }
        public string Emailaddress { get; set; }

        public List<Account> Accounts { get; set; } = new List<Account>();
        public class Account
        {
            public int AccountId { get; set; }
            public string Frequency { get; set; }
            public DateTime Created { get; set; }
            public decimal Balance { get; set; }
            public decimal TotalBalance { get; set; }
            public List<Disposition> Dispositions { get; set; }
            public class Disposition
            {
                public int CustomerId { get; set; }
                public string Givenname { get; set; }
                public string Surname { get; set; }
                public string Type { get; set; }
            }

        }
        public class Customer
        {
            public int CustomerId { get; set; }
            public string Gender { get; set; }
            public string Givenname { get; set; }
            public string Surname { get; set; }
            public string Streetaddress { get; set; }
            public string City { get; set; }
            public string Zipcode { get; set; }
            public string Country { get; set; }
            public string CountryCode { get; set; }
            public DateTime Birthday { get; set; }
            public string NationalId { get; set; }
            public string Telephonecountrycode { get; set; }
            public string Telephonenumber { get; set; }
            public string Emailaddress { get; set; }
        }
    }
}