namespace UCEME
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        // Para obtener más información acerca de Bundling, consulte http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/shared").Include(
                        "~/Scripts/Internal/Shared/GoogleAnalytics.js",
                        "~/Scripts/Internal/Shared/NavBar.js",
                        "~/Scripts/Internal/Shared/RefreshPage.js",
                        "~/Scripts/Internal/Shared/HeaderShrinker.js",
                        "~/Scripts/Internal/Shared/DataLoader.js",
                        "~/Scripts/Internal/Shared/ItemManager.js",
                        "~/Scripts/Internal/Shared/MediaManager.js"));

            bundles.Add(new ScriptBundle("~/bundles/slidemenu").Include(
                        "~/Scripts/Internal/Shared/slide.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrapjs").Include(
                        "~/Scripts/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información sobre los formularios. De este modo, estará
            // preparado para la producción y podrá utilizar la herramienta de creación disponible en http://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-2.8.3.js"));

            bundles.Add(new ScriptBundle("~/bundles/shims").Include(
                        "~/Scripts/External/es5-shim.min.js",
                        "~/Scripts/External/es5-sham.min.js",
                        "~/Scripts/External/es6-shim.min.js",
                        "~/Scripts/External/es6-sham.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/normalize.css",
                "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrapcss").Include(
                "~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/jqueryui").Include(
                "~/Content/themes/base/jquery-ui.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            //Scripts.DefaultTagFormat = "<script src='{0}' async></script>";

            BundleTable.EnableOptimizations = true;
        }
    }
}