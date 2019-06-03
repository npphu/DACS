using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S_FOOD.Models;
using PagedList;
using PagedList.Mvc;

namespace S_FOOD.Controllers
{
    public class CTVMonAnController : Controller
    {
        DBSfoodDataContext data = new DBSfoodDataContext();
        // GET: CTVMonAn
        public ActionResult Index()
        {
            if (Session["TenTK_CTV"] == null)
            {
                return RedirectToAction("DangNhap", "CTVTaiKhoan");
            }
            CONGTACVIEN tk = (CONGTACVIEN)Session["TenTK_CTV"];
            var monan = from m in data.MONANs where m.MaTK_CTV == tk.MaTK_CTV select m;
         
            return View(monan);
        }
        public ActionResult MonAnTheoCTV()
        {
            if (Session["TenTK_CTV"] == null)
            {
                return RedirectToAction("DangNhap", "CTVTaiKhoan");
            }
            CONGTACVIEN tk = (CONGTACVIEN)Session["TenTK_CTV"];
            var monan = from m in data.MONANs where m.MaTK_CTV == tk.MaTK_CTV select m;
            return View(monan);
        }
        // GET: CTVMonAn/Details/5
        public ActionResult Details(int id)
        {
            if (Session["TenTK_CTV"] == null)
            {
                return RedirectToAction("DangNhap", "CTVTaiKhoan");
            }
            MONAN ctv = data.MONANs.SingleOrDefault(i => i.ID_MonAn == id);
            return View(ctv);

        }

        // GET: CTVMonAn/Create
        public ActionResult Create()
        {
            if (Session["TenTK_CTV"] == null)
            {
                return RedirectToAction("DangNhap", "CTVTaiKhoan");
            }
            return View();
        }

        // POST: CTVMonAn/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection form, HttpPostedFileBase HinhAnh)
        {
            string tenMonAn = form["TenMonAn"];
            string gioiThieu = form["GioiThieu"];
            decimal giaban = Decimal.Parse( form["GiaBan"]);
            decimal phivanchuyen = Decimal.Parse(form["PhiVanChuyen"]);
            int soluong = Int32.Parse( form["SoLuong"]);
            int idkhuvuc = Int32.Parse( form["ID_KhuVuc"]);
            int idLoai = Int32.Parse(form["ID_Loai"]);
            ViewBag.MessageFail = string.Empty;
            if (string.IsNullOrWhiteSpace(tenMonAn))
            {
                ViewBag.MessageFail += "Tên món ăn không hợp lệ. ";
            }
            if (!string.IsNullOrEmpty(ViewBag.MessageFail))
            {
                return View();
            }

            string _FileName = "";
            try
            {
                if (HinhAnh.ContentLength > 0)
                {
                    string _folderPath = Path.Combine(Server.MapPath("~/images/MonAn/"));
                    // Determine whether the directory exists.
                    if (!Directory.Exists(_folderPath))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(_folderPath);
                    }

                    _FileName = Path.GetFileName(HinhAnh.FileName);
                    string _path = Path.Combine(Server.MapPath("~/images/MonAn/"), _FileName);
                    if (!System.IO.File.Exists(_path))
                    {
                        HinhAnh.SaveAs(_path);
                    }
                }
            }
            catch
            {
            }

            MONAN monAn = new MONAN();
            CONGTACVIEN tk = (CONGTACVIEN)Session["TenTK_CTV"];
            monAn.TenMonAn = tenMonAn;
            monAn.GiaBan = giaban;
            monAn.PhiVanChuyen = phivanchuyen;
            monAn.MoTa = gioiThieu;
            monAn.AnhBia = _FileName;
            monAn.NgayCapNhap = DateTime.Now;
            monAn.SoLuong = soluong;
            monAn.ID_KhuVuc = idkhuvuc;
            monAn.ID_Loai = idLoai;
            monAn.MaTK_CTV = tk.MaTK_CTV;

