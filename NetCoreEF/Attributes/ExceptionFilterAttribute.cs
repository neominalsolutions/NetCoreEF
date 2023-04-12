using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace NetCoreEF.Attributes
{
  public class ExceptionFilterAttribute : IExceptionFilter
  {
    private readonly IModelMetadataProvider modelMetadataProvider;

    public ExceptionFilterAttribute(IModelMetadataProvider modelMetadataProvider)
    {
      this.modelMetadataProvider = modelMetadataProvider;
    }

    public void OnException(ExceptionContext context)
    {

      string hata = context.Exception.Message;
      File.WriteAllText("Hatalar.txt", hata);

      var viewData = new ViewDataDictionary(this.modelMetadataProvider, context.ModelState);
      viewData.Add("Succeded", false);
      viewData.Add("Message", hata);

      context.ExceptionHandled = true;


      context.Result = new ViewResult()
      {
        ViewData = viewData
      };

      //context.HttpContext.Response.Redirect("Error");
    }
  }
}
