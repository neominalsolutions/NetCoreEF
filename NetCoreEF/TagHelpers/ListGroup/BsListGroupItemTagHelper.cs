using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NetCoreEF.TagHelpers.Alert;
using System.Runtime;
using System.Text;

namespace NetCoreEF.TagHelpers.ListGroup
{
  [HtmlTargetElement("bs-list-group-item")]
  public class BsListGroupItemTagHelper:TagHelper
  {
    [HtmlAttributeName("text")]
    public string Text { get; set; }

    [HtmlAttributeName("badgeText")]
    public string BadgeText { get; set; }

    [HtmlAttributeName("url")]
    public string Url { get; set; } 

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {

      output.TagName = "li";
      output.TagMode = TagMode.StartTagAndEndTag;
      output.Attributes.SetAttribute("class", "list-group-item d-flex justify-content-between align-items-center");
      output.Content.SetHtmlContent($"<a href='{Url}'> {Text} </a> <span class='badge bg-primary rounded-pill'>{BadgeText}</span>");

      return base.ProcessAsync(context, output);
    }
  }
}
