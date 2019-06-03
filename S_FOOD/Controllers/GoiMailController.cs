using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Net;
using System.Net.Mail;
using S_FOOD.Models;

namespace S_FOOD.Controllers
{
    public class GoiMailController : Controller
    {
        DBSfoodDataContext data = new DBSfoodDataContext();
        // GET: GoiMail
        public ActionResult Index()
        {
            return View();
        }

        
    }
}