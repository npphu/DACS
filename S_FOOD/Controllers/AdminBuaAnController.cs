using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S_FOOD.Models;

namespace S_FOOD.Controllers
{
    public class AdminBuaAnController : Controller
    {
        DBSfoodDataContext data = new DBSfoodDataContext();
        // GET: AdminBuaAn
        public ActionResult Index()
        {
            if (Session["TenTK_Admin"] == null)
            {
                return RedirectToAction("DangNhap", "AdminTaiKhoan");
            }
            List<LOAIBUAAN> all = data.LOAIBUAANs.ToList();
            return View(all);
        }

        // GET: AdminBuaAn/Details/5
        public ActionResult Details(int id)
        {
            if (Session["TenTK_Admin"] == null)
            {
                return RedirectToAction("DangNhap", "AdminTaiKhoan");
            }

            LOAIBUAAN kv = data.LOAIBUAANs.SingleOrDefault(i => i.ID_Loai == id);
            return View(kv);
        }

        // GET: AdminBuaAn/Create
        public ActionResult Create()
        {
            if (Session["TenTK_Admin"] == null)
            {
                return RedirectToAction("DangNhap", "AdminTaiKhoan");
            }
            return View();
        }

        // POST: AdminBuaAn/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, LOAIBUAAN loai)
        {
            var ten = collection["TenBuaAn"];
            loai.TenLoai = ten;
            data.LOAIBUAANs.InsertOnSubmit(loai);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }

        // GET: AdminBuaAn/Edit/5
        public ActionResult Edit(int id)
        {
            if (Session["TenTK_Admin"] == null)
            {
                return RedirectToAction("DangNhap", "AdminTaiKhoan");
            }
            LOAIBUAAN kv = data.LOAIBUAANs.SingleOrDefault(i => i.ID_Loai == id);
            return View(kv);
        }

        // POST: AdminBuaAn/Edit/5
        [HttpPost]
        public ActionResult Edit(LOAIBUAAN loai, FormCollection collection)
        {
            LOAIBUAAN l = data.LOAIBUAANs.SingleOrDefault(i => i.ID_Loai == loai.ID_Loai);
            l.TenLoai = loai.TenLoai;
            if (ModelState.IsValid)
            {

                UpdateModel(l);
                data.SubmitChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: AdminBuaAn/Delete/5
        public ActionResult Delete(int id)
        {
            if (Session["TenTK_Admin"] == null)
            {
                return RedirectToAction("DangNhap", "AdminTaiKhoan");
            }
            LOAIBUAAN kv = data.LOAIBUAANs.SingleOrDefault(n => n.ID_Loai == id);
            ViewBag.ID = kv.ID_Loai;
            if (kv == null)
            {

                return RedirectToAction("Index", "AdminVIP");
            }
            return View(kv);
        }

        // POST: AdminBuaAn/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            LOAIBUAAN kv = data.LOAIBUAANs.SingleOrDefault(i => i.ID_Loai == id);
            int countLoai = data.MONANs.Count(i => i.ID_Loai == id);
            if (countLoai > 0)
            {
                ViewBag.ErrorBody = string.Format("Không thể Xóa,  do Loại [{0}] đang có [{1}] món ăn.", kv.TenLoai, countLoai);
                List<LOAIBUAAN> all = data.LOAIBUAANs.ToList();
                return View("Index", all.ToList());
            }
            else
            if (ModelState.IsValid)
            {
                data.LOAIBUAANs.DeleteOnSubmit(kv);
                data.SubmitChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
