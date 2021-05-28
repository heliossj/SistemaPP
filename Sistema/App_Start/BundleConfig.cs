using System.Web;
using System.Web.Optimization;

namespace Sistema
{
    public class BundleConfig
    {
        // Para obter mais informações sobre o agrupamento, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap.min.css",
                      "~/Content/font-awesome.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/dataTables.bootstrap4.min.css",
                      "~/Content/bootstrap-datepicker.css",
                      "~/Content/bootstrap-datepicker3.css",
                      "~/Content/css/select2.css",
                      "~/Content/css/select2.custom.css",
                      "~/Content/css/select2-bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/Sistema.css"
                      ));
            // Use a versão em desenvolvimento do Modernizr para desenvolver e aprender. Em seguida, quando estiver
            // pronto para a produção, utilize a ferramenta de build em https://modernizr.com para escolher somente os testes que precisa.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*",
                        "~/Scripts/jquery-3.5.1.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                        "~/Scripts/jquery.dataTables.min.js",
                        "~/Scripts/dataTables.bootstrap4.min.js",
                        "~/Scripts/bootstrap-datepicker.js",

                        "~/Scripts/select2.js",
                        "~/Scripts/select2_locale_pt_BR.js",
                        "~/Scripts/sSelect.js",
                        "~/Scripts/Functions.js",
                        "~/Scripts/functionsb3.js",
                        "~/Scripts/jquery.mask.js",
                        "~/Scripts/bootstrap-notify.js",
                        "~/Scripts/Sistema.js"
                      ));

        }
    }
}
