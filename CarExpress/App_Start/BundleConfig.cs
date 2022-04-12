using System.Web.Optimization;

namespace CarExpress
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/assets/cache/jquery.min.js").Include(
                "~/assets/js/jquery-1.12.0.min.js"
            ));
            bundles.Add(new ScriptBundle("~/assets/cache/custom.min.js").Include(
               "~/assets/js/custom.js"
           ));
            bundles.Add(new ScriptBundle("~/assets/cache/core.min.js").Include(
                "~/assets/js/modernizr-2.8.3.min.js",
                "~/assets/js/popper.js",
                 "~/assets/js/bootstrap.min.js",
                 "~/assets/js/isotope.pkgd.min.js",
                 "~/assets/js/imagesloaded.pkgd.min.js",
                 "~/assets/js/jquery.counterup.min.js",
                 "~/assets/js/waypoints.min.js",
                 "~/assets/js/owl.carousel.min.js",
                  "~/assets/js/jquery.lazy.min.js",
                 "~/assets/js/plugins.js",
                 "~/assets/js/main.js",
                 "~/assets/js/notify.js"
            ));
            bundles.Add(new ScriptBundle("~/assets/cache/validate.min.js").Include(
                "~/assets/js/jquery.validate.js",
                "~/assets/js/jquery.unobtrusive-ajax.js",
                "~/assets/js/jquery.validate.unobtrusive.js"
           ));
            bundles.Add(new StyleBundle("~/assets/cache/style.min.css").Include(
                "~/assets/css/bootstrap.min.css",
                "~/assets/css/animate.css",
                "~/assets/css/owl.carousel.min.css",
                "~/assets/css/chosen.min.css",
                "~/assets/css/jquery-ui.css",
                "~/assets/css/themify-icons.css",
                 "~/assets/css/ionicons.min.css",
                  "~/assets/css/meanmenu.min.css",
                   "~/assets/css/bundle.css",
                   "~/assets/css/style.css",
                   "~/assets/css/responsive.css",
                   "~/assets/css/custom.css"
            ));
        }
    }
}
