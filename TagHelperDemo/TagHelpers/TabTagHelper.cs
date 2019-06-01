using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TagHelperDemo.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [RestrictChildren("tab-item")]
    public class TabTagHelper : TagHelper
    {
        [HtmlAttributeName("active-page")]
        public string ActivePage { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            context.Items["ActivePage"] = ActivePage;
            output.TagName = "div";
           
            output.PreContent.SetHtmlContent($"<h3>Code Camp</h3><ul class='nav nav-tabs'>");
            output.PostContent.SetHtmlContent("</ul>");
            output.Attributes.Add("class","container");

        }

       
    }
}
