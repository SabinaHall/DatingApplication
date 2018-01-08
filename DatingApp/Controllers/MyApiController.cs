using DatingApp.Models;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace DatingApp.Controllers.Api
{
    public class MyApiController : ApiController
    {
        //Metod för att få åtkomst till Session i Api-Controller.
        private HttpSessionStateBase GetSessionForService()
        {
            var request = HttpContext.Current.Items["MS_HttpRequestMessage"] as HttpRequestMessage;

            if (request == null)
                return null;

            var httpContext = (HttpContextWrapper)request.Properties["MS_HttpContext"];
            return httpContext.Session;
        }

        //Skapar en post som skickas med jQuery och AJAX till Api-Controllern. 
        [ValidateAntiForgeryToken]
        [System.Web.Http.HttpPost]
        public void Create([FromBody] Post post, int? id)
        {
            if (ModelState.IsValid)
            {
                using (MyDataContext db = new MyDataContext())
                {
                    User userReciver = db.User.Find(id);
                    var sessionId = HttpContext.Current.Session.SessionID;
                    User userSender = db.User.First(x => x.SId == sessionId);

                    post.Receiver = userReciver;
                    post.Sender = userSender;

                    userReciver.Posts.Add(post);
                    db.SaveChanges();
                }
            }
        }
    }
}