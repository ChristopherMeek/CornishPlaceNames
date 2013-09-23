using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain;
using Nancy;
using Simple.Data;

namespace Server
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
            return Response.AsJson((Place)Database.Open().Places.FindById((int)parameters.id));
        }

        private object ListPlaces(dynamic parameters)
        {
            if (Request.Query.keyword.HasValue)
                return Response.AsJson(FindByEnglishName((string)Request.Query.keyword));

            if (Request.Query.cornishKeyword.HasValue)
                return Response.AsJson(FindByCornishName((string)Request.Query.cornishKeyword));

            return 500;
        }

        private static IEnumerable<Place> FindByEnglishName(string name)
        {
            var db = Database.Open();
            IEnumerable<Place> places = db.Places.FindAll(db.Places.EnglishName.Like(name.WithWildcards())).OrderByEnglishName();
            return places;
        }

        private static IEnumerable<Place> FindByCornishName(string name)
        {
            var db = Database.Open();
            IEnumerable<Place> places = db.Places.FindAll(db.Places.CornishName.Like(name.WithWildcards())).OrderByCornishName();
            return places;
        }
    }
}