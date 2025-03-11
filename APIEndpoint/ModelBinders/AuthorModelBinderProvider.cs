// ModelBinders/AuthorModelBinderProvider.cs
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using APIEndpoint.Models;

namespace APIEndpoint.ModelBinders
{
    public class AuthorModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(Author))
            {
                return new AuthorModelBinder();
            }

            return null;
        }
    }
}