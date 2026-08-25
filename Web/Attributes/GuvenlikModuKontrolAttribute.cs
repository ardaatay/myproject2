using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Web.Extensions;

namespace Web.Attributes;

public class GuvenlikModuKontrolAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var httpContext = context.HttpContext;
        
        // Service'i HttpContext üzerinden al
        var guvenlikModuService = httpContext.RequestServices.GetRequiredService<IGuvenlikModuService>();
        var guvenlikModu = guvenlikModuService.GetGuvenlikModuDurumu().Result;
        
        if (guvenlikModu)
        {
            // Kullanıcının rollerini kontrol et
            var user = httpContext.User;
            if (!user.IsInRole("ADMIN"))
            {
                // AJAX request mi kontrol et
                if (httpContext.Request.IsAjaxRequest())
                {
                    context.Result = new JsonResult(new 
                    { 
                        success = false, 
                        message = "Güvenlik modu aktif. Bu işlem sadece Admin kullanıcıları tarafından yapılabilir." 
                    });
                }
                else
                {
                    // Normal sayfa request'i için TempData ile mesaj gönder
                    var tempData = context.Controller as Controller;
                    if (tempData != null)
                    {
                        tempData.TempData["ErrorMessage"] = "Güvenlik modu aktif. Bu işlem sadece Admin kullanıcıları tarafından yapılabilir.";
                        var controllerName = context.RouteData.Values["controller"]?.ToString();
                        context.Result = new RedirectToActionResult("Index", controllerName, null);
                    }
                }
                return;
            }
        }
        
        base.OnActionExecuting(context);
    }
}