using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S_FOOD.Models;

namespace S_FOOD.Controllers
{
    public class AdminVIPController : Controller
    {
        DBSfoodDataContext data = new DBSfoodDataContext();
        // GET: AdminVIP
        public ActionResult Index()
        {
            if (Session["TenTK_Admin"] == null)
            {
                return RedirectToAction("DangNhap", "AdminTaiKhoan");
            }
            List<VIP> all = data.VIPs.ToList();
            return View(all);           
        }

        // GET: AdminVIP/Details/5
        public ActionResult Details(int id)
        {
            if (Session["TenTK_Admin"] == null)
            {
                return RedirectToAction("DangNhap", "AdminTaiKhoan");
            }
            VIP ctv = data.VIPs.SingleOrDefault(i => i.LevelVip == id);
            return View(ctv);          
        }

        // GET: AdminVIP/Create
        public ActionResult Create()
        {
            if (Session["TenTK_Admin"] == null)
            {
                return RedirectToAction("DangNhap", "AdminTaiKhoan");
            }
            return View();
        }

        // POST: AdminVIP/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, VIP vip)
        {
            decimal gia = Convert.ToDecimal(collection["GiaQuangCao"]);                                          
                vip.SoTienCanTra = gia;
                data.VIPs.InsertOnSubmit(vip);
                data.SubmitChanges();
                return RedirectToAction("Index","AdminVIP");                       
        }

        // GET: AdminVIP/Edit/5
        public ActionResult Edit(int id)
        {
            if (Session["TenTK_Admin"] == null)
            {
                return RedirectToAction("DangNhap", "AdminTaiKhoan");
            }
            VIP ctv = data.VIPs.SingleOrDefault(i => i.LevelVip == id);
            return View(ctv);
        }

        // POST: AdminVIP/Edit/5
        [HttpPost]
        public ActionResult Edit(VIP vip, FormCollection collection)
        {
            
            VIP ctv = data.VIPs.SingleOrDefault(i => i.LevelVip == vip.LevelVip);
            var gia = Convert.ToDecimal( collection["GiaQuangCao"]);
            ctv.SoTienCanTra = gia;                     
            if (ModelState.IsValid)
            {

                UpdateModel(ctv);
                data.SubmitChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: AdminVIP/Delete/5
        public ActionResult Delete(int id)
        {
            if (Session["TenTK_Admin"] == null)
            {
                return RedirectToAction("DangNhap", "AdminTaiKhoan");
            }
            VIP vip = data.VIPs.SingleOrDefault(n => n.LevelVip == id);
            ViewBag.ID = vip.LevelVip;
            if (vip == null)
            {

                return RedirectToAction("Index", "AdminVIP");
            }
            return View(vip);
        }

        // POST: AdminVIP/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            VIP vip = data.VIPs.SingleOrDefault(i => i.LevelVip == id);
            int  countCTV = data.CONGTACVIENs.Count(i => i.LevelVip == id);           
            if (countCTV > 0)
            {
                 ViewBag.ErrorBody = string.Format("Không thể Xóa Level Vip [{0}] do có [{1}] Cộng tác viên đang sử dụng dịch vụ.", vip.LevelVip, countCTV);
                 List<VIP> all = data.VIPs.ToList();
                 return View("Index", all.ToList());
            }                  
            else                   
            if (ModelState.IsValid)
            {
                data.VIPs.DeleteOnSubmit(vip);                   
                data.SubmitChanges();                                                                  
            }         
            return RedirectToAction("Index", "AdminVIP");

        }
        public ActionResult Loi()
        {
            return PartialView();
        }
    }
}
