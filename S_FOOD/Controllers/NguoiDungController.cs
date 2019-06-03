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
    public class NguoiDungController : Controller
    {
        DBSfoodDataContext data = new DBSfoodDataContext();
        // GET: NguoiDung
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

        public ActionResult DangKy(FormCollection collection, NGUOIDUNG tk)
        {
            var tendn = collection["TenDangNhap"];
            var matkhau = collection["MatKhau"];
            var nhaplaimatkhau = collection["NhapLaiMatKhau"];
            var hoten = collection["HoTen"];
            bool gioitinh = Convert.ToBoolean(Convert.ToInt16(collection["GioiTinh"]));
            var dienthoai = collection["DienThoai"];
            var email = collection["Email"];
            var diachi = collection["DiaChi"];
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
            message.Subject = "[THÔNG TIN TÀI KHOẢN KHÁCH HÀNG]" + "-" + collection["HoTen"];
            message.Body = "Tên tài khoản:" + " " + collection["TenDangNhap"] + ", " + "Mật khẩu:" + " " + collection["MatKhau"];

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Index");
            }
            //-------------------------------------
            if (nhaplaimatkhau != matkhau)
            {
                ViewData["Loi1"] = "Mật khẩu không trùng khớp";
            }
            else
            {
                tk.TenTK_User = tendn;
                tk.MatKhau_User = matkhau;
                tk.HoTen = hoten;
                tk.GioiTinh = gioitinh;
                tk.DienThoai = dienthoai;
                tk.Email = email;
                tk.DiaChi = diachi;
                data.NGUOIDUNGs.InsertOnSubmit(tk);             
                data.SubmitChanges();
                return RedirectToAction("DangNhap");
            }

            return this.DangKy();
        }
        [HttpPost]
        public ActionResult Send(FormCollection form)
        {
            string user = "npphu97@gmail.com";
            string pass = "18021997";

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(user, pass),
                EnableSsl = true
            };

            string from = user;
            string to = form["Email"];
            MailMessage message = new MailMessage(from, to);
            message.Subject = form["HoTen"];
            message.Body = form["MatKhau"];

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Index");
            }

            return RedirectToAction("Index");
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

            NGUOIDUNG tk = this.data.NGUOIDUNGs.FirstOrDefault(n => n.TenTK_User == tendn && n.MatKhau_User == matkhau);
            if (tk != null)
            {
                ViewBag.ThongBao = "Đăng nhập thành công";
                Session["TenTK_User"] = tk;
                return RedirectToAction("Index", "Sfood");
            }
            else
                ViewBag.Thongbao = "Sai thông tin đăng nhập!";
            return View();
        }
        public ActionResult HienTenDN(int id)
        {
            NGUOIDUNG tk = (NGUOIDUNG)Session["TenTK_User"];
            return PartialView(tk);
        }

        public ActionResult TenDN()
        {
            return View();
        }

        public ActionResult TaiKhoan()
        {
            NGUOIDUNG tk = (NGUOIDUNG)Session["TenTK_User"];
            return PartialView(tk);
        }

        public ActionResult DangXuat()
        {
            //TaiKhoan tk = (TaiKhoan)Session["Username"];
            Session["TenTK_User"] = null;
            return RedirectToAction("DangNhap", "NguoiDung");
        }

        public ActionResult ThongTinTK()
        {

            if (Session["TenTK_User"] == null)
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            NGUOIDUNG tk = (NGUOIDUNG)Session["TenTK_User"];
            var tt = from m in data.NGUOIDUNGs where m.MaTK_User == tk.MaTK_User select m;
            return View(tt);            
        }
        public ActionResult Edit(int id)
        {
            if (Session["TenTK_User"] == null)
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            NGUOIDUNG ctv = data.NGUOIDUNGs.SingleOrDefault(i => i.MaTK_User == id);
            return View(ctv);
            
        }

        // POST: AdminCTV/Edit/5
        [HttpPost]
        public ActionResult Edit(NGUOIDUNG congtacvien, FormCollection collection)
        {
            string matkhaucu = collection["MatKhauCu"];
            NGUOIDUNG ctv = data.NGUOIDUNGs.SingleOrDefault(i => i.MaTK_User == congtacvien.MaTK_User);        
            if(matkhaucu == ctv.MatKhau_User)
            {
                ctv.MaTK_User = congtacvien.MaTK_User;
                ctv.HoTen = congtacvien.HoTen;
                ctv.Email = congtacvien.Email;
                ctv.DiaChi = congtacvien.DiaChi;
                ctv.DienThoai = congtacvien.DienThoai;
                ctv.NgaySinh = congtacvien.NgaySinh;
                ctv.GioiTinh = congtacvien.GioiTinh;

                UpdateModel(ctv);
                data.SubmitChanges();

                Session["TenTK_User"] = null;
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            else
            {
                ViewBag.Loi = "Mật khẩu không hợp lệ !";
            }

            return View();
                                   
        }

        public ActionResult DoiMatKhau(int id)
        {
            if (Session["TenTK_User"] == null)
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            NGUOIDUNG ctv = data.NGUOIDUNGs.SingleOrDefault(i => i.MaTK_User == id);
            return View(ctv);

        }

        // POST: AdminCTV/Edit/5
        [HttpPost]
        public ActionResult DoiMatKhau(NGUOIDUNG congtacvien, FormCollection collection)
        {
            string matkhaucu = collection["MatKhauCu"];
            string matkhaumoi = collection["MatKhauMoi"];
            string nhaplai = collection["NhapLai"];
            NGUOIDUNG ctv = data.NGUOIDUNGs.SingleOrDefault(i => i.MaTK_User == congtacvien.MaTK_User);
            if(matkhaumoi != nhaplai)
            {
                ViewBag.LoiMK = "Mật khẩu không trùng khớp!";
            }
            if (matkhaucu == ctv.MatKhau_User)
            {
                ctv.MatKhau_User = matkhaumoi;

                UpdateModel(ctv);
                data.SubmitChanges();

                Session["TenTK_User"] = null;
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            else
            {
                ViewBag.Loi = "Mật khẩu không hợp lệ !";
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
            var nd = this.data.NGUOIDUNGs.FirstOrDefault(m => m.Email == email);
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
                message.Subject = "[THÔNG TIN TÀI KHOẢN KHÁCH HÀNG]" + "-" + nd.HoTen;
                message.Body = "Tên tài khoản:" + " " + nd.TenTK_User + ", " + "Mật khẩu:" + " " + nd.MatKhau_User;

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

            return RedirectToAction("DangNhap","NguoiDung");
        }

    }
}