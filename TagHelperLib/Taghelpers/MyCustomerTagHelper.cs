using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TagHelperLib.Taghelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("my-customer1",Attributes = "info")]
    public class MyCustomerTagHelper : TagHelper
    {
        public string Info { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
           // output.Content.SetHtmlContent("CurrentTime:"+DateTime.Now.ToShortDateString());
            output.TagName = "strong";
            var ata = _nameList.FirstOrDefault(c => c.Name.ToLower() == Info.ToLower());
            var result = $"Name :{ata.Name} Address:{ata.Address}";
            output.Content.SetContent(result);
        }

        private class NameRecord
        {
            public string Name { get; set; }
            public string Address { get; set; }
        }

        private readonly  List<NameRecord> _nameList=new List<NameRecord>()
        {
            new NameRecord{Name = "Subhashri",Address = "abc"},
            new NameRecord{Name = "Igor",Address = "test"}
        };
    }
}
