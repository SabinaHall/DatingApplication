using DatingApp.Models;
using System.Linq;
using System.Web.Mvc;

namespace DatingApp.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View(db.User.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View(db.User.ToList());
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}