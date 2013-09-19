using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace Server
{
    public class MigratingBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            var announcer = new NullAnnouncer();
            var assembly = Assembly.GetExecutingAssembly();

            var migrationContext = new RunnerContext(announcer)
            {
                Namespace = "Server.Migrations"
            };

            var options = new MigrationOptions();
            var factory = new FluentMigrator.Runner.Processors.SqlServer.SqlServer2008ProcessorFactory();
            var processor = factory.Create(
                ConfigurationManager.ConnectionStrings["Simple.Data.Properties.Settings.DefaultConnectionString"].ConnectionString, 
                announcer, 
                options);
            var runner = new MigrationRunner(assembly, migrationContext, processor);
            runner.MigrateUp(true);
        }
    }

    public class MigrationOptions : IMigrationProcessorOptions
    {
        public MigrationOptions()
        {
            PreviewOnly = false;
            Timeout = 60;
        }

        public bool PreviewOnly { get; private set; }
        public int Timeout { get; private set; }
        public string ProviderSwitches { get; private set; }
    }
}