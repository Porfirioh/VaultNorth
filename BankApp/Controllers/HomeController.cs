using BankApp.Models;
using BankApp.Repositories;
using BankApp.Services;
using BankApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;

namespace BankApp.Controllers
{
    [Authorize(Roles = "Cashier, Admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BankAppDataContext _context;
        private readonly IBankAppService _services;
        private readonly IBankAppRepository _repository;

        public HomeController(ILogger<HomeController> logger, BankAppDataContext context, IBankAppService services, 
            IBankAppRepository repository)
        {
            _logger = logger;
            _context = context;
            _services = services;
            _repository = repository;
        }
        public IActionResult Index()
        {
            var model = new OverviewViewModel();

            model.TotalBalance = _repository.GetAllAccounts().Sum(account => account.Balance);

            model.NumberOfAccounts = _repository.GetAllAccounts().Count();

            return View(model);
        }
        public IActionResult Overview()
        {
            var model = new OverviewViewModel();

            model.TotalBalance = _repository.GetAllAccounts().Sum(account => account.Balance);

            model.NumberOfAccounts = _repository.GetAllAccounts().Count();

            return View(model);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}