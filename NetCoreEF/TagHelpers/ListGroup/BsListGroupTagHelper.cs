using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NetCoreEF.TagHelpers.Alert;
using static System.Net.Mime.MediaTypeNames;

namespace NetCoreEF.TagHelpers.ListGroup
{

  


  [HtmlTargetElement("bs-list-group")]
  public class BsListGroupTagHelper:TagHelper
  {
    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
      output.TagName = "ul";
      output.TagMode = TagMode.StartTagAndEndTag;
      output.Attributes.SetAttribute("class","list-group");

      //output.($"<ul class='list-group'></ul>");


      return base.ProcessAsync(context, output);
    }

  }
}
