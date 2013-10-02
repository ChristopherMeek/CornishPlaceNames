using System;
using System.Web.Management;
using Nancy;
using Server.HAL;

namespace Server.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = Home;
        }

        private object Home(dynamic parameters)
        {
            var representation = new HalBuilder(Settings.ToAbsolute("/"))
                .AddLink("places-english", Settings.ToAbsolute("/places?keyword={keyword}&page={page}&pageSize={pageSize}"), templated: true)
                .AddLink("places-cornish", Settings.ToAbsolute("/places?cornishKeyword={keyword}&page={page}&pageSize={pageSize}"), templated: true)
                .AddLink("license", "http://creativecommons.org/licenses/by/3.0/")
                .AddProperty("description", "Welcome to the Cornish Places Names API")
                .Build();

            return Response.AsJson(representation);
        }
    }
}