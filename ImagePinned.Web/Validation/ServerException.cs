using ListShop.BLL.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ListShop.Web.Validation
{
    public class ServerException : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled && filterContext.Exception is BLLException)
            {
                filterContext.Result = new RedirectResult("/Error/BLLError");
                filterContext.ExceptionHandled = true;
            }

            if (!filterContext.ExceptionHandled && filterContext.Exception is Exception)
            {
                filterContext.Result = new RedirectResult("/Error/ServerError");
                filterContext.ExceptionHandled = true;
            }

        }
    }
}