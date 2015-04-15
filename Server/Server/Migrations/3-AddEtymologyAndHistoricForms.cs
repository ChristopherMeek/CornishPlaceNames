using FluentMigrator;

namespace Server.Migrations
{
    [Migration(3)]
    public class AddEtymologyAndHistoricFormsColumns : Migration
    {
        public override void Up()
        {
            Alter.Table("Places")
                .AddColumn("Etymology").AsString(int.MaxValue).Nullable()
                .AddColumn("HitoricForms").AsString(int.MaxValue).Nullable();
        }

        public override void Down()
        {
            Delete.Column("Etymology").FromTable("Places");
            Delete.Column("HistoricForms").FromTable("Places");
        }
    }
}