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
            TagBuilder result = new TagBuilder("ul");
            TagBuilder list = new TagBuilder("ul");
            TagBuilder page;
            List<int> pageNumbers = new List<int>();

            if (PageModel.HasPreviousPage)
            {
                page = CreatePageItem(PageModel.PageNumber - 1, "Previous", "chevron_left");
                list.InnerHtml.AppendHtml(page);
            }

            const int delta = 2;
            var left = PageModel.PageNumber - delta;
            var right = PageModel.PageNumber + delta;

            for (var i = 1; i <= PageModel.TotalPages; i++)
            {
                if (i == 1 || (i >= left && i <= right) || i == PageModel.TotalPages)
                {
                    pageNumbers.Add(i);
                }
            }

            int? previousPage = null;
            foreach (var pageNumber in pageNumbers)
            {
                if (previousPage is { })
                {
                    if (pageNumber - previousPage == 2)
                    {
                        page = CreatePageItem(previousPage.Value + 1);
                        list.InnerHtml.AppendHtml(page);
                    }
                    else if (pageNumber - previousPage != 1)
                    {
                        var point = CreateDots();
                        list.InnerHtml.AppendHtml(point);
                    }
                }
                previousPage = pageNumber;
                page = CreatePageItem(pageNumber);

                if (pageNumber == PageModel.PageNumber)
                {
                    page.AddCssClass("active");                  
                }
                list.InnerHtml.AppendHtml(page);
            }

            if (PageModel.HasNextPage)
            {
                page = CreatePageItem(PageModel.PageNumber + 1, "Next", "chevron_right");
                list.InnerHtml.AppendHtml(page);
            }

            list.AddCssClass("pagination");
            result.InnerHtml.AppendHtml(list);
            output.Content.AppendHtml(result.InnerHtml);
        }

        private TagBuilder CreateDots()
        {
            var link = CreateLinkTag();
            link.InnerHtml.Append("...");

            var li = CreateListItemTag(link);

            return li;
        }

        private TagBuilder CreatePageItem(object page)
        {
            var link = CreateLinkTag(page, page.ToString()!);
            link.InnerHtml.Append(page.ToString());

            var li = CreateListItemTag(link);

            return li;
        }

        private TagBuilder CreatePageItem(object page, string ariaLabel, string value)
        {
            var span = new TagBuilder("span");
            span.Attributes.Add("aria-hidden", "true");
            span.AddCssClass("material-icons");
            span.InnerHtml.Append(value);

            var link = CreateLinkTag(page, ariaLabel);
            link.InnerHtml.AppendHtml(span);

            var li = CreateListItemTag(link);

            return li;
        }

        private TagBuilder CreateLinkTag(object page, string ariaLabel)
        {
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            var link = CreateLinkTag();
            link.Attributes.Add("aria-label", ariaLabel);
            link.Attributes["href"] = urlHelper.Action(PageAction, new { p = page });

            return link;
        }

        private TagBuilder CreateLinkTag()
        {
            var link = new TagBuilder("a");
            link.AddCssClass("page-link");
            return link;
        }

        private TagBuilder CreateListItemTag(TagBuilder link)
        {
            var li = new TagBuilder("li");
            li.AddCssClass("page-item");
            li.InnerHtml.AppendHtml(link);
            return li;
        }
    }
}