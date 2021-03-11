using BankApp.Models;
using BankApp.Repositories;
using BankApp.Services;
using BankApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BankApp.Controllers
{
    [Authorize(Roles = "Cashier")]
    public class TransactionController : Controller
    {
        private readonly IBankAppService _service;
        private readonly IBankAppRepository _repository;

        public TransactionController(IBankAppService service, IBankAppRepository repository)
        {
            _service = service;
            _repository = repository;
        }

        public IActionResult Transactions()
        {
            return View();
        }

        public IActionResult Deposit()
        {
            var model = new TransactionViewModel() { Date = DateTime.Now };

            return View(model);
        }
        public IActionResult WithDrawalInCash()
        {
            var model = new TransactionViewModel() { Date = DateTime.Now };

            return View(model);
        }
        public IActionResult Transaction()
        {
            var model = new TransactionViewModel() { Date = DateTime.Now };

            model.Operation = "Transaction";

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Transaction(TransactionViewModel model)
        {

            if (model.Account <= 0)
            {

                ViewBag.Account = "Endast mellan 1 och 1000000";

                return View();
                
            }

            if(model.AccountId == model.Account)
            {
                ViewBag.AccountId = "Avsändare kan inte vara samma som mottagare";
                ViewBag.Account = "Mottagare kan inte vara samma som avsändare";

                return View();
            }



            if (ModelState.IsValid)
            {
                var transaction = new Transactions()
                {
                    AccountId = model.AccountId,
                    Date = model.Date,
                    Operation = model.Operation,
                    Amount = model.Amount,
                    Balance = model.Balance,
                    Symbol = model.Symbol,
                    Bank = model.Bank,
                    Account = model.Account.ToString()
                };

                
                var fromAccount = _repository.GetAccount(transaction.AccountId);

                var toAccount = _repository.GetAccount(Convert.ToInt32(transaction.Account));

                
                if (fromAccount == null)
                {
                    ViewBag.AccountId = "Avsändaren kunde inte hittas";
                    return View();
                }

                if (toAccount == null)
                {
                    ViewBag.Account = "Mottagaren kunde inte hittas";
                    return View();
                }
                
                var hasValidBalance = _service.HasValidBalance(fromAccount, transaction);

                if(!hasValidBalance)
                {
                    model.TransactionErrorMessage = "Avsändaren saknar täckning";
                    return View(model);
                }


                var didSucceed = _service.Transfer(fromAccount, toAccount, transaction);

                if (didSucceed)
                    model.ConfirmationMessage = "Genomfördes utan problem";
                else
                    model.ErrorMessage = "Kunde inte genomföras";

                return RedirectToAction("Feedback", model);
            }

            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult WithdrawalInCash(TransactionViewModel model)
        {

            var transaction = new Transactions()
            {
                AccountId = model.AccountId,
                Date = model.Date,
                Operation = model.Operation,
                Amount = model.Amount,
                Balance = model.Balance,
                Symbol = model.Symbol,
                Bank = model.Bank,
                Account = model.Account.ToString()
            };

            if (ModelState.IsValid)
            {
                var account = _repository.GetAccount(transaction.AccountId);

                var didSucceed = _service.Withdraw(account, transaction);

                if (didSucceed)
                    model.ConfirmationMessage = "Genomfördes utan problem";
                else
                    model.ErrorMessage = "Kunde inte genomföras";
                
                return RedirectToAction("Feedback", model);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Deposit(TransactionViewModel model)
        {

            var transaction = new Transactions()
            {
                AccountId = model.AccountId,
                Date = model.Date,
                Operation = "Deposit",
                Amount = model.Amount,
                Balance = model.Balance,
                Symbol = model.Symbol,
                Bank = model.Bank,
                Account = model.Account.ToString()
            };

            if (ModelState.IsValid)
            {
                var account = _repository.GetAccount(transaction.AccountId);

                var didSucceed = _service.Deposit(account, transaction);

                if (didSucceed)
                    model.ConfirmationMessage = "Genomfördes utan problem";
                else
                    model.ErrorMessage = "Kunde inte genomföras";

                return RedirectToAction("Feedback", model);
            }

            return View();
        }


        public IActionResult Feedback(TransactionViewModel model)
        {
            ViewBag.Message = "Hej";
            return View(model);
        }
    }
}