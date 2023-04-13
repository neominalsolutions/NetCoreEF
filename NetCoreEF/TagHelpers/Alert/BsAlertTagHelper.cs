using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NetCoreEF.TagHelpers.Alert
{
  // <alert text="mesaj1"></alert>
  // <alert />

  [HtmlTargetElement("alert")]
  public class BsAlertTagHelper:TagHelper
  {
    private readonly IHtmlHelper htmlHelper = null;

    public BsAlertTagHelper(IHtmlHelper htmlHelper)
    {
      this.htmlHelper = htmlHelper;
    }

    [HtmlAttributeName("text")]
    public string Text { get; set; }

    [HtmlAttributeName("type")]
    public AlertTypes Type { get; set; }

    [HtmlAttributeName("onClick")]
    public string OnClick { get; set; }

    public string Color = string.Empty;

 



    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; }
    public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
      //output.TagMode = TagMode.StartTagAndEndTag;



      switch (Type)
      {
        case AlertTypes.Warning:
          Color = "warning";
          break;
        case AlertTypes.Info:
          Color = "info";
          break;
        case AlertTypes.Error:
          Color = "danger";
          break;
        default:
          break;
      }


      //output.Content.SetHtmlContent($"<div class='alert alert-{Color}'>{Text}</div>");


      ViewContext.ViewBag.ElementId = Guid.NewGuid().ToString();
      //Contextualize the html helper
      (htmlHelper as IViewContextAware).Contextualize(ViewContext);

      var content = await htmlHelper?.PartialAsync("~/TagHelpers/Alert/_BsAlertTagHelperPartialView.cshtml", this);

      output.Content.SetHtmlContent(content);
    }


  }


  public enum AlertTypes
  {
    Warning,
    Info,
    Error
  }
}
