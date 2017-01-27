// This file can be modified in any way, with two exceptions. 1) The name of
// this class must be "ModuleInitializer". 2) This class must have a public or
// internal parameterless "Run" method that returns void. In addition to these
// modifications, this file may also be moved to any location, as long as it
// remains a part of its current project.

using System;
using System.Net.Mime;
using IntegrationTests;
using Ujo.Repository.Migrations;

namespace Ujo.Repository.Tests
{
    //Module initialiser info: http://www.fredonism.com/archive/run-code-once-before-and-after-all-tests-with-xunit/
    //Database local initialiser info: https://martinwilley.com/net/code/localdbtest.html
    public sealed class ModuleInitializer : IDisposable
    {
        static ModuleInitializer()
        {
            Current = new ModuleInitializer();
        }

        public static ModuleInitializer Current { get; private set; }

        internal static void Run()
        {
        }

        private ModuleInitializer()
        {
            var testDatabase = new TestDatabase("Ujo");
            testDatabase.CreateDatabase();
            //globally inject a connection string with this name
           //this is not working using hardcoded settings
           //testDatabase.InitConnectionString("Ujo");
            //if we're using Entity Framework Code First, run all the migrations.
            var migrate =
                new System.Data.Entity.MigrateDatabaseToLatestVersion<UjoContext,
                    Configuration>();
            var dbContext = new UjoContext();
            migrate.InitializeDatabase(dbContext);
        }

        ~ModuleInitializer()
        {
            Dispose();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            // Assembly teardown code goes here
            var testDatabase = new TestDatabase("Ujo");
            testDatabase.CleanupDatabase();
           
        }
    }
}