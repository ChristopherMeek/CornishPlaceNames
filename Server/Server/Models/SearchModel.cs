using System.Collections.Generic;
using System.Web.UI;
using Domain;

namespace Server.Models
{
    public class SearchModel
    {
        public SearchModel()
        {
            SearchText = string.Empty;
            Places = new List<Place>();
        }

        public SearchModel(string searchText, IEnumerable<Place> places)
        {
            SearchText = searchText;
            Places = places;
        }

        public string SearchText { get; set; }
        public IEnumerable<Place> Places { get; set; }
    }
}