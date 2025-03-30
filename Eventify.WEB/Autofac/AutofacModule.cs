using Autofac;
using Eventify.UoW;
using Eventify.UoW.Base;
using Eventify.WEB.ApplicationServices;
using Eventify.WEB.ApplicationServices.Base;

namespace Eventify.WEB.Autofac
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserApplicationService>().As<IUserApplicationService>();
            builder.RegisterType<ManageUsersUoW>().As<IManageUsersUoW>();
            builder.RegisterType<ManageRoleUoW>().As<IManageRoleUoW>();
            builder.RegisterType<LoginApplicationService>().As<ILoginApplicationService>();
        }
    }
}
