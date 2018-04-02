using System.Web;
using System.Web.Optimization;

namespace SpfInventaire.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            ////////////////////////////////////////////////////////////////////
            /////////////////// Bundle Generaux /////////////////////////////////
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-1.12.4.min.js", //{version}
                "~/Scripts/jquery.unobtrusive-ajax.min.js",
                "~/Scripts/jquery.validate.min.js",
                "~/Scripts/jquery.validate.unobtrusive.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryUi").Include(
            "~/Scripts/jquery-ui-{version}.min.js",
            "~/Scripts/jquery-ui-init.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/respond.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatable").Include(
                        "~/Scripts/jquery.dataTables.min.js",
                        "~/Scripts/dataTables.bootstrap.min.js",
                        "~/Scripts/datatableInitializer.js"));

            bundles.Add(new ScriptBundle("~/bundles/AjaxService").Include(
                        "~/Scripts/AjaxMessages.js",
                        "~/Scripts/AjaxService.js"
                        ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/site.css"));


            ////////////////////////////////////////////////////////////////////
            /////////////////// Bundle Page Specifique /////////////////////////////////
            bundles.Add(new ScriptBundle("~/bundles/SaisieInventaire").Include(
                        "~/Scripts/ViewScripts/BoutonInventaire.js",
                        "~/Scripts/ViewScripts/SaisieInventaire.js",
                        "~/Scripts/ViewScripts/TicketForm.js",
                        "~/Scripts/ViewScripts/GestionListSelectionMateriel.js"                      
                        ));


            bundles.Add(new ScriptBundle("~/bundles/TicketForm").Include(
            "~/Scripts/ViewScripts/BoutonInventaire.js",
            "~/Scripts/ViewScripts/TicketForm.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/SaisieStock").Include(
                "~/Scripts/ViewScripts/BoutonInventaire.js",
                "~/Scripts/ViewScripts/SaisieStock.js"
                ));

            //Activation de l'optimisation (Override the <compilation debug="true" code in the web.conf)
            BundleTable.EnableOptimizations = true;
        }
    }
}
