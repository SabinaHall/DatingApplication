﻿using System.Web.Mvc;

namespace DatingApp.Controllers
{
    //Skapat error-klasser som fångar specifika fel.
    public class ErrorController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }

        public ViewResult NotFound()
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }

        public ViewResult Unauthorized()
        {
            Response.StatusCode = 401;
            return View("Unauthorized");
        }
    }
}