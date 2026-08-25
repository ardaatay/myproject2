using Microsoft.AspNetCore.Mvc;
using Web.ViewModels;

namespace Web.ViewComponents;

public class AktifBirimViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var user = HttpContext.User;
        var birimId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == "BirimId")?.Value);
        var birimAdi = user.Claims.FirstOrDefault(c => c.Type == "BirimAdi")?.Value!;

        return View(new AktifBirimViewModel
        {
            AktifBirimAdi = birimAdi,
            AktifBirimId = birimId
        });
    }
}