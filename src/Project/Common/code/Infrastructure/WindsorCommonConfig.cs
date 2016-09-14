namespace Sitecore.Common.Website.Infrastructure
{
    using System.Web.Mvc;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    public class WindsorCommonConfig
    {
        public static void Configurate(IWindsorContainer container)
        {
            container.Register(Classes.FromAssemblyInDirectory(new AssemblyFilter(".\\bin", "Sitecore.Feature.*.dll")).BasedOn(typeof(IController)).LifestyleScoped());
            container.Register(Classes.FromAssemblyInDirectory(new AssemblyFilter(".\\bin", "Sitecore.Feature.*.dll")).BasedOn(typeof(IWindsorInstaller)));
            var dependencyInstallers = container.Kernel.GetAssignableHandlers(typeof(IWindsorInstaller));
            foreach (var dependencyInstaller in dependencyInstallers)
            {
                var installer = container.Resolve(dependencyInstaller.ComponentModel.Implementation);
                ((IWindsorInstaller)installer).Install(container, null);
            }
        }
    }
}
