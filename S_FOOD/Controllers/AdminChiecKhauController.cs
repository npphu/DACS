using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S_FOOD.Models;

namespace S_FOOD.Controllers
{
    public class AdminChiecKhauController : Controller
    {
        DBSfoodDataContext data = new DBSfoodDataContext();
        // GET: AdminChiecKhau
        public ActionResult Index()
        {
            if (Session["TenTK_Admin"] == null)
            {
                return RedirectToAction("DangNhap", "AdminTaiKhoan");
            }
            List<CHIECKHAU> all = data.CHIECKHAUs.ToList();
            return View(all.ToList());
        }

        // GET: AdminChiecKhau/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminChiecKhau/Create
        public ActionResult Create()
        {
            if (Session["TenTK_Admin"] == null)
            {
                return RedirectToAction("DangNhap", "AdminTaiKhoan");
            }
            return View();
        }

        // POST: AdminKhuVuc/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, CHIECKHAU kv)
        {
            var ten = Convert.ToInt32(collection["GiaTri"]);
            kv.GiaTriCK = ten;
            data.CHIECKHAUs.InsertOnSubmit(kv);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }

        // GET: AdminKhuVuc/Edit/5
        public ActionResult Edit(int id)
        {
            if (Session["TenTK_Admin"] == null)
            {
                return RedirectToAction("DangNhap", "AdminTaiKhoan");
            }
            CHIECKHAU kv = data.CHIECKHAUs.SingleOrDefault(i => i.ID_ChiecKhau == id);
            return View(kv);
        }

        // POST: AdminKhuVuc/Edit/5
        [HttpPost]
        public ActionResult Edit(CHIECKHAU kv, FormCollection collection)
        {

            CHIECKHAU khuvuc = data.CHIECKHAUs.SingleOrDefault(i => i.ID_ChiecKhau == kv.ID_ChiecKhau);
            khuvuc.GiaTriCK = kv.GiaTriCK;
            if (ModelState.IsValid)
            {

                UpdateModel(khuvuc);
                data.SubmitChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: AdminChiecKhau/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminChiecKhau/Delete/5
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
