// Copyright(c) Daniel Veintimilla 2016.

#region usings

using System.Web.Optimization;

#endregion

namespace StockTicker
{
    /// <summary>Bundles the js and css files into Script/Style bundles.</summary>
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/datatables.js",
                "~/Scripts/accounting.min.js",
                "~/Scripts/jquery.notify.min.js",
                "~/Scripts/tiktok.js",
                "~/Scripts/loadingoverlay.js",
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/App").Include(
                "~/Scripts/App/Notifications.js",
                "~/Scripts/App/StockHandler.js",
                "~/Scripts/App/StockMapper.js",
                "~/Scripts/App/MessageProcessor.js",
                "~/Scripts/App/CommonUtils.js",
                "~/Scripts/App/UserLoginHandler.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/bootstrap-select.min.js",
                "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/font-awesome.min.css",
                "~/Content/bootstrap-select.min.css",
                "~/Content/jquery.notify.css",
                "~/Content/jquery.dataTables.css",
                "~/Content/tiktok.css",
                "~/Content/loadingoverlay.css",
                "~/Content/dataTables.bootstrap.min.css",
                "~/Content/site.css"));

            BundleTable.EnableOptimizations = false;
        }
    }
}
