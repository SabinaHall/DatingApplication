using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DatingApp.Models;
using Microsoft.AspNet.Identity;

namespace DatingApp.Controllers
{
    public class UsersController : Controller
    {
        private MyDataContext db = new MyDataContext();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.User.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Firstname,Lastname,Age,Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Create", "Users");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id) 
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                db.SaveChanges();
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Firstname,Lastname,Age,Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Loggedin", "Users");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.User.Find(id);
            db.User.Remove(user);
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

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            using (MyDataContext db = new MyDataContext())
            {
                var usr = db.User.Where(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefault();
                if (usr != null)
                {
                    Session["Id"] = usr.Id.ToString();
                    return RedirectToAction("Loggedin");
                }
                else
                {
                    ModelState.AddModelError("", "Email or Password is invalid");
                }
            }
            return View();
        }


        public ActionResult LoggedIn(int? Id)
        {
            if (Session["Id"] != null || Id != null)
            {
                using (MyDataContext db = new MyDataContext())
                {

                    int id = Id ?? int.Parse(Session["id"].ToString());
                    var user = db.User.Include(i => i.Posts).First(x => x.Id == id);
                    return View(user);
                }
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            Session.RemoveAll();
            FormsAuthentication.SignOut();
            return RedirectToAction("index", "Home");
        }

        public ActionResult Search()
        {

           

            return View();
        }

        //[HttpPost]
        //public ActionResult AddFriend(int? id, Friend friend)
        //{
        //    using (MyDataContext db = new MyDataContext())
        //    {
        //        User from = (User)Session["Id"];
        //        User to = (User)User.Identity.GetUserId();

        //        friend.From = from;
        //        friend.To = to;

        //        db.Friends.
        //        db.SaveChanges();

        //        return View();
        //    }
        //}

    }
}
