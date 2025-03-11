using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using APIEndpoint.Models;
using System.Text.Json;

namespace APIEndpoint.ModelBinders
{
    public class AuthorModelBinder : IModelBinder
    {

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            using (var reader = new StreamReader(bindingContext.HttpContext.Request.Body))
            {
                var body = await reader.ReadToEndAsync();
                Console.WriteLine($"Received JSON: {body}"); // برای Debugging

                if (string.IsNullOrWhiteSpace(body))
                {
                    bindingContext.ModelState.AddModelError("body", "درخواست JSON خالی است");
                    bindingContext.Result = ModelBindingResult.Failed();
                    return;
                }

                try
                {
                    var author = JsonSerializer.Deserialize<Author>(body, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true // برای پشتیبانی از حروف کوچک و بزرگ
                    });

                    if (author == null || string.IsNullOrWhiteSpace(author.Name))
                    {
                        bindingContext.ModelState.AddModelError("name", "نام نویسنده الزامی است");
                        bindingContext.Result = ModelBindingResult.Failed();
                        return;
                    }

                    // مقداردهی پیش‌فرض برای Cover
                    if (string.IsNullOrWhiteSpace(author.Cover))
                    {
                        author.Cover = "/images/authors/default.jpg";
                    }

                    bindingContext.Result = ModelBindingResult.Success(author);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing JSON: {ex.Message}");
                    bindingContext.ModelState.AddModelError("json", "فرمت JSON نامعتبر است");
                    bindingContext.Result = ModelBindingResult.Failed();
                }
            }
        }
    }
}