using BankApp.Models;
using System.Linq;

namespace BankApp.Repositories
{
    public class BankAppRepository : IBankAppRepository
    {
        private readonly BankAppDataContext _context;
        public BankAppRepository(BankAppDataContext context)
        {
            _context = context;
        }
        public Accounts GetAccount(int Id)
        {
            return _context.Accounts.FirstOrDefault(account => account.AccountId == Id);
        }

        public IQueryable<Accounts> GetAllAccounts()
        {
            return _context.Accounts;
        }

        public void UpdateAccount(Accounts account)
        {
            _context.Update(account);
            _context.SaveChanges();

        }
        public void CreateTransaction(Transactions transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
        }

        public IQueryable<Customers> GetAllCustomers()
        {
            return _context.Customers;
        }

        public IQueryable<Customers> GetCustomer(int Id)
        {
            return _context.Customers.Where(customer => customer.CustomerId == Id);
        }

        public void UpdateCustomer(Customers customer)
        {
            _context.Update(customer);
            _context.SaveChanges();
        }

        public void DeleteCustomer(Customers customer)
        {
            _context.Remove(customer);
            _context.SaveChanges();
        }
    }
}