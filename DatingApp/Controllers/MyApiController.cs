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
        public void Create([FromBody] Post post, int? id)
        {
            //var session = HttpContext.Current.Session;

            if (ModelState.IsValid)
            {
                using (MyDataContext db = new MyDataContext())
                {
                    User sender = db.User.Find(id);

                    post.Sender = sender;
                    post.Receiver = sender;

                    sender.Posts.Add(post);
                    db.SaveChanges();
                }
            }
        }

        public int Hej()
        {
            return 0;
        }








        //public List<string> Test()
        //{
        //    List<string> lista = new List<string>();
        //    var hej = "hej";
        //    var hello = "hello";
        //    lista.Add(hej);
        //    lista.Add(hello);

        //    return lista;
        //}

        //[System.Web.Http.HttpGet]
        //public List<Post> List(int? id)
        //{
        //    User user = db.User.Find(id);
        //    return user.Posts.ToList();
        //}
    }
}