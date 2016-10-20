namespace ContosoUniversityCore.Infrastructure
{
    using Domain;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class EntityModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            return typeof(IEntity).IsAssignableFrom(context.Metadata.ModelType) ? new EntityModelBinder() : null;
        }
    }
}