using System.Collections.Generic;
using Domain;
using Nancy;
using Nancy.ModelBinding;
using Server.HAL;
using Simple.Data;

namespace Server.Modules
{
    public class PlaceModule : NancyModule
    {
        public PlaceModule()
        {
            Get["/places"] = ListPlaces;
            Get["/places/{id}"] = GetPlace;
        }

        private object GetPlace(dynamic parameters)
        {
            var place = (Place)Database.Open().Places.FindById((int)parameters.id);
            var representation = new HalBuilder(Request.Url.ToString())
                .AddPublicPropertiesOf(place)
                .Build();

            return Response.AsJson(representation);
        }

        private object ListPlaces(dynamic parameters)
        {
            var searchParameters = this.Bind<SearchPlacesParameters>();
            if (!searchParameters.IsValid) return 400;

            Promise<int> total;
            var places = SearchPlaces(searchParameters, out total);

            return Response.AsJson(BuidlListHal(total, places, searchParameters));
        }

        private object BuidlListHal(Promise<int> total, IEnumerable<Place> places, SearchPlacesParameters parameters)
        {
            var totalPages = total/parameters.PageSize;
            
            var builder = new HalBuilder(Request.Url.ToString())
                .AddProperty("pages", totalPages)
                .AddProperty("page", parameters.Page)
                .EmbedListResourceWithProperties(
                    "places",
                    places,
                    place => Settings.ToAbsolute(string.Format("/places/{0}", place.Id)),
                    place => place.Id,
                    place => place.EnglishName,
                    place => place.CornishName,
                    place => place.Parish);

            if (parameters.Page < totalPages)
                builder.AddLink("next", new SearchUrlBuilder(parameters).NextPage().Build());

            if (parameters.Page > 1)
                builder.AddLink("prev", new SearchUrlBuilder(parameters).PreviousPage().Build());

            return builder.Build();
        }

        private static IEnumerable<Place> SearchPlaces(SearchPlacesParameters searchParameters, out Promise<int> total)
        {
            var db = Database.Open();
            SimpleQuery query = null;

            if (searchParameters.IsEnglishSearch)
                query =
                    db.Places.FindAll(db.Places.EnglishName.Like(searchParameters.Keyword.WithWildcards())).OrderByEnglishName();

            if (searchParameters.IsCornishSearch)
                query =
                    db.Places.FindAll(db.Places.CornishName.Like(searchParameters.CornishKeyword.WithWildcards()))
                        .OrderByCornishName();

            var places = query.Skip((searchParameters.Page.Value - 1) * searchParameters.PageSize.Value)
                .Take(searchParameters.PageSize.Value)
                .WithTotalCount(out total)
                .ToList<Place>();

            return places;
        }

        public class SearchPlacesParameters
        {
            public SearchPlacesParameters()
            {
                
            }

            public SearchPlacesParameters(SearchPlacesParameters old)
            {
                Keyword = old.Keyword;
                CornishKeyword = old.CornishKeyword;
                Page = old.Page;
                PageSize = old.PageSize;
            }

            public string Keyword { get; set; }
            public string CornishKeyword { get; set; }
            public int? Page { get; set; }
            public int? PageSize { get; set; }

            public bool IsValid
            {
                get
                {
                    return (!string.IsNullOrEmpty(Keyword) ^ !string.IsNullOrEmpty(CornishKeyword))
                           && Page.HasValue
                           && PageSize.HasValue;
                }
            }

            public bool IsEnglishSearch
            {
                get
                {
                    return !string.IsNullOrEmpty(Keyword);
                }
            }

            public bool IsCornishSearch
            {
                get
                {
                    return !string.IsNullOrEmpty(CornishKeyword);
                }
            }
        }

        public class SearchUrlBuilder
        {
            private SearchPlacesParameters _params;

            public SearchUrlBuilder(SearchPlacesParameters parameters)
            {
                _params = new SearchPlacesParameters(parameters);
            }

            public SearchUrlBuilder PreviousPage()
            {
                _params.Page--;
                return this;
            }

            public SearchUrlBuilder NextPage()
            {
                _params.Page++;
                return this;
            }

            public string Build()
            {
                if (_params.IsCornishSearch)
                {
                    return Settings.ToAbsolute(string.Format("/places?cornishKeyword={0}&page={1}&pageSize={2}",_params.Keyword,_params.Page,_params.PageSize));
                }

                return Settings.ToAbsolute(string.Format("/places?keyword={0}&page={1}&pageSize={2}", _params.Keyword, _params.Page, _params.PageSize));
            }
        }
    }
}