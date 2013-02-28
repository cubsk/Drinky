using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NHibernate;
using Drinky.DAC;

namespace Drinky.MVC
{
    public class MvcController : Controller
    {
        protected ISession Session { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Session = SessionManager.GetSession();
            base.OnActionExecuting(filterContext);
        }
    }
}
