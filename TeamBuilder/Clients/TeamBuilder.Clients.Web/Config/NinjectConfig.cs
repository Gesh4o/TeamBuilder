namespace TeamBuilder.Web
{
    using System.Data.Entity;
    using System.Reflection;
    using System.Web;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin.Security;

    using Ninject;
    using Ninject.Extensions.Conventions;
    using Ninject.Web.Common;
    using Ninject.Web.Common.OwinHost;

    using Owin;

    using TeamBuilder.Clients.Common;
    using TeamBuilder.Clients.Infrastructure.Identity;
    using TeamBuilder.Data;
    using TeamBuilder.Data.Models;
    using TeamBuilder.Services.Common.Contracts;
    using TeamBuilder.Services.Data.Contracts;
    using TeamBuilder.Services.Data.Implementations;

    public class NinjectConfig
    {
        public static void Configure(IAppBuilder app)
        {
            IKernel kernel = CreateKernel();
            app.UseNinjectMiddleware(() => kernel);
        }

        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<DbContext>().To<TeamBuilderContext>().InRequestScope();
            kernel.Bind<IUserStore<ApplicationUser>>().To<UserStore<ApplicationUser>>().InRequestScope();
            kernel.Bind<ApplicationUserManager>().ToSelf();
            kernel.Bind<ApplicationSignInManager>().ToSelf();
            kernel.Bind<IAuthenticationManager>()
                .ToMethod(
                    x => HttpContext.Current.GetOwinContext().Authentication);
            kernel.Bind<IRoleStore<IdentityRole>>().To<TeamBuilderRoleStore>().InRequestScope();
            kernel.Bind<IFileService>().To<DropboxService>();

            kernel.Bind(
                k =>
                    k.From(ServerConstants.DataServicesAssembly)
                        .SelectAllClasses()
                        .InheritedFrom<IService>()
                        .BindDefaultInterface());

            kernel.Bind(
                k =>
                    k.From(ServerConstants.DataRepositoryAssembly)
                        .SelectAllClasses()
                        .Where(type => type.Name.Contains("Repository"))
                        .BindDefaultInterface());
        }

        private static IKernel CreateKernel()
        {
            IKernel k = new StandardKernel();
            k.Load(Assembly.GetExecutingAssembly());

            try
            {
                RegisterServices(k);
            }
            catch
            {
                k.Dispose();
                throw;
            }

            return k;
        }
    }
}