using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TagHelperDemo.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("tab-item",ParentTag = "tab")]
    public class TabItemTagHelper : TagHelper
    {

        [HtmlAttributeName("title")]

        public string Title { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var activePage = context.Items["ActivePage"] as string;
            if(activePage==Title)
                output.Attributes.Add("class","active");
            output.TagName = "li";
            output.PostContent.SetHtmlContent($"<a href='#'>{Title}</a?");

        }

       
    }
}
