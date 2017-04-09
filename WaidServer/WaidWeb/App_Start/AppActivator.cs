using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using WaidWeb.Migrations;
using WaidWeb.Models;
using WebMatrix.WebData;

[assembly: WebActivator.PostApplicationStartMethod(typeof(WaidWeb.App_Start.AppActivator), "PostStart")]

namespace WaidWeb.App_Start
{
    public class AppActivator
    {
        public static void PostStart()
        {
            Database.SetInitializer<UsersContext>(null);

            try
            {
                using (var context = new UsersContext())
                {
                    if (!context.Database.Exists())
                    {
                        // Create the SimpleMembership database without Entity Framework migration schema
                        ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                    }
                }

                WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName",
                                                         autoCreateTables: true);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588",
                    ex);
            }

            //var dbMigrator = new DbMigrator(new Configuration());
            //dbMigrator.Update();
        }
    }
}
