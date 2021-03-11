using BankApp.Models;
using BankApp.Repositories;
using BankApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BankApp.Tests
{
    [TestClass]
    public class BankAppService
    {
        private Mock<IBankAppRepository>  bankAppRepository;
        private IBankAppService sut;

        [TestInitialize]
        public void Initialize()
        {
            bankAppRepository = new Mock<IBankAppRepository>();
            sut = new Services.BankAppService(bankAppRepository.Object);
        }

        [TestMethod]
        public void Withdrawa_should_return_true_if_account_balance_is_valid()
        {
            var account = new Accounts();
            var transaction = new Transactions();

            account.Balance = 300;
            transaction.Amount = 300;
            transaction.Operation = "Withdrawal in Cash";

            Assert.IsTrue(sut.Withdraw(account, transaction));
        }

        [TestMethod]
        public void Withdraw_should_return_false_if_account_balance_is_invalid()
        {
            var account = new Accounts();
            var transaction = new Transactions();

            account.Balance = 0;
            transaction.Amount = 300;
            transaction.Operation = "Withdrawal in Cash";

            Assert.IsFalse(sut.Withdraw(account, transaction));
        }

        [TestMethod]
        public void Transfer_should_return_false_if_account_balance_is_invalid()
        {
            var fromAccount = new Accounts();
            fromAccount.Balance = 0;

            var toAccount = new Accounts();

            var transaction = new Transactions();
            transaction.Amount = 300;

            Assert.IsFalse(sut.Transfer(fromAccount, toAccount, transaction));
        }

        [TestMethod]
        public void Transfer_should_return_true_if_account_balance_is_valid()
        {
            var fromAccount = new Accounts();
            fromAccount.Balance = 300;
            
            var toAccount = new Accounts();

            var transaction = new Transactions();
            transaction.Amount = 300;

            Assert.IsTrue(sut.Transfer(fromAccount, toAccount, transaction));
        }
    }
}

