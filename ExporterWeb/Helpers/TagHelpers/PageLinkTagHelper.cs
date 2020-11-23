using System.Collections.Generic;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ExporterWeb.Helpers.TagHelpers
{
    [HtmlTargetElement("div", Attributes = "page-model")]
    public class PageLinkTagHelper : TagHelper
    {
        private IUrlHelperFactory _urlHelperFactory;

        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            _urlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public PagingInfo PageModel { get; set; }
        public string PageAction { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            TagBuilder result = new TagBuilder("div");
            List<int> pages = new List<int>();

            if (PageModel.HasPreviousPage)
            {
                CreatePageTag(PageModel.PageNumber - 1, "Назад", result);
            }

            const int delta = 2;
            var left = PageModel.PageNumber - delta;
            var right = PageModel.PageNumber + delta;

            for (var i = 1; i <= PageModel.TotalPages; i++)
            {
                if (i == 1 || (i >= left && i <= right) || i == PageModel.TotalPages)
                {
                    pages.Add(i);
                }
            }

            int? previousPage = null;
            foreach (var i in pages)
            {
                if (previousPage is {})
                {
                    if (i - previousPage == 2)
                    {
                        CreatePageTag(previousPage.Value + 1, result);
                    }
                    else if (i - previousPage != 1)
                    {
                        CreatePoint(result);
                    }
                }

                previousPage = i;
                CreatePageTag(i, result);
            }

            if (PageModel.HasNextPage)
            {
                CreatePageTag(PageModel.PageNumber + 1, "Вперед", result);
            }

            output.Content.AppendHtml(result.InnerHtml);
        }

        private void CreatePoint(TagBuilder result)
        {
            var tag = new TagBuilder("a");
            tag.InnerHtml.Append("...");
            result.InnerHtml.AppendHtml(tag);
        }

        private TagBuilder CreateBasePageTag(object page, TagBuilder result)
        {
            var tag = new TagBuilder("a");
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            tag.Attributes["href"] = urlHelper.Action(PageAction, new { pageNumber = page });
            return tag;
        }

        private void CreatePageTag(object page, TagBuilder result)
        {
            var tag = CreateBasePageTag(page, result);
            tag.InnerHtml.Append(page.ToString());
            result.InnerHtml.AppendHtml(tag);
        }

        private void CreatePageTag(object page, string value, TagBuilder result)
        {
            var tag = CreateBasePageTag(page, result);
            tag.InnerHtml.Append(value);
            result.InnerHtml.AppendHtml(tag);
        }
    }
}