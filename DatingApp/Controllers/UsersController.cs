using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DatingApp.Models;
using System.IO;

namespace DatingApp.Controllers
{
    public class UsersController : BaseController
    {
        //Tar in en söksträng som gör att man kan söka efter en användare, 
        //den kollar om personen är synlig för sökning och sen skriver ut de användare som matchar mot söksträngen. 
        public ActionResult SearchUser(string searchString)
        {
            var users = (from u in db.User
                         where u.IsVisible == false
                         select u);
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(x => x.Firstname.Contains(searchString));
            }
            return View(users.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        //När man skapar en profil ska man kunna ladda upp en bild tsm med sina övriga uppgifter.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Firstname,Lastname,Age,Email,Password")] User user, HttpPostedFileBase picUpload)
        {
            if (ModelState.IsValid)
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
            return View(user);
        }

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

        //Här lagras de nya värdena i databasen efter att användaren har ändrat det de vill.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Firstname,Lastname,Age,Email,Password")] User user)
        {
            if (ModelState.IsValid && Session["Id"] != null)
            {
                var userToEdit = db.User.Find(user.Id);

                userToEdit.Firstname = user.Firstname;
                userToEdit.Lastname = user.Lastname;
                userToEdit.Age = user.Age;
                userToEdit.Email = user.Email;
                userToEdit.Password = user.Password;

                db.SaveChanges();

                return RedirectToAction("Loggedin", "Users");
            }
            else
            {
                return RedirectToAction("Unauthorized", "Error");
            }
        }

        //Hämtar användaren som är inloggad, så det är den användarens profilbild som ändras.
        public ActionResult ChangeProfilePic(int? id)
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

        //Hämtar hem id till personen och kollar om man har lagt till en fil och lagrar i databasen.
        [HttpPost]
        public ActionResult ChangeProfilePic(HttpPostedFileBase picUpload, int? id)
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
            return View();
        }

        //Hämtar hem rätt bild till rätt persons profil.
        public ActionResult Image(int id)
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

        public ActionResult Login()
        {
            return View();
        }

        //Jämför det ifyllda användarnamnet och lösenordet med databasen data, om det stämmer loggas man in, 
        //annars får användaren ett felmeddelande.
        [HttpPost]
        public ActionResult Login(User user)
        {
            var usr = db.User.Where(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefault();
            if (usr != null)
            {
                usr.SId = HttpContext.Session.SessionID;
                db.SaveChanges();

                Session["Id"] = usr.Id.ToString();

                return RedirectToAction("Loggedin");
            }
            else
            {
                ModelState.AddModelError("", "Email or Password is invalid");
            }
            return View();
        }

        //Den person som är inloggad behöver komma åt vem som har skickat inlägg och vem man börjar följa.
        public ActionResult LoggedIn(int? Id)
        {
            if (Session["Id"] != null || Id != null)
            {
                int id = Id ?? int.Parse(Session["id"].ToString());

                var user = db.User
                    .Include(i => i.Posts.Select(x => x.Sender))
                    .Include(y => y.Friends.Select(x => x.To))
                    .Include(y => y.Friends.Select(x => x.From))
                    .First(x => x.Id == id);

                int idSession = int.Parse(Session["Id"].ToString());
                var followers = db.Friends
                    .Select(x => x.To)
                    .Where(x => x.Id == idSession)
                    .ToList();

                Session["CountFollowers"] = followers.Count;
                var usr = db.User.Find(id);
                Session["ModelId"] = usr.Id;

                return View(user);
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }

        //Eftersom vi använder Session så måste vi "döda" det befintliga Sessionet när en användare loggar ut,
        //så att den som loggar in får ett unikt. 
        public ActionResult LogOut()
        {
            Session.RemoveAll();
            Session.Clear();
            Session.Abandon();
            Response.AppendHeader("Cache-Control", "no-store");
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            FormsAuthentication.SignOut();
            return RedirectToAction("index", "Home");
        }

        //När man klickar på knappen så hämtar man hem den personens id man är inne på och vem som är inloggad och sparar detta
        //till databasen där ena är from (inloggad) och andra är to(till vem).
        public ActionResult AddFriend(int? id)
        {
            int idSession = int.Parse(Session["Id"].ToString());
            var user = db.User.First(x => x.Id == idSession);

            if (user != null)
            {
                User requestTo = db.User.Find(id);

                Friend friend = new Friend();
                friend.From = user;
                friend.To = requestTo;
                user.Friends.Add(friend);
                db.SaveChanges();

                return RedirectToAction("LoggedIn");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        //Kappen tar in personens id och sätter den till antingen false eller true om personen vill vara synlig eller ej 
        //och sparar detta.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IsVisible(int? id)
        {
            if (ModelState.IsValid)
            {
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
            return View();
        }

        //När man klickar på knappen dölj bild så aktiveras denna action och ändrar värdet på propertyn IsPicVisible.
        //Tar in användarens Id för att hitta vem som är inloggad. 
        public ActionResult HidePic(int? id)
        {
            User user = db.User.Find(id);

            if (user.IsPicVisible == false)
            {
                user.IsPicVisible = true;
                db.SaveChanges();
                return RedirectToAction("LoggedIn", "Users");
            }
            else
            {
                user.IsPicVisible = false;
                db.SaveChanges();
                return RedirectToAction("LoggedIn", "Users");
            }
        }
    }
}
