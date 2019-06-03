using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S_FOOD.Models;

namespace S_FOOD.Controllers
{
    public class AdminKhuVucController : Controller
    {
        DBSfoodDataContext data = new DBSfoodDataContext();
        // GET: AdminKhuVuc
        public ActionResult Index()
        {
            if (Session["TenTK_Admin"] == null)
            {
                return RedirectToAction("DangNhap", "AdminTaiKhoan");
            }
            List<KHUVUC> all = data.KHUVUCs.ToList();
            return View(all);
        }

        // GET: AdminKhuVuc/Details/5
        public ActionResult Details(int id)
        {
            if (Session["TenTK_Admin"] == null)
            {
                return RedirectToAction("DangNhap", "AdminTaiKhoan");
            }
            
            KHUVUC kv = data.KHUVUCs.SingleOrDefault(i => i.ID_KhuVuc == id);
            return View(kv);
        }

        // GET: AdminKhuVuc/Create
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
        public ActionResult Create(FormCollection collection, KHUVUC kv)
        {
            var ten = collection["TenKhuVuc"];
            kv.TenKhuVuc = ten;
            data.KHUVUCs.InsertOnSubmit(kv);
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
            KHUVUC kv = data.KHUVUCs.SingleOrDefault(i => i.ID_KhuVuc == id);
            return View(kv);
        }

        // POST: AdminKhuVuc/Edit/5
        [HttpPost]
        public ActionResult Edit(KHUVUC kv, FormCollection collection)
        {

            KHUVUC khuvuc = data.KHUVUCs.SingleOrDefault(i => i.ID_KhuVuc == kv.ID_KhuVuc);
            khuvuc.TenKhuVuc = kv.TenKhuVuc;           
            if (ModelState.IsValid)
            {

                UpdateModel(khuvuc);
                data.SubmitChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: AdminKhuVuc/Delete/5
        public ActionResult Delete(int id)
        {
            if (Session["TenTK_Admin"] == null)
            {
                return RedirectToAction("DangNhap", "AdminTaiKhoan");
            }
            KHUVUC kv = data.KHUVUCs.SingleOrDefault(n => n.ID_KhuVuc == id);
            ViewBag.ID = kv.ID_KhuVuc;
            if (kv == null)
            {

                return RedirectToAction("Index", "AdminVIP");
            }
            return View(kv);
        }

        // POST: AdminKhuVuc/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            KHUVUC kv = data.KHUVUCs.SingleOrDefault(i => i.ID_KhuVuc == id);
            int countKhuVuc = data.MONANs.Count(i => i.ID_KhuVuc == id);
            if (countKhuVuc > 0)
            {
                ViewBag.ErrorBody = string.Format("Không thể Xóa,  do Khu Vực [{0}] đang có [{1}] món ăn.", kv.TenKhuVuc, countKhuVuc);
                List<KHUVUC> all = data.KHUVUCs.ToList();
                return View("Index", all.ToList());
            }
            else
            if (ModelState.IsValid)
            {
                data.KHUVUCs.DeleteOnSubmit(kv);
                data.SubmitChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
