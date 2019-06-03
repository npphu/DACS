using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S_FOOD.Models;

namespace S_FOOD.Controllers
{
    public class AdminTaiKhoanController : Controller
    {
        DBSfoodDataContext data = new DBSfoodDataContext();
        // GET: AdminTaiKhoan
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]

        public ActionResult DangKy(FormCollection collection, Admin tk)
        {
            var tendn = collection["TenDangNhap"];
            var matkhau = collection["MatKhau"];
            var nhaplaimatkhau = collection["NhapLaiMatKhau"];
            
            if (nhaplaimatkhau != matkhau)
            {
                ViewData["Loi1"] = "Mật khẩu không trùng khớp";
            }
            else
            {
                tk.TenTK_Admin = tendn;
                tk.MatKhau_Admin = matkhau;                
                data.Admins.InsertOnSubmit(tk);
                data.SubmitChanges();
                return RedirectToAction("DangNhap");
            }
            return this.DangKy();
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var tendn = collection["TenDangNhap"];
            var matkhau = collection["MatKhau"];

            Admin tk = data.Admins.SingleOrDefault(n => n.TenTK_Admin == tendn && n.MatKhau_Admin == matkhau);
            if (tk != null)
            {
                ViewBag.ThongBao = "Đăng nhập thành công";
                Session["TenTK_Admin"] = tk;
                return RedirectToAction("Index", "Admin");
            }
            else
                ViewBag.Thongbao = "Sai thông tin đăng nhập!";
            return View();
        }

        public ActionResult HienTenDN(int id)
        {
            Admin tk = (Admin)Session["TenTK_Admin"];
            return PartialView(tk);
        }

        public ActionResult TenDN()
        {
            return View();
        }

        public ActionResult TaiKhoan()
        {
            Admin tk = (Admin)Session["TenTK_Admin"];
            return PartialView(tk);
        }

        public ActionResult DangXuat()
        {
            //TaiKhoan tk = (TaiKhoan)Session["Username"];
            Session["TenTK_User"] = null;
            return RedirectToAction("DangNhap", "NguoiDung");
        }

    }
}