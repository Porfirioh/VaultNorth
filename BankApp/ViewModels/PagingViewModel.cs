namespace BankApp.ViewModels
{
    public class PagingViewModel
    {
        public int CurrentPage { get; set; }
        public int MaxPages { get; set; }

        public bool ShowPreviousButton
        {
            get { return CurrentPage > 1; }
        }
        public bool ShowNextButton
        {
            get { return CurrentPage < MaxPages; }
        }
    }
}