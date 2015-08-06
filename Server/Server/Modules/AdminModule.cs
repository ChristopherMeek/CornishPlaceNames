using System.Collections.Generic;
using System.Linq;
using Domain;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using Server.Models;
using Simple.Data;

namespace Server.Modules
{
    public class AdminModule : NancyModule
    {
        public AdminModule()
        {
            this.RequiresAuthentication();
            Get["/admin"] = Admin;
            Post["/admin/search"] = Admin;
            Get["/admin/edit/{id}"] = Edit;
            Post["/admin/edit/{id}"] = Save;

            Get["/admin/resources"] = Resources;
            Post["/admin/resources"] = SaveResources;
        }

        private object SaveResources(object arg)
        {
            var resources = this.Bind<IList<Resource>>();

            return Response.AsRedirect("/admin/resources");
        }

        private object Admin(dynamic parameters)
        {
            string searchText = Request.Form.SearchText;
            if (!string.IsNullOrEmpty(searchText))
                return View[new SearchModel(searchText, Search(searchText))];
            
            return View[new SearchModel()];
        }

        private object Edit(dynamic parameters)
        {
            var place = (Place)Database.Open().Places.FindById((int)parameters.id);
            return View[new EditModel(place)];
        }

        private object Save(dynamic parameters)
        {
            var place = this.Bind<Place>();
            Database.Open().Places.UpdateById(place);
            return View[new EditModel(place)];
        }

        private static IEnumerable<Place> Search(string searchText)
        {
            var db = Database.Open();
            SimpleQuery query = db.Places.FindAll(db.Places.EnglishName.Like(searchText.WithWildcards())).OrderByEnglishName();
            return query.ToList<Place>();
        }

        private object Resources(dynamic parameters)
        {
            var db = Database.Open();
            IList<Resource> resources = db.Resources.All().ToList<Resource>();
            return View[new ResourcesModel(resources)];
        }
    }
}