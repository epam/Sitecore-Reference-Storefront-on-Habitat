namespace Sitecore.Common.Website.Infrastructure
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Castle.MicroKernel;
    using Castle.MicroKernel.Lifestyle;

    /// <summary>
    /// The windsor controller factory.
    /// </summary>
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        /// <summary>
        /// The kernel.
        /// </summary>
        private readonly IKernel kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindsorControllerFactory" /> class.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        public WindsorControllerFactory(IKernel kernel)
        {
            this.kernel = kernel;
        }

        /// <summary>
        /// Releases the specified controller.
        /// </summary>
        /// <param name="controller">The controller to release.</param>
        public override void ReleaseController(IController controller)
        {
            if (this.IsFromCurrentAssembly(controller.GetType()))
            {
                this.kernel.ReleaseComponent(controller);
            }
            else
            {
                base.ReleaseController(controller);
            }
        }

        /// <summary>
        /// Retrieves the controller instance for the specified request context and controller type.
        /// </summary>
        /// <param name="requestContext">The context of the HTTP request, which includes the HTTP context and route data.</param>
        /// <param name="controllerType">The type of the controller.</param>
        /// <returns>
        /// The controller instance.
        /// </returns>
        /// <exception cref="System.Web.HttpException">Error 40.4</exception>
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                throw new HttpException(404, string.Format(CultureInfo.InvariantCulture, "The controller for path '{0}' could not be found.", requestContext.HttpContext.Request.Path));
            }

            using (kernel.BeginScope())
            {
                return (IController)this.kernel.Resolve(controllerType);
            }
        }

        protected override Type GetControllerType(RequestContext requestContext, string controllerName)
        {
            var controllerType = base.GetControllerType(requestContext, controllerName);
            if (controllerType != null)
            {
                return controllerType;
            }

            controllerType = Type.GetType(controllerName);

            return controllerType;
        }

        /// <summary>
        /// Determines whether the specified object is from current assembly.
        /// </summary>
        /// <param name="type">The type to check for.</param>
        /// <returns>True if it is from the current assembly and false otherwise</returns>
        protected bool IsFromCurrentAssembly(Type type)
        {
            if (type != null)
            {
                var currentAssembly = Assembly.GetExecutingAssembly().FullName;
                var controllerAssembly = type.Assembly.FullName;

                if (currentAssembly.Equals(controllerAssembly, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}