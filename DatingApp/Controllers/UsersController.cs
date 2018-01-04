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
using System.IO;

namespace DatingApp.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        //Tar in en söksträng som gör att man kan söka efter en användare, den kollar om personen är synlig och sen skriver ut de användare som matchar mot söksträngen. 
        public ActionResult Index(string searchString)
        {
            using (MyDataContext db = new MyDataContext())
            {
                var users = (from u in db.User
                             where u.IsVisible == false
                             select u);
                if (!String.IsNullOrEmpty(searchString))
                {
                    users = users.Where(x => x.Firstname.Contains(searchString));
                    //return View(users);
                }
                return View(users.ToList());
            }
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (MyDataContext db = new MyDataContext())
            {
                User user = db.User.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        //När man skapar en profil ska man kunna ladda upp en bild. Man kollar om den är ifylld eller inte och lägger till 
        //detta till personen i databasen och läser upp bilden på profilsidan. Man kan ladda upp med bild eller utan bild. 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Firstname,Lastname,Age,Email,Password")] User user, HttpPostedFileBase picUpload)
        {
            if (ModelState.IsValid)
            {
                using (MyDataContext db = new MyDataContext())
                {
                    if (picUpload != null && picUpload.ContentLength > 0)
                    {
                        user.Filename = picUpload.FileName;
                        user.ContentType = picUpload.ContentType;
                        db.User.Add(user);

                        using (var reader = new BinaryReader(picUpload.InputStream))
                        {
                            user.File = reader.ReadBytes(picUpload.ContentLength);
                        }
                        //db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Login", "Users");
                    }
                    else
                    {
                        db.User.Add(user);
                        db.SaveChanges();
                        return RedirectToAction("Login", "Users");
                    }
                }
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
            using (MyDataContext db = new MyDataContext())
            {
                User user = db.User.Find(id);
                if (user == null)
                {
                    db.SaveChanges();
                    return HttpNotFound();
                }
                return View(user);
            }
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Firstname,Lastname,Age,Email,Password")] User user)
        {
            if (ModelState.IsValid && Session["Id"] != null)
            {
                using (MyDataContext db = new MyDataContext())
                {
                    //var hej = new User { Firstname = user.Firstname, Email = user.Email };
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Loggedin", "Users");
                }
            }
            else
            {
                return RedirectToAction("Unauthorized", "Error");
            }
            //return View(user);
        }

        //GET
        public ActionResult ChangeProfilePic(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (MyDataContext db = new MyDataContext())
            {
                User user = db.User.Find(id);
                if (user == null)
                {
                    db.SaveChanges();
                    return HttpNotFound();
                }
                return View(user);
            }
        }

        //Hämtar hem id till personen och kollar om man har lagt till en ful och sedan läser man upp filen på profilsidan
        [HttpPost]
        public ActionResult ChangeProfilePic(HttpPostedFileBase picUpload, int? id)
        {
            using (MyDataContext db = new MyDataContext())
            {
                User user = db.User.Find(id);

                if (picUpload != null && picUpload.ContentLength > 0)
                {
                    user.Filename = picUpload.FileName;
                    user.ContentType = picUpload.ContentType;

                    using (var reader = new BinaryReader(picUpload.InputStream))
                    {
                        user.File = reader.ReadBytes(picUpload.ContentLength);
                    }

                    db.SaveChanges();
                    db.Entry(user).State = EntityState.Modified;

                    return RedirectToAction("Loggedin", "Users");
                }
            }
            return View();
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (MyDataContext db = new MyDataContext())
            {
                User user = db.User.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (MyDataContext db = new MyDataContext())
            {
                User user = db.User.Find(id);
                db.User.Remove(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            using (MyDataContext db = new MyDataContext())
            {
                if (disposing)
                {
                    db.Dispose();
                }
                base.Dispose(disposing);
            }
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
                    Session["Friends"] = usr.Friends.Count;

                    return RedirectToAction("Loggedin");
                }
                else
                {
                    ModelState.AddModelError("", "Email or Password is invalid");
                }
            }
            return View();
        }

        //Den person som är inloggad behöver komma åt vem som har skickat inlägg och vem man skickar en vänförfrågan till.
        //Här görs en lambda som hjälper oss att hämta detta. 
        public ActionResult LoggedIn(int? Id)
        {
            if (Session["Id"] != null || Id != null)
            {
                using (MyDataContext db = new MyDataContext())
                {
                    int id = Id ?? int.Parse(Session["id"].ToString());
                    var user = db.User
                        .Include(i => i.Posts.Select(x => x.Sender))
                        .Include(y => y.Friends.Select(x => x.To))
                        .First(x => x.Id == id);
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

        //När man klickar på knappen så hämtar man hem den personens id man är inne på och vem som är inloggad och sparar detta
        //till databasen där ena är from (inloggad) och andra är to(till vem).
        public ActionResult AddFriend(int? id)
        {
            using (MyDataContext db = new MyDataContext())
            {
                int idSession = int.Parse(Session["Id"].ToString());
                var user = db.User.First(x => x.Id == idSession);

                if (user != null)
                {
                    User requestTo = db.User.Find(id);

                    Friend friend = new Friend();
                    friend.From = user;
                    friend.To = requestTo;
                    friend.IsFriend = true;
                    user.Friends.Add(friend);
                    db.SaveChanges();

                    return RedirectToAction("LoggedIn");
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
        }

        //Hämtar hem rätt bild till rätt persons profil
        public ActionResult Image(int id)
        {
            using (MyDataContext db = new MyDataContext())
            {
                var user = db.User.Single(x => x.Id == id);
                if (user.File != null && user.ContentType != null)
                {
                    return File(user.File, user.ContentType);
                }
                if (user.File == null && user.ContentType == null)
                {
                    return RedirectToAction("LoggedIn", "Users");
                }
                return View();
            }
        }

        ////GET
        //public ActionResult IsVisible(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    using (MyDataContext db = new MyDataContext())
        //    {
        //        User user = db.User.Find(id);
        //        if (user == null)
        //        {
        //            db.SaveChanges();
        //            return HttpNotFound();
        //        }
        //        return View(user);
        //    }
        //}


        //Kappen tar in personens id och sätter den till antingen false eller true om personen vill vara synlig eller ej och sparar detta
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IsVisible(int? id)
        {
            if (ModelState.IsValid)
            {
                using (MyDataContext db = new MyDataContext())
                {
                    //int sessionId = int.Parse(Session["id"].ToString());

                    User user = db.User.Find(id);
                    if (user.IsVisible == false)
                    {
                        user.IsVisible = true;

                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Loggedin", "Users");
                    }
                    else
                    {
                        user.IsVisible = false;

                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Loggedin", "Users");
                    }
                }
            }
            return View();
        }
    }
}
