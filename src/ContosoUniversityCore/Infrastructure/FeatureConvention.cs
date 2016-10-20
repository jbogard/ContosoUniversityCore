namespace ContosoUniversityCore.Infrastructure
{
    using System;
    using System.Reflection;
    using Microsoft.AspNetCore.Mvc.ApplicationModels;
    using System.Linq;

    public class FeatureConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            controller.Properties.Add("feature",
              GetFeatureName(controller.ControllerType));
        }
        private string GetFeatureName(TypeInfo controllerType)
        {
            string[] tokens = controllerType.FullName.Split('.');
            if (tokens.All(t => t != "Features")) return "";
            string featureName = tokens
              .SkipWhile(t => !t.Equals("features",
                StringComparison.CurrentCultureIgnoreCase))
              .Skip(1)
              .Take(1)
              .FirstOrDefault();
            return featureName;
        }
    }
}