using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Hosting;

namespace SweetHome.Api.Handlers
{
    public class OptionsHttpMessageHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Method == HttpMethod.Options)
            {
                var apiExplorer = GlobalConfiguration.Configuration.Services.GetApiExplorer();

                var config = request.GetConfiguration();
                var routeData = config.Routes.GetRouteData(request);
                var controllerContext = new HttpControllerContext(config, routeData, request);

                request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;
                controllerContext.RouteData = routeData;

                // get controller type
                var controllerDescriptor = new DefaultHttpControllerSelector(config).SelectController(request);
                var controllerRequested = controllerDescriptor.ControllerName;

                var supportedMethods = apiExplorer.ApiDescriptions
                    .Where(d =>
                    {
                        var controller = d.ActionDescriptor.ControllerDescriptor.ControllerName;
                        return string.Equals(
                            controller, controllerRequested, StringComparison.OrdinalIgnoreCase);
                    })
                    .Select(d => d.HttpMethod.Method)
                    .Distinct();

                if (!supportedMethods.Any())
                    return Task.Factory.StartNew(
                        () => request.CreateResponse(HttpStatusCode.NotFound));

                return Task.Factory.StartNew(() =>
                {
                    var resp = new HttpResponseMessage(HttpStatusCode.OK);
                    return resp;
                });
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}