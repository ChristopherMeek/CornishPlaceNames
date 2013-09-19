using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentMigrator;

namespace Server.Migrations
{
    [Migration(1)]
    public class RemoveImportedPlacesTable : Migration
    {
        public override void Up()
        {
            if(Schema.Table("Places").Exists())
                Delete.Table("Places");
        }

        public override void Down() { }
    }
}