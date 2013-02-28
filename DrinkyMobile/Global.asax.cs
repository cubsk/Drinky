using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Drinky.MVC.Binders;

namespace DrinkyMobile
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;

            ChainableModelBinder defaultBinder = new NullableGuidModelBinder();
            var next = defaultBinder.Chain(new EnumConverterModelBinder());
            next = next.Chain(new DictionaryModelBinder());
            ModelBinders.Binders.DefaultBinder = defaultBinder;
        }

        void Application_EndRequest(object sender, EventArgs e)
        {
            Drinky.DAC.SessionManager.CloseSession();
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
        }
    }
}