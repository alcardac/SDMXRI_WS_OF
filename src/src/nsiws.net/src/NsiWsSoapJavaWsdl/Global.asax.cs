namespace NsiWsSoapJavaWsdl
{
    using System.ServiceModel.Activation;
    using System.Web;
    using System.Web.Routing;

    using log4net;

    public class Global : HttpApplication
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Global));
        protected void Application_Start()
        {
            //RouteTable.Routes.Add(new ServiceRoute("data", new WebServiceHostFactory(), typeof(DataResource)));

            RouteTable.Routes.Add(new ServiceRoute("NSIStdV20Service", new SoapServiceHostFactory(typeof(INSIEstatV20Service)), typeof(NsiEstatV20Service)));
        }
    }

    //public class EstatBehaviour : Behaviourt

}