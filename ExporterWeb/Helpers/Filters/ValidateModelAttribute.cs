using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExporterWeb.Helpers
{
    public class ValidateModelAttribute : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (!context.ModelState.IsValid)
                context.Result = ((PageModel)context.Controller).Page();
        }
    }
}