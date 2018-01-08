using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace DatingApp.Controllers
{
    public class LanguageController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //Tar in cookien för att kolla vad det nuvarande språket är. 
        //Culture ändrar t.ex. hur man skriver ut tiden beroende på vilket land man är.
        public ActionResult Change(string LanguageAbbreviation)
        {
            if (LanguageAbbreviation != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(LanguageAbbreviation);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageAbbreviation);
            }
            HttpCookie cookie = new HttpCookie("Language"); 
            cookie.Value = LanguageAbbreviation;
            Response.Cookies.Add(cookie);

            return View("Index");
        }

    }
}