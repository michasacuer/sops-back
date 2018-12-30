using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SOPS
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
            GlobalConfiguration.Configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {            
            if (Request.UserLanguages != null)
            {
                foreach (var culture in Request.UserLanguages)
                {
                    try
                    {
                        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(culture);
                        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(culture);
                        return;
                    }
                    catch (CultureNotFoundException err)
                    {

                    }
                }
            }

            string defaultCulture = "en-US";
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(defaultCulture);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(defaultCulture);
        }

        protected void Application_EndRequest()
        {   //here breakpoint
            // under debug mode you can find the exceptions at code: this.Context.AllErrors
        }
    }
}
