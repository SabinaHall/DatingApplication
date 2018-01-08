using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace DatingApp.Controllers
{
    public class PostsController : BaseController
    {
        //Hämtar de inlägg som är skrivna till den personens sida du är inne på.
        public ActionResult Index(int? id)
        {
                var post = db.Posts.Include(x => x.Receiver.Id);
                return View(db.Posts.ToList());
        }
    }
}
