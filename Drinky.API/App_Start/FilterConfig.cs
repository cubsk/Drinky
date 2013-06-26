using System.Web;
using System.Web.Mvc;
using Drinky.API.Security;

namespace Drinky.API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());


        }
    }
}