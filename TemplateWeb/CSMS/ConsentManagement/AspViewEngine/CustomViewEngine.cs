using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ConsentManagement
{
    public class CustomViewEngine:RazorViewEngine
    {
       public CustomViewEngine()
        {
            var viewLocations = new[] {
           "~/Views/{0}.cshtml",
           //"~/Views/Login/index.cshtml",
           //"~/Views/Shared/{0}.cshtml",
           // "~/Views/QueryBondInvestorType/{0}.cshtml",
           // "~/Views/ViewCusDetail/{0}.cshtml",
            "~/Views/{1}/{0}.cshtml"
            //"~/Views/Shared/{0}.aspx",
            //"~/Views/Shared/{0}.ascx",
            //"~/AnotherPath/Views/{0}.ascx"
            // etc
        };
      
            this.PartialViewLocationFormats = viewLocations;
            this.ViewLocationFormats = viewLocations;
        }
    }
}