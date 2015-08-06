using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain;

namespace Server.Models
{
    public class ResourcesModel
    {
        public ResourcesModel(IList<Resource> resources)
        {
            Resources = resources.Select((r, i) => new ResourceModel {Index = i, Resource = r}).ToList();
        }

        public IList<ResourceModel> Resources { get; set; }

        public int NextIndex
        {
            get { return Resources.Select(r => r.Index).Max() + 1; }
        }
    }

    public class ResourceModel
    {
        public int Index { get; set; }
        public Resource Resource { get; set; }
    }
}