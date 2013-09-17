using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Server.Domain;
using Simple.Data;

namespace Server
{
    public class PlaceModule : NancyModule
    {
        public PlaceModule()
        {
            Get["/places"] = ListPlaces;
        }

        private object ListPlaces(dynamic parameters)
        {
            if (Request.Query.keyword.HasValue)
                return Response.AsJson(FindByEnglishName((string)Request.Query.keyword));

            if (Request.Query.cornishKeyword.HasValue)
                return Response.AsJson(FindByCornishName((string) Request.Query.cornishKeyword));

            return 500;
        }

        private static IEnumerable<Place> FindByEnglishName(string name)
        {
            var db = Database.Open();
            IEnumerable<Place> places = db.Places.FindAll(db.Places.Name.Like(name.WithWildcards())).OrderByName();
            return places;
        }

        private static IEnumerable<Place> FindByCornishName(string name)
        {
            var db = Database.Open();
            IEnumerable<Place> places = db.Places.FindAll(db.Places.CornishForm.Like(name.WithWildcards())).OrderByCornishForm();
            return places;
        }
    }
}