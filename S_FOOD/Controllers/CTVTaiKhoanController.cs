using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S_FOOD.Models;

using System.Net;
using System.Net.Mail;

namespace S_FOOD.Controllers
{
    public class CTVTaiKhoanController : Controller
    {
        DBSfoodDataContext data = new DBSfoodDataContext();
        // GET: CTVTaiKhoan
        public ActionResult Index()
        {
            return View();
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
            CONGTACVIEN tk = this.data.CONGTACVIENs.FirstOrDefault(n => n.TenTK_CTV == tendn && n.MatKhau_CTV == matkhau);
            if (tk != null)
            {
                ViewBag.ThongBao = "Đăng nhập thành công";
                Session["TenTK_CTV"] = tk;
                return RedirectToAction("Index", "CTV");
            }
            else
                ViewBag.Thongbao = "Sai thông tin đăng nhập!";
            return View();
        }

        /*public ActionResult HienTenDN(int id)
        {
            CONGTACVIEN tk = (CONGTACVIEN)Session["TenTK_CTV"];
            return PartialView(tk);
        }*/

        public ActionResult TenDN()
        {
            return View();
        }

        public ActionResult TaiKhoan()
        {
            CONGTACVIEN tk = (CONGTACVIEN)Session["TenTK_CTV"];
            return PartialView(tk);
        }

        public ActionResult DangXuat()
        {
            //TaiKhoan tk = (TaiKhoan)Session["Username"];
            Session["TenTK_CTV"] = null;
            return RedirectToAction("DangNhap", "CTVTaiKhoan");
        }

        public ActionResult ThongTinTK()
        {

            if (Session["TenTK_CTV"] == null)
            {
                return RedirectToAction("DangNhap", "CTVTaiKhoan");
            }
            CONGTACVIEN tk = (CONGTACVIEN)Session["TenTK_CTV"];
            var tt = from m in data.CONGTACVIENs where m.MaTK_CTV == tk.MaTK_CTV select m;
            return View(tt);
        }

        public ActionResult Edit(int id)
        {
            if (Session["TenTK_CTV"] == null)
            {
                return RedirectToAction("DangNhap", "CTVTaiKhoan");
            }
            CONGTACVIEN ctv = data.CONGTACVIENs.SingleOrDefault(i => i.MaTK_CTV == id);
            return View(ctv);
        }
        [HttpPost]
        public ActionResult Edit(FormCollection collection, CONGTACVIEN nd)
        {
            string matkhaucu = collection["MatKhauCu"];
            string matkhaumoi = collection["MatKhauMoi"];
            string nhaplai = collection["NhapLaiMatKhauMoi"];
            CONGTACVIEN ctv = data.CONGTACVIENs.SingleOrDefault(i => i.MaTK_CTV == nd.MaTK_CTV);
            if(matkhaumoi != nhaplai)
            {
                ViewBag.LoiMK = "Mật khẩu không trùng khớp";
            }
            if(ctv.MatKhau_CTV == matkhaucu)
            {
                ctv.TenTK_CTV = nd.TenTK_CTV;
                ctv.MatKhau_CTV = matkhaumoi;
                UpdateModel(ctv);
                data.SubmitChanges();
                Session["TenTK_CTV"] = null;
                return RedirectToAction("DangNhap", "CTVTaiKhoan");
            }
            else
            {
                ViewBag.Loi = "Mật khẩu không hợp lệ";
            }
            return View();
        }

        [HttpGet]
        public ActionResult QuenMatKhau()
        {

            return View();
        }

        [HttpPost]
        public ActionResult QuenMatKhau(FormCollection collection)
        {


            var email = collection["Email"];
            var nd = this.data.CONGTACVIENs.FirstOrDefault(m => m.Email == email);
            if (nd == null)
            {
                ViewData["Loi1"] = "Email không tồn tại, vui lòng kiểm tra lại!";
            }
            else
            {
                //goi mail
                string user = "npphu97@gmail.com";
                string pass = "18021997";

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential(user, pass),
                    EnableSsl = true
                };

                string from = user;
                string to = collection["Email"];
                MailMessage message = new MailMessage(from, to);
                message.Subject = "[THÔNG TIN TÀI KHOẢN CỘNG TÁC VIÊN]" + "-" + nd.HoTen;
                message.Body = "Tên tài khoản:" + " " + nd.TenTK_CTV + ", " + "Mật khẩu:" + " " + nd.MatKhau_CTV;

                try
                {
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    return View("Index");
                }
                //---------------------------------------------------------------
                /* user.Password = matkhaumoi;
                 UpdateModel(user);
                 data.SubmitChanges();
                 return RedirectToAction("DangNhap", "NguoiDung");*/
            }

            return RedirectToAction("DangNhap", "CTVTaiKhoan");
        }
    }
}