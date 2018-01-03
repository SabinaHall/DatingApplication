using DatingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace DatingApp.Controllers.Api
{
    public class MyApiController : ApiController
    {
        private MyDataContext db = new MyDataContext();

        [ValidateAntiForgeryToken]
        [System.Web.Http.HttpPost]
        public void Create([Bind(Include = "PostId,Message")] Post post, int? id)
        {
            var session = HttpContext.Current.Session;
            if (ModelState.IsValid && session != null)
            {
                using (MyDataContext db = new MyDataContext())
                {
                    int sessionId = int.Parse(session.ToString());
                    var userSender = db.User.First(x => x.Id == sessionId);

                    User userReciver = db.User.Find(id);

                    post.Sender = userSender;
                    post.Receiver = userReciver;

                    userReciver.Posts.Add(post);
                    db.SaveChanges();
                }
            }
        }

        [System.Web.Http.HttpGet]
        public List<Post> List(int? id)
        {
            User user = db.User.Find(id);
            return user.Posts.ToList();
        }
    }
}