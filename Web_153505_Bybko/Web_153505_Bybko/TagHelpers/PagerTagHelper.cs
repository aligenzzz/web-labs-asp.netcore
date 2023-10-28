using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.IO;
using System.Xml.Serialization;

namespace Web_153505_Bybko.TagHelpers
{
    public class PagerTagHelper : TagHelper
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _contextAccessor;

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string? Genre { get; set; }
        public bool Admin { get; set; } = false;


        public PagerTagHelper(LinkGenerator linkGenerator, IHttpContextAccessor contextAccessor)
        {
            _linkGenerator = linkGenerator;
            _contextAccessor = contextAccessor;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            TagBuilder nav = new TagBuilder("nav");
            nav.Attributes.Add("arial-label", "Page navigation");
           
            TagBuilder ul = new("ul");
            ul.AddCssClass("pagination");

            ul.InnerHtml.AppendHtml(CreateListItem(1, "First"));
            ul.InnerHtml.AppendHtml(CreateListItem(CurrentPage - 1, "Previous"));
            for (int i = 0; i < TotalPages; i++)
                ul.InnerHtml.AppendHtml(CreateListItem(i + 1));
            ul.InnerHtml.AppendHtml(CreateListItem(CurrentPage + 1, "Next"));
            ul.InnerHtml.AppendHtml(CreateListItem(TotalPages, "Last"));

            nav.InnerHtml.AppendHtml(ul);

			output.Content.SetHtmlContent(nav);

            var scriptElement = new TagBuilder("script");
            scriptElement.Attributes.Add("src", $"/js/site.js");
            output.Content.AppendHtml(scriptElement);
        }

        private TagBuilder CreateListItem(int pageno, string innerString = "")
        {
            TagBuilder li = new("li");
            li.AddCssClass("page-item");

            if (CurrentPage == pageno && innerString == "")
                li.AddCssClass("active");

            if (CurrentPage == 1 && (innerString == "First" || innerString == "Previous"))
                li.AddCssClass("disabled");
            if (CurrentPage == TotalPages && (innerString == "Next" || innerString == "Last"))
                li.AddCssClass("disabled");

            li.InnerHtml.AppendHtml(CreateLinkItem(pageno, innerString)); 

            return li;
        }

        private TagBuilder CreateLinkItem(int pageno, string innerStr)
        {
            string? path = string.Empty;
            if (Admin)
            {
                path = _linkGenerator.GetPathByPage(_contextAccessor.HttpContext!,
                                                    values: new { pageno = pageno, genre = Genre });
            }  
            else
            {
                path = _linkGenerator.GetPathByAction(_contextAccessor.HttpContext!,
                                                      values: new { pageno = pageno, genre = Genre });
            }
                
            if (path is null)
            {
                throw new ArgumentNullException("path");
            }
                
            TagBuilder link = new TagBuilder("a");
            link.Attributes.Add("href", path);
            link.AddCssClass("page-link");

            if (innerStr == "")
            {
                link.InnerHtml.Append(pageno.ToString());
            }
            else
            {
                link.InnerHtml.Append(innerStr);
            }


            return link;
        }
    }
}
