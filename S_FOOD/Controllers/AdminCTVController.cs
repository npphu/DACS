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
    public class AdminCTVController : Controller
    {
        DBSfoodDataContext data = new DBSfoodDataContext();
        // GET: AdminCTV
        public ActionResult Index()
        {
            if (Session["TenTK_Admin"] == null)
            {
                return RedirectToAction("DangNhap", "AdminTaiKhoan");
            }
            List<CONGTACVIEN> all = data.CONGTACVIENs.ToList();
            return View(all);
        }

        // GET: AdminCTV/Details/5
        public ActionResult Details(int id)
        {
            if (Session["TenTK_Admin"] == null)
            {
                return RedirectToAction("DangNhap", "AdminTaiKhoan");
            }
            CONGTACVIEN ctv = data.CONGTACVIENs.SingleOrDefault(i => i.MaTK_CTV == id);
            return View(ctv);
           
        }

        // GET: AdminCTV/Create
        public ActionResult Create()
        {
            if (Session["TenTK_Admin"] == null)
            {
                return RedirectToAction("DangNhap", "AdminTaiKhoan");
            }
            return View();
        }

        // POST: AdminCTV/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, CONGTACVIEN tk)
        {
            var tendn = collection["TenDangNhap"];
            var matkhau = collection["MatKhau"];
            var nhaplaimatkhau = collection["NhapLaiMatKhau"];
            var hoten = collection["HoTen"];
            var cmnd = collection["CMND"];
            var email = collection["Email"];
            var diachi = collection["DiaChi"];
            var dienthoai = collection["DienThoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["NgaySinh"]);
            bool gioitinh = Convert.ToBoolean(Convert.ToInt16(collection["GioiTinh"]));
            var giaquangcao = Convert.ToInt32( collection["GiaQuangCao"]);
            var ngayhethanvip = String.Format("{0:MM/dd/yyyy}",collection["NgayHetHan"]);
            //Goi mail
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
            message.Subject = "[THÔNG TIN TÀI KHOẢN CỘNG TÁC VIÊN]" + "-" + collection["HoTen"];
            message.Body = "Tên tài khoản:" + " " + collection["TenDangNhap"] + " " + ", Mật khẩu:" + " " + collection["MatKhau"];

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Index");
            }
            //-----------------------------------
            ViewBag.MessageFail = string.Empty;
            if (nhaplaimatkhau != matkhau)
            {
                ViewBag.MessageFail += "Mật khẩu không trùng khớp";
            }
             if (DateTime.Now.Year - DateTime.Parse(ngaysinh).Year < 18)
            {

                ViewBag.MessageFail += "Cộng tác viên phải đủ 18 tuổi";

            }
             if(!string.IsNullOrEmpty(ViewBag.MessageFail))
            {
                return View();
            }
            else
            {
                tk.TenTK_CTV = tendn;
                tk.MatKhau_CTV = matkhau;
                tk.HoTen = hoten;
                tk.CMND = cmnd;
                tk.Email = email;
                tk.DiaChi = diachi;
                tk.DienThoai = dienthoai;
                tk.NgaySinh =DateTime.Parse(ngaysinh);
                tk.GioiTinh = gioitinh;
                tk.LevelVip = giaquangcao;
                tk.NgayHetHanVip = DateTime.Parse( ngayhethanvip);                                              
                data.CONGTACVIENs.InsertOnSubmit(tk);
                data.SubmitChanges();            
            }
            ViewBag.MessageSuccess = "Thêm Cộng tác viên: [" + hoten + "] thành công";
            return View();

        }

        // GET: AdminCTV/Edit/5
        public ActionResult Edit(int id)
        {
            if (Session["TenTK_Admin"] == null)
            {
                return RedirectToAction("DangNhap", "AdminTaiKhoan");
            }
            CONGTACVIEN ctv = data.CONGTACVIENs.SingleOrDefault(i => i.MaTK_CTV == id);
            return View(ctv);
        }

        // POST: AdminCTV/Edit/5
        [HttpPost]
        public ActionResult Edit(CONGTACVIEN congtacvien, FormCollection collection)
        {
            CONGTACVIEN ctv = data.CONGTACVIENs.SingleOrDefault(i => i.MaTK_CTV == congtacvien.MaTK_CTV);
            var giaquangcao = Convert.ToInt32(collection["GiaQuangCao"]);
            ctv.MaTK_CTV = congtacvien.MaTK_CTV;
            ctv.TenTK_CTV = congtacvien.TenTK_CTV;
            ctv.MatKhau_CTV = congtacvien.MatKhau_CTV;
            ctv.HoTen = congtacvien.HoTen;
            ctv.CMND = congtacvien.CMND;
            ctv.Email = congtacvien.Email;
            ctv.DiaChi = congtacvien.DiaChi;
            ctv.DienThoai = congtacvien.DienThoai;
            ctv.NgaySinh = congtacvien.NgaySinh;
            ctv.GioiTinh = congtacvien.GioiTinh;
            ctv.LevelVip = giaquangcao;
            ctv.NgayHetHanVip = congtacvien.NgayHetHanVip;
            if (ModelState.IsValid)
            {
            
                UpdateModel(ctv);
                data.SubmitChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: AdminCTV/Delete/5
        public ActionResult Delete(int id)
        {
            if (Session["TenTK_Admin"] == null)
            {
                return RedirectToAction("DangNhap", "AdminTaiKhoan");
            }
            CONGTACVIEN ctv = data.CONGTACVIENs.SingleOrDefault(n => n.MaTK_CTV == id);
            ViewBag.Masach = ctv.MaTK_CTV;
            if (ctv == null)
            {

                return RedirectToAction("Index", "AdminCTV");
            }
            return View(ctv);           
        }
        // POST: AdminCTV/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {         
            CONGTACVIEN ctv = data.CONGTACVIENs.SingleOrDefault(i => i.MaTK_CTV == id);            
            int countMonAn = data.MONANs.Count(i => i.MaTK_CTV == id);
            
            if (countMonAn > 0 )
            {

                ViewBag.ErrorBody = string.Format("Không thể Xóa  do [{0}] đang có [{1}] Món Ăn trên Web.", ctv.HoTen, countMonAn);
                List<CONGTACVIEN> all = data.CONGTACVIENs.ToList();       
                return View("Index", all.ToList());

            }
            else
             if (ModelState.IsValid)
            {
                data.CONGTACVIENs.DeleteOnSubmit(ctv);
                data.SubmitChanges();
                
            }
                return RedirectToAction("Index","AdminCTV");
        }
        [ChildActionOnly]
        public ActionResult PV_Dropdown_TienVip(int? id)
        {
            List<VIP> all = data.VIPs.OrderBy(i => i.SoTienCanTra).ToList();
            if (id != null)
            {
                ViewBag.idLoai = id;
            }
            return PartialView(all);
        }
    }
}
