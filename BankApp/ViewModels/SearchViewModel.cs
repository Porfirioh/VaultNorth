using System;
using System.Collections.Generic;

namespace BankApp.ViewModels
{
    public class SearchViewModel
    {

        public bool ShowSearchResults = false;
        
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public List<SearchResult> SearchResults { get; set; } = new List<SearchResult>();
        public PagingViewModel PagingViewModel { get; set; } = new PagingViewModel();
        public class SearchResult
        {
            public int CustomerId { get; set; }
            public DateTime Birthday { get; set; }
            public string Givenname { get; set; }
            public string Surname { get; set; }
            public string Streetaddress { get; set; }
            public string City { get; set; }
            public string Zipcode { get; set; }
        }

        public string Error { get; set; }
    }
}