using System;
using System.ComponentModel.DataAnnotations;

namespace BankApp.ViewModels
{
    public class CorrectDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var inputDate = Convert.ToDateTime(value);

            var shortDate = DateTime.Now.ToShortDateString();

            var currentDate = Convert.ToDateTime(shortDate);

            return inputDate >= currentDate;
        }
    }
    public class TransactionViewModel
    {

        [Required(ErrorMessage = "Fältet är obligatoriskt")]
        [Range(1, 1000000, ErrorMessage = "Endast mellan 1 och 1000000")]
        public int AccountId { get; set; }

        [CorrectDate(ErrorMessage = "Endast från och med dagens datum")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Fältet är obligatoriskt")]
        [Range(1, 1000000, ErrorMessage = "Endast mellan 1 och 1000000")]
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public string Symbol { get; set; }
        public string Bank { get; set; }
        public int Account { get; set; }
        public string ErrorMessage { get; set; }
        public string TransactionErrorMessage { get; set; }
        public string ConfirmationMessage { get; set; }
        public string Operation { get; set; }
    }
}