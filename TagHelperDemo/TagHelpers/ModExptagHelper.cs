using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TagHelperDemo.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("mod-exp")]
    public class ModExptagHelper : TagHelper
    {

        public ModelExpression HelperFor { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            var str = HelperFor == null ? "" : "Name:" + HelperFor.Name + " Model " + HelperFor.Model;
           
            output.Content.SetHtmlContent($"<div>{str}</div>");
            output.Attributes.Add("class", "highlight");

        }

       
    }
}
