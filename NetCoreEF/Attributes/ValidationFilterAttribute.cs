using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace NetCoreEF.Attributes
{
 
  public class ValidationFilterAttribute : ActionFilterAttribute
  {

    public override void OnActionExecuting(ActionExecutingContext context)
    {

      if (!context.ModelState.IsValid)
      {
        // 422  status uygulamada validasyon hataları olduğu durumda döndüğümüz bir status code.
        context.Result = new ViewResult();
      }

      base.OnActionExecuting(context);
    }
  }
}
