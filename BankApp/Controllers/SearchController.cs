using BankApp.Models;
using BankApp.Repositories;
using BankApp.Services;
using BankApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BankApp.Controllers
{
    [Authorize(Roles = "Cashier")]
    public class SearchController : Controller
    {
        private readonly IBankAppService _service;
        private readonly BankAppDataContext _context;
        private readonly IBankAppRepository _repository;
        private readonly int PageSize = 50;
        private readonly int Transactions = 20;

        public SearchController(IBankAppService service, BankAppDataContext context, IBankAppRepository repository)
        {
            _service = service;
            _context = context;
            _repository = repository;
        }
        public IActionResult Search()
        {
            var model = new SearchViewModel();

            return View(model);
        }

        [HttpGet]
        public IActionResult Search(SearchViewModel model, string page)
        {
            
            int currentPage = string.IsNullOrEmpty(page) ? 1 : Convert.ToInt32(page);
        
            var query = _repository.GetAllCustomers()
                .Where(customer => customer.Givenname.Contains(model.Name) || customer.Surname.Contains(model.Name) || customer.City.Contains(model.City))
                .Select(customer => new SearchViewModel.SearchResult {
                CustomerId = customer.CustomerId,
                Birthday = customer.Birthday,
                Givenname = customer.Givenname,
                Surname = customer.Surname,
                Streetaddress = customer.Streetaddress,
                City = customer.City,
                Zipcode = customer.Zipcode 
                });

            var pageCount = (double)query.Count() / PageSize;

            model.PagingViewModel.MaxPages = (int)Math.Ceiling(pageCount);

            query = query.Skip((currentPage - 1) * PageSize).Take(PageSize);

            model.PagingViewModel.CurrentPage = currentPage;

            model.SearchResults = query.ToList();

            model.ShowSearchResults = true;
            
            if (model.Name == null && model.City == null)
                model.ShowSearchResults = false;
            
            return View(model);
        }

        public IActionResult CustomerDetails(int id)
        {

            var model = new CustomerDetailsViewModel();

            var customer = _repository.GetCustomer(id).Select(c => new CustomerDetailsViewModel.Customer {
                CustomerId = c.CustomerId,
                Gender = c.Gender,
                Givenname = c.Givenname,
                Surname = c.Surname,
                Streetaddress = c.Streetaddress,
                City = c.City,
                Zipcode = c.Zipcode,
                Country = c.Country,
                CountryCode = c.CountryCode,
                Birthday = c.Birthday,
                NationalId = c.NationalId,
                Telephonecountrycode = c.Telephonecountrycode,
                Telephonenumber = c.Telephonenumber,
                Emailaddress = c.Emailaddress
            }).FirstOrDefault();

            if(customer == null)
            {
                var searchViewModel = new SearchViewModel();

                searchViewModel.Error = "Kunde inte hittas";

                return View("Search", searchViewModel);
            }

            model.CurrentCustomer = customer;

            var query = _context.Dispositions.Include(d => d.Customer).Include(d => d.Account).Where(d => d.CustomerId == id).Select(d => new CustomerDetailsViewModel.Account { 
            AccountId = d.AccountId,
            Frequency = d.Account.Frequency,
            Created = d.Account.Created,
            Balance = d.Account.Balance,
            });

            model.Accounts = query.ToList();

            model.CustomerId = id;


            foreach (var account in model.Accounts)
            {
                var disponents = _context.Dispositions.Where(d => d.AccountId == account.AccountId).Include(d => d.Customer).Select( d => new CustomerDetailsViewModel.Disponent { 
                CustomerId = d.Customer.CustomerId,
                Type = d.Type,
                Givenname = d.Customer.Givenname,
                Surname = d.Customer.Surname
                });

                account.Disponents = disponents.ToList();

            }


            return View(model);
        }




        [HttpGet]
        public IActionResult AccountDetails(int id, string page)
        {
            int currentPage = string.IsNullOrEmpty(page) ? 1 : Convert.ToInt32(page);

            var model = new AccountDetailsViewModel();

            model.CustomerId = id;

            model.Account = _context.Accounts.Where(a => a.AccountId == id).Select(a => new AccountDetailsViewModel.AccountDetails { 
                    AccountId = a.AccountId, 
                    Balance = a.Balance }).FirstOrDefault();

           

            var query = _context.Transactions.Where(t => t.AccountId == id).Select(t => new AccountDetailsViewModel.Transaction
            {
                Date = t.Date,
                Type = t.Type,
                Operation = t.Operation,
                Amount = t.Amount,
                Balance = t.Balance,
                Symbol = t.Symbol,
                Bank = t.Bank,
                Account = t.Account
            }).OrderByDescending(t => t.Date);

            var pageCount = (double)query.Count() / Transactions;

            model.PagingViewModel.MaxPages = (int)Math.Ceiling(pageCount);

            //model.Transactions = query.Skip((currentPage - 1) * Transactions).Take(Transactions).ToList();

            model.Transactions = query.Take(Transactions * currentPage).ToList();

            model.PagingViewModel.CurrentPage = currentPage;

            return View(model);
        }

        public IActionResult Next(int id, string page)
        {
            int currentPage = string.IsNullOrEmpty(page) ? 1 : Convert.ToInt32(page);

            var model = new AccountDetailsViewModel();

            model.CustomerId = id;

            var query = _context.Transactions.Where(t => t.AccountId == id).Select(t => new AccountDetailsViewModel.Transaction
            {
                Date = t.Date,
                Type = t.Type,
                Operation = t.Operation,
                Amount = t.Amount,
                Balance = t.Balance,
                Symbol = t.Symbol,
                Bank = t.Bank,
                Account = t.Account
            }).OrderByDescending(t => t.Date);

            var pageCount = (double)query.Count() / Transactions;

            model.PagingViewModel.MaxPages = (int)Math.Ceiling(pageCount);

            //model.Transactions = query.Skip((currentPage - 1) * Transactions).Take(Transactions).ToList();

            model.Transactions = query.Take(Transactions * currentPage).ToList();

            model.PagingViewModel.CurrentPage = currentPage;

            return View("_AccountDetailsPartial", model);
        }



        public IActionResult AddCustomer()
        {

            return View();
        }

        public IActionResult EditCustomer(int Id)
        {
            var model = new EditCustomerViewModel();

            var customer = _repository.GetCustomer(Id).Select(customer => new EditCustomerViewModel.Customer
            {
                CustomerId = customer.CustomerId,
                Gender = customer.Gender,
                Givenname = customer.Givenname,
                Surname = customer.Surname,
                Streetaddress = customer.Streetaddress,
                City = customer.City,
                Zipcode = customer.Zipcode,
                Country = customer.Country,
                CountryCode = customer.CountryCode,
                Birthday = customer.Birthday,
                NationalId = customer.NationalId,
                Telephonecountrycode = customer.Telephonecountrycode,
                Telephonenumber = customer.Telephonenumber,
                Emailaddress = customer.Emailaddress
            }).FirstOrDefault();


            model.CurrentCustomer = customer;

            return View(model);
        }


        [HttpPost]
        public IActionResult UpdateCustomer(EditCustomerViewModel model)
        {
            var customer = new Customers();

            customer.CustomerId = model.CustomerId;
            customer.Gender = model.Gender;
            customer.Givenname = model.Givenname;
            customer.Surname = model.Surname;
            customer.Streetaddress = model.Streetaddress;
            customer.City = model.City;
            customer.Zipcode = model.Zipcode;
            customer.Country = model.Country;
            customer.CountryCode = model.CountryCode;
            customer.Birthday = model.Birthday;
            customer.NationalId = model.NationalId;
            customer.Telephonecountrycode = model.Telephonecountrycode;
            customer.Telephonenumber = model.Telephonenumber;
            customer.Emailaddress = model.Emailaddress;

            _repository.UpdateCustomer(customer);


            return RedirectToAction("Confirmation");
        }



        public IActionResult Confirmation()
        {
            return View();
        }
    }
}