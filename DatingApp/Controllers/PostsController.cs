using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DatingApp.Models;

namespace DatingApp.Controllers
{
    public class PostsController : Controller
    {
        private MyDataContext db = new MyDataContext();

        // GET: Posts
        //Hämtar de inlägg som är skrivna till den personens sida du är inne på 
        public ActionResult Index(int? id)
        {
            var post = db.Posts.Include(x => x.Receiver.Id);
            return View(db.Posts.ToList());
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            return View();
        }

       // POST: Posts/Create
       //Hämtar in ett id och sätter vem som har skickat inlägget(userSender) och vem som kommer att få inlägget(UserReciver)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostId,Message")] Post post, int? id)
        {
            if (ModelState.IsValid)
            {
                using (MyDataContext db = new MyDataContext())
                {
                    int sessionId = int.Parse(Session["id"].ToString());
                    var userSender = db.User.First(x => x.Id == sessionId);

                    User userReciver = db.User.Find(id);

                    post.Sender = userSender;
                    post.Receiver = userReciver;

                    userReciver.Posts.Add(post);
                    db.SaveChanges();
                }
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
            return View();
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostId,Message")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
