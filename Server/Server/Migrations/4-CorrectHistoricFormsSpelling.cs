using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentMigrator;

namespace Server.Migrations
{
    [Migration(4)]
    public class CorrectHostoricFormsSpelling : Migration
    {
        public override void Up()
        {
            Alter.Table("Places")
                .AddColumn("HistoricForms").AsString(int.MaxValue).Nullable();

            Delete.Column("HitoricForms").FromTable("Places");
        }

        public override void Down()
        {
            Delete.Column("HistoricForms").FromTable("Places");
        }
    }
}