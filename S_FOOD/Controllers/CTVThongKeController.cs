using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S_FOOD.Models;

namespace S_FOOD.Controllers
{
    public class CTVThongKeController : Controller
    {
        // GET: SFood
        DBSfoodDataContext data = new DBSfoodDataContext();
        public List<MONAN> MonAnBanChay(int count)
        {
            return data.MONANs.OrderByDescending(a => a.SoLuong).Take(count).ToList();

        }
        public ActionResult PV_ThongKeKhachHang()
        {
            if (Session["TenTK_CTV"] == null)
            {
                return RedirectToAction("DangNhap", "CTVTaiKhoan");
            }
            CONGTACVIEN tk = (CONGTACVIEN)Session["TenTK_CTV"];
            var top = from m in data.V_Top5KhachHangs where m.MaTK_CTV == tk.MaTK_CTV select m;
            return PartialView(top);
        }

        public ActionResult PV_ThongKeMonAn()
        {
            if (Session["TenTK_CTV"] == null)
            {
                return RedirectToAction("DangNhap", "CTVTaiKhoan");
            }
            CONGTACVIEN tk = (CONGTACVIEN)Session["TenTK_CTV"];
            var top = from m in data.V_Top5MonAns where m.MaTK_CTV == tk.MaTK_CTV select m;
            return PartialView(top);
        }
        // GET: CTVThongKe
        public ActionResult Index()
        {
                        
            return View();
        }

        // GET: CTVThongKe/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CTVThongKe/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CTVThongKe/Create
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

        // GET: CTVThongKe/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CTVThongKe/Edit/5
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

        // GET: CTVThongKe/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CTVThongKe/Delete/5
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
