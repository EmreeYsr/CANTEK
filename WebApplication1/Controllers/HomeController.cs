using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

public class HomeController : Controller
{
    [HttpPost]
    public IActionResult ChangeLanguage(string lang)
    {
        if (lang == "tr" || lang == "en")
        {
            var cultureInfo = new CultureInfo(lang);
            var requestCulture = new RequestCulture(cultureInfo);
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(requestCulture), new CookieOptions() { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            if (lang == "tr")
            {
                return RedirectToAction("Index", "PLC");  // T�rk�e sayfaya y�nlendir
            }
            else
            {
                return RedirectToAction("IndexEn", "PLC");  // �ngilizce sayfaya y�nlendir
            }
        }

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Index()
    {
        return View();
    }
}
