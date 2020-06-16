using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
namespace ConsentManagement
{
    public class AspAuthorize: ActionFilterAttribute
    {
        public string functionName { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            RouteValueDictionary Rd = new RouteValueDictionary { { "controller", "Login" }, { "action", "Index" } };
            if (filterContext.HttpContext.Session["usr"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(Rd);
            }
            string tempUser = filterContext.HttpContext.Session["usr"].ToString();

            //RouteValueDictionary Rd2 = new RouteValueDictionary { { "controller", "Login" }, { "action", "GotoAuthorizeFailPage" }, {"ErrorMsg","คุณไม่มีสิทธิ์ใช้หน้าจอนี้ !!"} };
            //if (filterContext.HttpContext.Session["usr"] == null)
            //{
            //    filterContext.Result = new RedirectToRouteResult(Rd);
            //}
            //else if (filterContext.HttpContext.Session["usr"].ToString() == "")
            //{
            //    filterContext.Result = new RedirectToRouteResult(Rd);
            //}

            bool canAccessForm = UserManagement.ReadMenuPermission2(tempUser, functionName);
            if (!canAccessForm)
            {
                filterContext.Result = new RedirectToRouteResult(Rd);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}