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
            if (!Request.Query.keyword.HasValue) return 500;

            var keyword = "%" + Request.Query.keyword + "%";

            var db = Database.Open();
            IEnumerable<Place> places = db.Places.FindAll(db.Places.Name.Like(keyword) || db.Places.CornishForm.Like(keyword));
            return Response.AsJson(places);
        }
    }
}