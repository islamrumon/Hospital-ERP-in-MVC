using System.Web;
using System.Web.Optimization;

namespace ASITHmsMvc
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/assets/js/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/assets/js/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/assets/js/bootstrap.js",
                      "~/assets/js/custom.js",
                      "~/assets/js/edit.js",
                      "~/assets/js/charts.js",
                      "~/assets/jquery-ui.js", "~/assets/js/date.formet.js", "~/assets/simple.money.format.js"));

            //data table design bootstrap
            bundles.Add(new ScriptBundle("~/bundles/datatable").Include(
                "~/assets/js/jquery.dataTables.js", "~/assets/js/dataTables.bootstrap.js"));
                



            bundles.Add(new StyleBundle("~/Content/css").Include(
                       "~/assets/css/font-awesome.css",
                      "~/assets/css/bootstrap.css",
                      "~/assets/css/main.css",
                      "~/assets/css/dashboard.css",
                      "~/assets/css/login-register.css",
                      "~/assets/css/dataTables.bootstrap.css",
                      "~/assets/jquery-ui.css", 
                       "~/assets/css/Site.css"));

            ////Kendo add css
            //bundles.Add(new StyleBundle("~/css/kendo").Include(
            //    "~/assets/kendo/kendo.common.min.css",
            //    "~/assets/kendo/kendo.rtl.min.css",
            //    "~/assets/kendo/kendo.default.min.css",
            //    "~/assets/kendo/kendo.mobile.all.min.css"
            //    ));

            ////kendo add js
            //bundles.Add(new ScriptBundle("~/js/kendo").Include(
            //    "~/assets/kendo/angular.min.js",
            //    "~/assets/kendo/jszip.min.js",
            //    "~/assets/kendo/kendo.all.min.js"
            //    ));

        }
    }
}
