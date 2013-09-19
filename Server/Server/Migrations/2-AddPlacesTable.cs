using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentMigrator;

namespace Server.Migrations
{
    [Migration(2)]
    public class AddPlacesTable : Migration
    {
        public override void Up()
        {
            Create.Table("Places")
                .WithColumn("Id").AsInt32().PrimaryKey().NotNullable()
                .WithColumn("EnglishName").AsString(int.MaxValue).Nullable()
                .WithColumn("Type").AsString(int.MaxValue).Nullable()
                .WithColumn("Parish").AsString(int.MaxValue).Nullable()
                .WithColumn("Keverang").AsString(int.MaxValue).Nullable()
                .WithColumn("GridReference").AsString(int.MaxValue).Nullable()
                .WithColumn("CornishName").AsString(int.MaxValue).Nullable()
                .WithColumn("Administration").AsString(int.MaxValue).Nullable()
                .WithColumn("Notes").AsString(int.MaxValue).Nullable();
        }

        public override void Down()
        {
            Delete.Table("Places");
        }
    }
}