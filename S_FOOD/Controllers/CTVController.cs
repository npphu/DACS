using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S_FOOD.Models;

namespace S_FOOD.Controllers
{
    public class CTVController : Controller
    {
        // GET: CTV
        public ActionResult Index()
        {
            if (Session["TenTK_CTV"] == null)
            {
                return RedirectToAction("DangNhap", "CTVTaiKhoan");
            }
            return View();
        }

        // GET: CTV/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CTV/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CTV/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CTV/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CTV/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CTV/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CTV/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
