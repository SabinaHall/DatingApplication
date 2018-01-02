using DatingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace DatingApp.Controllers.Api
{
    public class UsersController : ApiController
    {
        public class PostListItem
        {
            public int Id { get; set; }
            public string Message { get; set; }
            public User Reciver { get; set; }
        }

        //// GET: Users
        //public List<Post> PostList()
        //{
        //    Post post = new Post();
        //    User user = new User();
        //    return user.Posts
        //        .Select(postList => new PostListItem
        //        {
        //            Id = postList.PostId,
        //            Message = postList.Message,
        //            Reciver = postList.Receiver
        //        })
        //        .ToList();
        //}
    }
}