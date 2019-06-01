using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TagHelperDemo.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("auto-price",Attributes = "model")]
    [HtmlTargetElement("auto-price", Attributes = "make")]
    public class AutoPriceTagHelper : TagHelper
    {
       // [HtmlAttributeNotBound]
        public string Make { get; set; }
       // [HtmlAttributeName("model-name")]
        public string Model { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.PreElement.SetHtmlContent("<div>PreElement</div>");
            output.PreContent.SetHtmlContent("<div>PreConent</div>");
            output.Content.SetHtmlContent($"<div>Model Name :{Model}</div>");
            output.Attributes.Add("class", "highlight");
            output.PostContent.SetHtmlContent("<div>PostContent</div>");
            output.PostElement.SetHtmlContent("<div>PostElement</div>");

        }

       
    }
}
