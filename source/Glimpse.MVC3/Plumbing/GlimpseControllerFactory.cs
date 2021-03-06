﻿using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc3.Extensions;

namespace Glimpse.Mvc3.Plumbing
{
    internal class GlimpseControllerFactory : IControllerFactory
    {
        public IControllerFactory ControllerFactory { get; set; }
        public IGlimpseLogger Logger { get; set; }

        public GlimpseControllerFactory(IControllerFactory controllerFactory)
        {
            ControllerFactory = controllerFactory;
        }

        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            IController controller = ControllerFactory.CreateController(requestContext, controllerName);

            Trace.Write(string.Format("{0}.CreateController(requestContext, \"{1}\") = {2}", ControllerFactory.GetType().Name, controllerName, controller == null ? "null" : controller.GetType().ToString()));

            if (controller != null)
                return controller.TrySetActionInvoker(Logger);

            return null;
        }

        public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            var controllerSessionBehavior = ControllerFactory.GetControllerSessionBehavior(requestContext, controllerName);
            
            return controllerSessionBehavior;
        }

        public void ReleaseController(IController controller)
        {
            ControllerFactory.ReleaseController(controller);
        }
    }
}