            if (ModelState.IsValid)
            {
                data.MONANs.InsertOnSubmit(monAn);
                data.SubmitChanges();
            }
            ViewBag.MessageSuccess = "Thêm món ăn: [" + tenMonAn + "] thành công";
            return View();
        }

        // GET: CTVMonAn/Edit/5
        public ActionResult Edit(int id)
        {
            if (Session["TenTK_CTV"] == null)
            {
                return RedirectToAction("DangNhap", "CTVTaiKhoan");
            }
            MONAN ctv = data.MONANs.SingleOrDefault(i => i.ID_MonAn == id);
            return View(ctv);
        }

        // POST: CTVMonAn/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection form, HttpPostedFileBase fileUpload)
        {
            int id = Convert.ToInt32(form["ID_MonAn"]);
            string mota = form["GioiThieu"];
            MONAN monan = data.MONANs.SingleOrDefault(i => i.ID_MonAn == id);
            monan.NgayCapNhap = DateTime.Now;
            String path = "";
            String fileName = "";
            if( fileUpload == null)
            {
            }
            else
            {
                fileName = Path.GetFileName(fileUpload.FileName);
                path = Path.Combine(Server.MapPath("~/images/MonAn"), fileName);
                fileUpload.SaveAs(path);
                monan.AnhBia = fileName;

            }


            if (ModelState.IsValid)
            {
                monan.MoTa = mota;
                UpdateModel(monan);
                data.SubmitChanges();
            }            
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            if (Session["TenTK_CTV"] == null)
            {
                return RedirectToAction("DangNhap", "CTVTaiKhoan");
            }
            MONAN km = data.MONANs.SingleOrDefault(n => n.ID_MonAn == id);
            ViewBag.ID = km.ID_MonAn;
            if (km == null)
            {

                return RedirectToAction("Index", "CTVMonAn");
            }
            return View(km);
        }
        // POST: CTVMonAn/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection, int? page)
        {
            MONAN ctv = data.MONANs.SingleOrDefault(i => i.ID_MonAn == id);
            int countMonAn = data.DONHANGs.Count(i => i.ID_MonAn == id);
            int coutKM = data.KhuyenMais.Count(i => i.ID_MonAn == id);

            if (countMonAn > 0)
            {

                ViewBag.ErrorBody = string.Format("Không thể Xóa  do [{0}] đang có [{1}] đơn hàng.", ctv.TenMonAn, countMonAn);
                CONGTACVIEN tk = (CONGTACVIEN)Session["TenTK_CTV"];
                var monan = from m in data.MONANs where m.MaTK_CTV == tk.MaTK_CTV select m;
                return View("Index", monan);

            }
            if (coutKM > 0)
            {

                ViewBag.ErrorKM = string.Format("Không thể Xóa  do [{0}] đang được Khuyến Mại.", ctv.TenMonAn);
                CONGTACVIEN tk = (CONGTACVIEN)Session["TenTK_CTV"];
                var monan = from m in data.MONANs where m.MaTK_CTV == tk.MaTK_CTV select m;
                return View("Index", monan);

            }
            else
             if (ModelState.IsValid)
            {
                data.MONANs.DeleteOnSubmit(ctv);
                data.SubmitChanges();

            }
           
            return RedirectToAction("Index", "CTVMonAn");
        }
       
        public ActionResult PV_KhuVuc(int? id)
        {
            List<KHUVUC> all = data.KHUVUCs.OrderBy(i => i.TenKhuVuc).ToList();
            if (id != null)
            {
                ViewBag.idLoai = id;
            }
            return PartialView(all);
        }
       
        public ActionResult PV_LoaiBuaAn(int? id)
        {
            List<LOAIBUAAN> all = data.LOAIBUAANs.OrderBy(i => i.TenLoai).ToList();
            if (id != null)
            {
                ViewBag.idLoai = id;
            }
            return PartialView(all);
        }

    }
}
