using System;
using System.IO;
using System.Web.Mvc;
using log4net;

namespace Contoso.Filters
{
    /* The HandleError filter handles the exceptions that are raised by the controller actions, filters and views,
        it returns a custom view named Error which is placed in the Shared folder. 
        The HandleError filter works only if the <customErrors> section is turned on in web.config

    
       The HandleError filter not only just returns the Error view but it also creates and passes the HandleErrorInfo model 
       to the view. The HandleErrorInfo model contains the details about the exception and the names of the controller and action
       that caused the exception
       
       The HandleError filter has some limitations by the way.
        
        1. Not support to log the exceptions 
        2. Doesn't catch HTTP exceptions other than 500 
        3. Doesn't catch exceptions that are raised outside controllers 
        4. Returns error view even for exceptions raised in AJAX calls
        
      */

    // Extending HandleError

    public class OneDiscoveryExceptionFilter : HandleErrorAttribute
    {

        ILog logger = log4net.LogManager.GetLogger(typeof(Controller));
        public override void OnException(ExceptionContext exceptionContext)
        {
            //To check wehther the exception has been handled 
            if (!exceptionContext.ExceptionHandled)
            {
                //To get the controller name
                string controllerName = (string)exceptionContext.RouteData.Values["controller"];
                //To get the action method's name 
                string actionName = (string)exceptionContext.RouteData.Values["action"];
                // To get the Error Information 
                var model = new HandleErrorInfo(exceptionContext.Exception, controllerName, actionName);
                logger.Debug(exceptionContext.Exception.Message);
                //This code shows the attribute where we redirect from Filter
                exceptionContext.Result = new ViewResult
                {

                    ViewName = View,// "ActionName of CustomError Page Name",
                    ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                    TempData = exceptionContext.Controller.TempData
                };

                exceptionContext.ExceptionHandled = true;
                exceptionContext.HttpContext.Response.Clear();
                exceptionContext.HttpContext.Response.StatusCode = 500;
                exceptionContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                base.OnException(exceptionContext);
            }
        }
    }
}
