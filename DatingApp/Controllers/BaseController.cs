using DatingApp.Models;
using System.Web.Mvc;

namespace DatingApp.Controllers
{
    public class BaseController : Controller
    {
        protected MyDataContext db = new MyDataContext();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}