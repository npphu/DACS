using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S_FOOD.Models;

namespace S_FOOD.Controllers
{
    public class CTVKhuyenMaiController : Controller
    {
        DBSfoodDataContext data = new DBSfoodDataContext();
        // GET: CTVKhuyenMai
        public ActionResult Index()
        {
            if (Session["TenTK_CTV"] == null)
            {
                return RedirectToAction("DangNhap", "CTVTaiKhoan");
            }
            CONGTACVIEN tk = (CONGTACVIEN)Session["TenTK_CTV"];
            var monan = from m in data.KhuyenMais where m.MONAN.MaTK_CTV == tk.MaTK_CTV select m;
           // List<KHUYENMAI> monan = data.KHUYENMAIs.ToList();
            return View(monan);
        }
        
        // GET: AdminKhuyenMai/Details/5
        public ActionResult Details(string id)
        {
            if (Session["TenTK_CTV"] == null)
            {
                return RedirectToAction("DangNhap", "CTVTaiKhoan");
            }
           /* GiaDaGiam gia = new GiaDaGiam(id);
            decimal g = 0;
            g = gia.dGiaDaGiam;
            ViewBag.Gia = g;
            KHUYENMAI km = this.data.KHUYENMAIs.FirstOrDefault(i => i.ID_KhuyenMai == id);*/
            return View();
        }

        // GET: AdminKhuyenMai/Create
        public ActionResult Create()
        {
            if (Session["TenTK_CTV"] == null)
            {
                return RedirectToAction("DangNhap", "CTVTaiKhoan");
            }
            return View();
        }

        // POST: AdminKhuyenMai/Create
        [HttpPost]

        public ActionResult Create(FormCollection collection, KhuyenMai km)
        {

            var giagiam = Convert.ToDouble(collection["GiaGiam"]);
            int monan = Convert.ToInt32(collection["IDMonAn"]);
            ViewBag.MessageFail = string.Empty;
            if (monan == 0)
            {
                ViewBag.MessageFail += "Không có thực đơn để thêm. ";
            }
            if (giagiam < 0 || giagiam > 100)
            {
                ViewBag.MessageFail += "% Khuyến mại không hợp lệ. ";
            }
            if (!string.IsNullOrEmpty(ViewBag.MessageFail))
            {
                return View();
            }

            KhuyenMai khuyenMai = new KhuyenMai();
            khuyenMai.ID_MonAn = monan;
            khuyenMai.GiamGia = giagiam;

            if (ModelState.IsValid)
            {
                data.KhuyenMais.InsertOnSubmit(khuyenMai);
                try
                {
                    data.SubmitChanges();
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Violation of PRIMARY KEY constraint"))
                    {
                        ViewBag.MessageFail = string.Format("Đã tồn tại khuyến mại cho thực đơn {0}.", khuyenMai.MONAN.TenMonAn);
                    }
                    else
                    {
                        ViewBag.ErrorBody = ex.ToString();
                    }
                    return View();
                }
            }
            ViewBag.MessageSuccess = "Thêm khuyến mại: [" + khuyenMai.MONAN.TenMonAn + "] thành công";
            return View();
        }

        // GET: AdminKhuyenMai/Edit/5
        public ActionResult Edit(int id)
        {
            if (Session["TenTK_CTV"] == null)
            {
                return RedirectToAction("DangNhap", "CTVTaiKhoan");
            }
            KhuyenMai khuyenMai = data.KhuyenMais.SingleOrDefault(i => i.ID_MonAn == id);
            return View(khuyenMai);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection form, KhuyenMai km)
        {

            var ten = Convert.ToInt32(form["IDMonAn"]);
            KhuyenMai khuvuc = data.KhuyenMais.SingleOrDefault(i => i.ID_MonAn == km.ID_MonAn);
            khuvuc.ID_MonAn = ten;
            khuvuc.GiamGia = km.GiamGia;
            if(ModelState.IsValid)
            {
                UpdateModel(khuvuc);
                data.SubmitChanges();
            }
            return RedirectToAction("Index");
        }
        // GET: AdminKhuyenMai/Delete/5
        public ActionResult Delete(int id)
        {
            if (Session["TenTK_CTV"] == null)
            {
                return RedirectToAction("DangNhap", "CTVTaiKhoan");
            }
            KhuyenMai km = data.KhuyenMais.SingleOrDefault(n => n.ID_MonAn == id);
            ViewBag.ID = km.ID_MonAn;
            if (km == null)
            {

                return RedirectToAction("Index", "CTVKhuyenMai");
            }
            return View(km);
        }

        // POST: AdminKhuyenMai/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {

            KhuyenMai km = data.KhuyenMais.SingleOrDefault(i => i.ID_MonAn == id);
            /*if (countMonAn > 0)
            {
                ViewBag.ErrorBody = string.Format("Không thể Xóa,  do mã khuyến mại[{0}] đang áp dụng cho [{1}] món ăn.", km.ID_KhuyenMai, countMonAn);
                List<KHUYENMAI> all = data.KHUYENMAIs.ToList();
                return View("Index", all.ToList());
            }
            else*/
            if (ModelState.IsValid)
            {
                data.KhuyenMais.DeleteOnSubmit(km);
                data.SubmitChanges();
            }
            return RedirectToAction("Index");
        }
        [ChildActionOnly]
        public ActionResult PV_Dropdown_MonAn(int? id)
        {
            if (Session["TenTK_CTV"] == null)
            {
                return RedirectToAction("DangNhap", "CTVTaiKhoan");
            }
            CONGTACVIEN tk = (CONGTACVIEN)Session["TenTK_CTV"];
            var monan = from m in data.MONANs where m.MaTK_CTV == tk.MaTK_CTV select m;
            if (id != null)
            {
                ViewBag.idLoai = id;
            }
            return PartialView(monan);
        }
        [HttpPost]
        public ActionResult ThucDonById(int? val)
        {
            if (val != null)
            {
                //Values are hard coded for demo. you may replae with values 
                // coming from your db/service based on the passed in value ( val.Value)
                int id = val.Value;
                MONAN thucDon = data.MONANs.SingleOrDefault(i => i.ID_MonAn == id);
                string donGia = Convert.ToDecimal(thucDon.GiaBan).ToString("N0") + "đ";
                return Json(new { Success = "true", Data = new { DonGia = donGia } });
            }
            return Json(new { Success = "false" });
        }

        [HttpPost]
        public ActionResult GetGiaKhuyenMai(int? idThucDon, int? val)
        {
            if (val != null && idThucDon != null)
            {
                //Values are hard coded for demo. you may replae with values 
                // coming from your db/service based on the passed in value ( val.Value)
                double percent = Convert.ToDouble(val.Value) / 100;
                int id = idThucDon.Value;
                MONAN thucDon = data.MONANs.SingleOrDefault(i => i.ID_MonAn == id);
                if (thucDon != null)
                {
                    decimal giaGiam = Convert.ToDecimal(thucDon.GiaBan) * Convert.ToDecimal(percent);
                    string giaKhuyenMai = (Convert.ToDecimal(thucDon.GiaBan) - giaGiam).ToString("N0") + "đ";
                    return Json(new { Success = "true", Data = new { GiaKhuyenMai = giaKhuyenMai } });
                }
                else
                {
                    return Json(new { Success = "false" });
                }

            }
            return Json(new { Success = "false" });
        }

    }
}
