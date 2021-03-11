using BankApp.Models;
using System.Linq;

namespace BankApp.Repositories
{
    public interface IBankAppRepository
    {
        IQueryable<Accounts> GetAllAccounts();
        Accounts GetAccount(int Id);
        void UpdateAccount(Accounts account);
        void CreateTransaction(Transactions transaction);
        IQueryable<Customers> GetAllCustomers();
        IQueryable<Customers> GetCustomer(int Id);
        void UpdateCustomer(Customers customer);
        void DeleteCustomer(Customers customer);
    }
}