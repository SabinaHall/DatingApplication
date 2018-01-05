using DatingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.SessionState;

namespace DatingApp.Controllers.Api
{
    public class MyApiController : ApiController
    {
        private MyDataContext db = new MyDataContext();

        private HttpSessionStateBase GetSessionForService()
        {
            var request = HttpContext.Current.Items["MS_HttpRequestMessage"] as HttpRequestMessage;

            if (request == null)
                return null;

            var httpContext = (HttpContextWrapper)request.Properties["MS_HttpContext"];
            return httpContext.Session;
        }

        [ValidateAntiForgeryToken]
        [System.Web.Http.HttpPost]
        public void Create([FromBody] Post post, int? id)
        {
            //var session = GetSessionForService();
            //var sessionString = session.ToString();
            //var sessionInt = int.Parse(sessionString);

            //var session = HttpContext.Current.Session;
            //var sessionString = session.ToString();
            //var sessionInt = int.Parse(sessionString);

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