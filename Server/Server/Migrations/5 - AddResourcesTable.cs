using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentMigrator;

namespace Server.Migrations
{
    [Migration(5)]
    public class AddResourcesTable : Migration
    {
        public override void Up()
        {
            Create.Table("Resources")
                .WithColumn("Name").AsString(int.MaxValue).NotNullable()
                .WithColumn("Description").AsString(int.MaxValue).NotNullable()
                .WithColumn("URL").AsString(int.MaxValue).NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Resource");
        }
    }
}