using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;

namespace Server.HAL
{
    public class HalBuilder
    {
        private dynamic _representation = new ExpandoObject();

        public HalBuilder(string selfUrl)
        {
            dynamic links = new ExpandoObject();
            links.self = new { href = selfUrl };
            _representation._links = links;
        }

        public object Build()
        {
            return _representation;
        }

        public HalBuilder AddPublicPropertiesOf<T>(T resource)
        {
            foreach (var propertyInfo in resource.GetType().GetProperties())
            {
                if (propertyInfo.GetGetMethod() != null)
                {
                    RepresentationAsDictionary.Add(propertyInfo.Name, propertyInfo.GetGetMethod().Invoke(resource, null));
                }
            }
            return this;
        }

        public HalBuilder AddProperty(string name, object value)
        {
            RepresentationAsDictionary.Add(name, value);
            return this;
        }

        public HalBuilder EmbedListResourceWithProperties<T>(string name, IEnumerable<T> resources, Func<T, string> selfLinkFactory, params Expression<Func<T, object>>[] propertiesToInclude)
        {
            IList<dynamic> itemRepresentations = new List<dynamic>();

            foreach (var item in resources)
            {
                var builder = new HalBuilder(selfLinkFactory(item));

                foreach (var property in propertiesToInclude)
                {
                    var propertyName = GetPropertyName(property);
                    builder.AddProperty(propertyName, property.Compile()(item));
                }

                itemRepresentations.Add(builder.Build());
            }

            EmbeddedAsDictionary.Add(name, itemRepresentations);

            return this;
        }

        private static string GetPropertyName<T>(Expression<Func<T, object>> property)
        {
            var expression = property.Body;

            MemberExpression memberExpression = null;
            if (expression.NodeType == ExpressionType.Convert)
            {
                var body = (UnaryExpression)expression;
                memberExpression = body.Operand as MemberExpression;
            }
            else if (expression.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression as MemberExpression;
            }

            return memberExpression.Member.Name;
        }

        private IDictionary<string, object> RepresentationAsDictionary
        {
            get
            {
                return (IDictionary<string, object>)_representation;
            }
        }

        private IDictionary<string, object> EmbeddedAsDictionary
        {
            get
            {
                if (!RepresentationAsDictionary.ContainsKey("_embedded"))
                {
                    var embedded = new ExpandoObject();
                    _representation._embedded = embedded;
                }

                return (IDictionary<string, object>)(_representation._embedded);
            }
        }

        private IDictionary<string, object> LinksAsDictionary
        {
            get
            {
                if (!RepresentationAsDictionary.ContainsKey("_links"))
                {
                    var links = new ExpandoObject();
                    _representation._links = links;
                }

                return (IDictionary<string, object>)(_representation._links);
            }
        }

        public HalBuilder AddLink(string rel, string href, bool templated = false)
        {
            LinksAsDictionary.Add(rel, new { href, templated });

            return this;
        }
    }
}