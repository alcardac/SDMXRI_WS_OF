namespace NSIWebServices
{
    using System.Reflection;
    using System.ServiceModel.Activation;
    using System.Web;
    using System.Web.Routing;

    using log4net;

    using Org.Sdmxsource.Util.Url;

    public class Global : HttpApplication
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Global));
        protected void Application_Start()
        {
            UriUtils.FixSystemUriDotBug();
            //RouteTable.Routes.Add(new ServiceRoute("data", new WebServiceHostFactory(), typeof(DataResource)));

            RouteTable.Routes.Add(new ServiceRoute("data", new SdmxRestServiceHostFactory(typeof(IDataResource)), typeof(DataResource)));
            RouteTable.Routes.Add(new ServiceRoute("", new SdmxRestServiceHostFactory(typeof(IStructureResource)), typeof(StructureResource)));
        }

        protected void Application_BeginRequest()
        {
            string path = Request.PhysicalPath;
            var rawUrl = Request.RawUrl;
            var url = Request.Url;
            _log.DebugFormat("path {0}, raw URL : {1}, url: {2}", path, rawUrl, url);
        }
    }
}