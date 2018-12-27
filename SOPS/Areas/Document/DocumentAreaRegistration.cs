using System.Web.Mvc;

namespace SOPS.Areas.Document
{
    public class DocumentAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Document";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Document_default",
                "Document/{action}/{id}",
                new { controller="Default", action = "Details", id = UrlParameter.Optional }
            );
        }
    }
}