//-----------------------------------------------------------------------
// <copyright file="InitializeControllerFactory.cs" company="Sitecore Corporation">
//     Copyright (c) Sitecore Corporation 1999-2016
// </copyright>
// <summary>Defines the InitializeControllerFactory class.</summary>
//-----------------------------------------------------------------------
// Copyright 2016 Sitecore Corporation A/S
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file 
// except in compliance with the License. You may obtain a copy of the License at
//       http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the 
// License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
// either express or implied. See the License for the specific language governing permissions 
// and limitations under the License.
// -------------------------------------------------------------------------------------------
namespace Sitecore.Common.Website.Pipelines
{
    using System.Web.Mvc;
    using Sitecore.Common.Website.Infrastructure;
    using Sitecore.Foundation.CommerceServer.Factories;
    using Sitecore.Pipelines;

    /// <summary>
    /// Used to initialize the HttpFactoryController
    /// </summary>
    public class InitializeControllerFactory
    {
        /// <summary>
        /// The entry point for the pipeline processor
        /// </summary>
        /// <param name="args">The arguments for the pipeline</param>
        public virtual void Process(PipelineArgs args)
        {
            this.SetControllerFactory(args);
        }

        /// <summary>
        /// Sets up the controller factory for Sitecore
        /// </summary>
        /// <param name="args">The args for the pipeline request</param>
        protected virtual void SetControllerFactory(PipelineArgs args)
        {
            WindsorCommonConfig.Configurate(WindsorConfig.Container);
            IControllerFactory controllerFactory = new WindsorControllerFactory(WindsorConfig.Container.Kernel);
            var sitecoreControllerFactory = new SitecoreWindsorControllerFactory(controllerFactory);

            ControllerBuilder.Current.SetControllerFactory(sitecoreControllerFactory);
        }
    }
}