using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S_FOOD.Models;

namespace S_FOOD.Controllers
{
    public class CTVDonHangController : Controller
    {
        DBSfoodDataContext data = new DBSfoodDataContext();
        // GET: CTVDonHang
        public ActionResult Index()
        {
           
            if (Session["TenTK_CTV"] == null)
            {
                return RedirectToAction("DangNhap", "CTVTaiKhoan");
            }
            CONGTACVIEN tk = (CONGTACVIEN)Session["TenTK_CTV"];
            var monan = from m in data.DONHANGs where m.MONAN.MaTK_CTV == tk.MaTK_CTV where m.DATHANG.TinhTrangGiao == false select m;
            return View(monan);
        }

        // GET: CTVDonHang/Details/5
        public ActionResult Details(int id)
        {
            if (Session["TenTK_CTV"] == null)
            {
                return RedirectToAction("DangNhap", "CTVTaiKhoan");
            }
            DONHANG dh = this.data.DONHANGs.FirstOrDefault(i => i.ID_DatHang == id);
            return View(dh);
        }

        // GET: CTVDonHang/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CTVDonHang/Create
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

        // GET: CTVDonHang/Edit/5
        public ActionResult Edit(int id)
        {
            if (Session["TenTK_CTV"] == null)
            {
                return RedirectToAction("DangNhap", "CTVTaiKhoan");
            }
            DATHANG ctv = data.DATHANGs.SingleOrDefault(i => i.ID_DatHang == id);
            return View(ctv);
        }

        // POST: CTVDonHang/Edit/5
        [HttpPost]
        public ActionResult Edit(DATHANG dh, FormCollection collection)
        {
            DATHANG ctv = data.DATHANGs.SingleOrDefault(i => i.ID_DatHang == dh.ID_DatHang);
            ctv.DaThanhToan = dh.DaThanhToan;
            ctv.TinhTrangGiao = dh.TinhTrangGiao;
            {
                UpdateModel(ctv);
                data.SubmitChanges();
            }
            return RedirectToAction("Index");
        }

    }
}
