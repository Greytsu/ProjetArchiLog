using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetArchiLog.Library.Config
{
    public class SwaggerConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var controllerNamespace = controller.ControllerType.Namespace;
            var apiversion = controllerNamespace.Split(".").Last().ToLower();
            if (!apiversion.StartsWith("v")) { apiversion = "v1"; }
            controller.ApiExplorer.GroupName = apiversion;

        }
    }
}
