using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using MvcEFTest.Utilities;

namespace MvcEFTest
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Mapper.Initialize(
                config =>
                Assembly.GetExecutingAssembly()
                        .GetTypes()
                        .Where(
                            type =>
                            typeof(Profile).IsAssignableFrom(type) && type.GetConstructor(Type.EmptyTypes) != null)
                        .Select(Activator.CreateInstance)
                        .Cast<Profile>()
                        .ForEach(config.AddProfile));
        }
    }
}
