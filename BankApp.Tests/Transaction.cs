using BankApp.Controllers;
using BankApp.Models;
using BankApp.Repositories;
using BankApp.Services;
using BankApp.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace BankApp.Tests
{
    [TestClass]
    public class Transaction
    {
        private Mock<IBankAppRepository> bankAppRepository;
        private Mock<IBankAppService> bankAppService;
        private TransactionController sut;

        [TestInitialize]
        public void Initialize()
        {
            bankAppRepository = new Mock<IBankAppRepository>();
            bankAppService = new Mock<IBankAppService>();
            sut = new TransactionController(bankAppService.Object, bankAppRepository.Object);
        }

        [TestMethod]
        public void Withdrawal_in_cash_should_withdraw()
        {
            var model = new TransactionViewModel();

            model.AccountId = 1;
            model.Date = DateTime.Now;
            model.Amount = 300;
            model.Operation = "Withdrawal in Cash";

            sut.WithdrawalInCash(model);

            var account = new Accounts();

            account.AccountId = 1;

            bankAppRepository.Setup(a => a.GetAccount(model.AccountId)).Returns(account);

            bankAppService.Verify(b => b.Withdraw(It.IsAny<Accounts>(), It.IsAny<Transactions>()), Times.Once);
        }
        public void Transaction_should_transfer()
        {
            var model = new TransactionViewModel();

            model.AccountId = 1;
            model.Date = DateTime.Now;
            model.Amount = 300;
            model.Operation = "Transaction";
            model.Account = 2;

            sut.Transaction(model);

            var fromAccount = new Accounts();
            fromAccount.AccountId = 1;

            var toAccount = new Accounts();
            toAccount.AccountId = 2;

            bankAppRepository.Setup(a => a.GetAccount(model.AccountId)).Returns(fromAccount);
            bankAppRepository.Setup(a => a.GetAccount(Convert.ToInt32(model.Account))).Returns(toAccount);

            bankAppService.Verify(b => b.Transfer(It.IsAny<Accounts>(), It.IsAny<Accounts>(), It.IsAny<Transactions>()), Times.Once);
        }

        [TestMethod]
        public void Transfer_should_not_be_called_if_model_state_is_not_valid()
        {
            sut.ModelState.AddModelError("1", "Invalid input");

            bankAppService.Verify(b => b.Transfer(It.IsAny<Accounts>(), It.IsAny<Accounts>(), It.IsAny<Transactions>()), Times.Never);

        }
    }
}

