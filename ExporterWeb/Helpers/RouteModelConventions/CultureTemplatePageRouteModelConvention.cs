using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Linq;

namespace ExporterWeb.Helpers.RouteModelConventions
{
    public class CultureTemplatePageRouteModelConvention : IPageRouteModelConvention
    {
        public void Apply(PageRouteModel model)
        {
            foreach (var selector in model.Selectors.ToArray())
            {
                var template = selector.AttributeRouteModel.Template;
                if (template.EndsWith("/Index", System.StringComparison.InvariantCultureIgnoreCase))
                {
                    model.Selectors.Remove(selector);
                    continue;
                }

                model.Selectors.Add(new SelectorModel
                {
                    AttributeRouteModel = new AttributeRouteModel
                    {
                        Order = -1,
                        Template = AttributeRouteModel.CombineTemplates("{culture?}", selector.AttributeRouteModel.Template),
                    }
                });
            }
        }
    }
}
