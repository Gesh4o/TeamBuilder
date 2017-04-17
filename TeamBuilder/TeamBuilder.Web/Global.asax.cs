namespace TeamBuilder.Web
{
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    using FluentValidation.Mvc;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            FluentValidationModelValidatorProvider.Configure();
            MapperWebConfig.RegisterAllMappings();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
