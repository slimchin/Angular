using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
namespace ConsentManagement
{
    public class AspAuthen: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            RouteValueDictionary Rd = new RouteValueDictionary { { "controller", "Login" }, { "action", "Index" } };
            if (filterContext.HttpContext.Session["usr"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(Rd);
            } else if (filterContext.HttpContext.Session["usr"].ToString() == "")
            {
                filterContext.Result = new RedirectToRouteResult(Rd);
            }
                base.OnActionExecuting(filterContext);
        }
    }
}