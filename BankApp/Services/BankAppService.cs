using BankApp.Models;
using BankApp.Repositories;
using System;

namespace BankApp.Services
{
    public class BankAppService : IBankAppService
    {
        private readonly IBankAppRepository _repository;

        public BankAppService(IBankAppRepository repository)
        {
            _repository = repository;
        }

        public bool Transfer(Accounts fromAccount, Accounts toAccount, Transactions transaction)
        {
            if (HasValidBalance(fromAccount, transaction))
            {
                var didSucceed = CreateTransaction(fromAccount, toAccount, transaction);

                if (didSucceed)
                    return true;
            }

            return false;
        }

        public bool Withdraw(Accounts account, Transactions transaction)
        {
            if (HasValidBalance(account, transaction))
            {
                var didSucceed = CreateWithdrawal(account, transaction);

                if (didSucceed)
                    return true;
                
            }

            return false;
        }

        private bool CreateCollection(Accounts account, Transactions transaction)
        {
            account.Balance = (account.Balance + transaction.Amount);

            _repository.UpdateAccount(account);

            transaction.Type = "Credit";

            transaction.Balance = account.Balance;

            _repository.CreateTransaction(transaction);

            return true;
        }

        private bool IsPositive(decimal amount)
        {
            if (amount < 0)
                return false;
            else
                return true;
        }

        private bool CreateTransaction(Accounts fromAccount, Accounts toAccount, Transactions transaction)
        {
            bool didSucceed;

            didSucceed = CreateWithdrawal(fromAccount, transaction);

            if(didSucceed)
            {
                var toTransaction = new Transactions()
                {
                    AccountId = toAccount.AccountId,
                    Date = transaction.Date,
                    Type = "Credit",
                    Operation = transaction.Operation,
                    Amount = Math.Abs(transaction.Amount),
                    Balance = toAccount.Balance,
                    Symbol = transaction.Symbol,
                    Bank = transaction.Bank,
                    Account = transaction.AccountId.ToString()
                };
                
                didSucceed = CreateCollection(toAccount, toTransaction);

                if (didSucceed)
                    return true;
            }

            return false;
        }

        public bool HasValidBalance(Accounts account, Transactions transaction)
        {
            if ((account.Balance - transaction.Amount) < 0)
                return false;
            else
                return true;
        }

        private bool CreateWithdrawal(Accounts account, Transactions transaction)
        {

            account.Balance = (account.Balance - transaction.Amount);

            _repository.UpdateAccount(account);

            transaction.Balance = account.Balance;

            transaction.Type = "Debit";

            transaction.Amount = (0 - transaction.Amount);

            _repository.CreateTransaction(transaction);

            return true;
        }

        public bool Deposit(Accounts account, Transactions transaction)
        {
            var result = CreateCollection(account, transaction);

            return result;
        }
    }
}