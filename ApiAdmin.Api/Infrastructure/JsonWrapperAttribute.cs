using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Web;
using log4net;
using System.Web.Http.Filters;

using ApiAdmin.Api.Controllers;
using ApiAdmin.Api.Models;


namespace ApiAdmin.Api.Infrastructure
{
    public class JsonWrapperAttribute : ActionFilterAttribute
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(JsonWrapperAttribute));

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var response = new ApiResponse();

            if (actionExecutedContext.Exception != null)
            {
                var state = HttpStatusCode.InternalServerError;
                response.Message = actionExecutedContext.Exception.Message;
                response.State = (int)state;
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(state, response);

                Exception exc = actionExecutedContext.Exception;

                var httpCode = exc.GetType() == typeof(HttpException) ? ((HttpException)exc).GetHttpCode() : 500;

                if (httpCode.ToString(CultureInfo.InvariantCulture).StartsWith("4"))
                {
                    _log.Error(string.Format("Message: {0}. StackTrace: {1}. HttpCode: {2}.", exc.Message, exc.StackTrace, httpCode), exc);
                }
                else
                {
                    _log.Error(exc.Message, exc);
                }
            }
            else
            {
                var content = (actionExecutedContext.ActionContext.Response.Content as ObjectContent);
                response.Data = content != null ? content.Value : null;
                response.State = HttpContext.Current.Items[BaseController.HttpStateKey] != null ? (int)HttpContext.Current.Items[BaseController.HttpStateKey] : (int)actionExecutedContext.ActionContext.Response.StatusCode;
                response.Message = HttpContext.Current.Items[BaseController.HttpMessageKey] != null ? (string)HttpContext.Current.Items[BaseController.HttpMessageKey] : "";
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.OK, response);
            }
        }
    }
}