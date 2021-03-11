using BankApp.Models;

namespace BankApp.Services
{
    public interface IBankAppService
    {
        bool Transfer(Accounts fromAccount, Accounts toAccount, Transactions transaction);
        bool Withdraw(Accounts account, Transactions transaction);
        bool Deposit(Accounts account, Transactions transaction);
        bool HasValidBalance(Accounts account, Transactions transaction);
    }
}