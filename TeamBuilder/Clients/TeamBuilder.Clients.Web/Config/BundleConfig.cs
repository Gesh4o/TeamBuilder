namespace TeamBuilder.Web
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery")
                .Include("~/Scripts/jquery-{version}.js")
                .Include("~/Scripts/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval")
                .Include("~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/plugins")
                .Include(
                    "~/Scripts/jquery-migrate-1.2.1.min.js",
                    "~/Scripts/jquery.easing.1.3.js",
                    "~/Scripts/jquery.scrollTo.min.js",
                    "~/Scripts/prism.js",
                    "~/Scripts/custom/main.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr")
                .Include("~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/sweetalert")
                .Include("~/lib/sweetalert2/dist/sweetalert2.min.js"));

            bundles.Add(
                new ScriptBundle("~/bundles/bootstrap")
                .Include(
                    "~/Scripts/bootstrap.js", 
                    "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css")
                .Include(
                    "~/Content/bootstrap.css", 
                    "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/styles")
                .Include(
                    "~/Content/styles.css", "~/lib/sweetalert2/dist/sweetalert2.min.css"));

            bundles.Add(new StyleBundle("~/Content/plugins")
                .Include(
                    "~/Content/plugins/font-awesome/css/font-awesome.css",
                    "~/Content/plugins/prism/prism.css"));
        }
    }
}