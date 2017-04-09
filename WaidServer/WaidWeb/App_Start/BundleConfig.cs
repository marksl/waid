using System.Web.Optimization;

namespace WaidWeb.App_Start
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;
            bundles.Add(new ScriptBundle("~/bundles/jquery", "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.1.min.js")
                .Include("~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/highcharts", "http://code.highcharts.com/2.3.2/highcharts.js")
                .Include("~/Scripts/highcharts.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap", "http://netdna.bootstrapcdn.com/twitter-bootstrap/2.2.1/js/bootstrap.min.js")
                .Include("~/Scripts/bootstrap.js"));

            BundleTable.EnableOptimizations = true;
        }
    }
}