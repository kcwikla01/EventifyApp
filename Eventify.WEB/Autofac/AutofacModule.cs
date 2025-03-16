using Autofac;
using Eventify.WEB.ApplicationServices;
using Eventify.WEB.ApplicationServices.Base;

namespace Eventify.WEB.Autofac
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserApplicationService>().As<IUserApplicationService>();
        }
    }
}
