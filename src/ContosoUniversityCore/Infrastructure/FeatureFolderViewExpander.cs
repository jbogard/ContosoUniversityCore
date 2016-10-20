namespace ContosoUniversityCore.Infrastructure
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.Razor;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.Controllers;

    public class FeatureViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context) { }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context,
          IEnumerable<string> viewLocations)
        {
            // Error checking removed for brevity
            var controllerActionDescriptor =
                (ControllerActionDescriptor) context.ActionContext.ActionDescriptor;
            string featureName = controllerActionDescriptor.Properties["feature"] as string;
            foreach (var location in viewLocations)
            {
                yield return location.Replace("{3}", featureName);
            }
        }
    }
}