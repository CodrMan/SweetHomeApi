using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Script.Serialization;
using log4net;

using ApiAdmin.Api.Controllers;
using ApiAdmin.Core.Entities.log4net;
using ApiAdmin.IoC;
using ApiAdmin.Services.Abstract;


namespace ApiAdmin.Api.Infrastructure
{
    public class RequestLoggingAttribute : ActionFilterAttribute
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(RequestLoggingAttribute));
        private readonly IMessageService _messageService = DependencyResolver.Resolve<IMessageService>();
        private readonly bool _isEnable = Convert.ToBoolean(ConfigurationManager.AppSettings["EnabledLogginReguest"]);
        private readonly object _locker = new object();
        private DateTime _startExecute;
        private DateTime _endExecute;

        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            if(!_isEnable)
                return;

            _startExecute = DateTime.UtcNow;
            lock (_locker)
            {
                try
                {
                    Log onExecLog = new Log
                    {
                        RequestType = filterContext.Request.Method.ToString(),
                        Response = "OnActionExecuting Event",
                        ExecutionTime = 0
                    };

                    var currentUser = ((BaseController)filterContext.ControllerContext.Controller).CurrentUser;
                    onExecLog.UserId = currentUser != null ? currentUser.Id : 0;

                    #region Getting query params

                    var queryStringParams = filterContext.Request.GetQueryNameValuePairs();
                    var formParams = filterContext.ActionArguments;

                    var totalParams = new Dictionary<string, string>();

                    if (queryStringParams != null && queryStringParams.Count() > 0)
                    {
                        foreach (var param in queryStringParams)
                            try
                            {
                                totalParams.Add(param.Key, param.Value);
                            }
                            catch { }
                    }
                    if (formParams != null && formParams.Count > 0)
                    {
                        foreach (var param in formParams)
                        {
                            string value = new JavaScriptSerializer().Serialize(param.Value);
                            try
                            {
                                totalParams.Add(param.Key, value);
                            }
                            catch { }
                        }
                    }

                    var actionParameters = string.Join(",", totalParams
                        .Where(p => p.Key != "__RequestVerificationToken" && !p.Value.ToLower().Contains("password"))
                        .Select(p => string.Concat(p.Key, "=", p.Value)));

                    onExecLog.InputParameters = actionParameters;

                    #endregion

                    AddLog(onExecLog);
                }
                catch (Exception log)
                {
                    _log.Error(log);
                    _messageService.AddErrorMessage(log.Message);
                }
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext filterContext)
        {
            if (!_isEnable)
                return;

            try
            {
                _endExecute = DateTime.UtcNow;
                if (filterContext.ActionContext.Response.Content is ObjectContent)
                {
                    string content = filterContext.Response.Content.ReadAsStringAsync().Result;
                    LogRequest(content, filterContext);
                }
                else if (filterContext.Exception != null)
                {

                    string content = filterContext.Exception.Message;

                    LogRequest(content, filterContext);
                }
                else
                {
                    LogRequest("", filterContext);
                }
            }
            catch (Exception log)
            {
                _log.Error(log);
                _messageService.AddErrorMessage(log.Message);
            }
        }

        private void LogRequest(string content, HttpActionExecutedContext filterContext)
        {
            lock (_locker)
            {
                try
                {
                    var logMessage = new Log
                    {
                        RequestType = filterContext.ActionContext.Request.Method.ToString(),
                        Response =  content,
                        ExecutionTime = (_endExecute - _startExecute).TotalSeconds
                    };

                    var currentUser = ((BaseController)filterContext.ActionContext.ControllerContext.Controller).CurrentUser;
                    logMessage.UserId = currentUser != null ? currentUser.Id : 0;

                    #region Getting query params

                    var queryStringParams = filterContext.ActionContext.Request.GetQueryNameValuePairs();
                    var formParams = filterContext.ActionContext.ActionArguments;

                    var totalParams = new Dictionary<string, string>();

                    if (queryStringParams != null && queryStringParams.Count() > 0)
                    {
                        foreach (var param in queryStringParams)
                            try
                            {
                                totalParams.Add(param.Key, param.Value);
                            }
                            catch { }
                    }
                    if (formParams != null && formParams.Count > 0)
                    {
                        foreach (var param in formParams)
                        {
                            string value = new JavaScriptSerializer().Serialize(param.Value);
                            try
                            {
                                totalParams.Add(param.Key, value);
                            }
                            catch { }
                        }
                    }

                    var actionParameters = string.Join(",", totalParams
                        .Where(p => p.Key != "__RequestVerificationToken" && !p.Value.ToLower().Contains("password"))
                        .Select(p => string.Concat(p.Key, "=", p.Value)));

                    logMessage.InputParameters = actionParameters;

                    #endregion

                    if (filterContext.Exception != null)
                    {
                        logMessage.ErrorMessage = filterContext.Exception.Message;
                        logMessage.InnerErrorMessage = filterContext.Exception.InnerException != null ? filterContext.Exception.InnerException.Message : "";
                        logMessage.ErrorSource = filterContext.Exception.Source;
                        logMessage.StackTrace = filterContext.Exception.StackTrace;
                    }

                    AddLog(logMessage);
                }
                catch (Exception log)
                {
                    _log.Error(log.Message, log);
                    _messageService.AddErrorMessage(log.Message);
                }
            }
        }

        private void AddLog(Log log)
        {
            _log.DebugFormat("UserId: {0} \r\n" +
                             " RequestType: {1}\r\n" +
                             " InputParameters: {2}\r\n" +
                             " Response: {3}\r\n" +
                             " ErrorSource: {4}\r\n" +
                             " ErrorMessage: {5}\r\n" +
                             " InnerErrorMessage: {6}\r\n" +
                             " StackTrace: {7}\r\n" +
                             " ExecutionTime: {8}\r\n",
                             log.UserId,
                             log.RequestType,
                             log.InputParameters,
                             log.Response,
                             log.ErrorSource,
                             log.ErrorMessage,
                             log.InnerErrorMessage,
                             log.StackTrace,
                             log.ExecutionTime);
        }
    }
}