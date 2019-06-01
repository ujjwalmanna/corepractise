using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TagHelperDemo.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    public class ModTestagHelper : TagHelper
    {
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
           
            output.Content.SetHtmlContent($"<div>{FirstName} {LastName}</div>");
            output.Attributes.Add("class", "highlight");

        }

       
    }
}
