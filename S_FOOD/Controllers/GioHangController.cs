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
    public class GioHangController : Controller
    {
        // GET: GioHang
        DBSfoodDataContext data = new DBSfoodDataContext();
        // GET: GioHang
        public ActionResult Index()
        {
            return View();
        }

        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }

        public ActionResult ThemGioHang(int iIDMonAn, string strURL)
        {
            List<GioHang> lstGiohang = LayGioHang();
            GioHang sanpham = lstGiohang.Find(n => n.iIDMonAn == iIDMonAn);
            if (sanpham == null)
            {
                sanpham = new GioHang(iIDMonAn);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoLuong++;
                return Redirect(strURL);
            }
        }

        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                iTongSoLuong = lstGiohang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }

        public double TongTien()
        {
            double iTongTien = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                iTongTien = lstGiohang.Sum(n => n.dThanhTien);
            }
            return iTongTien;
        }

        public ActionResult GioHang()
        {
            //----------------
            List<GioHang> lstGiohang = LayGioHang();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "SFood");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }

        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }

        public ActionResult XoaGiohang(int iMaSP)
        {
            List<GioHang> lstGiohang = LayGioHang();
            GioHang sanpham = lstGiohang.SingleOrDefault(n => n.iIDMonAn == iMaSP);
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.iIDMonAn == iMaSP);
                return RedirectToAction("GioHang");
            }
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "SFood");
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapnhatGiohang(int iMaSP, FormCollection f)
        {
            List<GioHang> lstGiohang = LayGioHang();
            GioHang sanpham = lstGiohang.SingleOrDefault(n => n.iIDMonAn == iMaSP);
            if (sanpham != null)
            {
                sanpham.iSoLuong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult XoaTatcaGiohang()
        {
            List<GioHang> lstGiohang = LayGioHang();
            lstGiohang.Clear();
            return RedirectToAction("Index", "SFood");
        }
        [HttpGet]

        public ActionResult DatHang()
        {
            if (Session["TenTK_User"] == null)
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "SFood");
            }
            List<GioHang> lstGiohang = LayGioHang();           
            ViewBag.Tongsoluong = TongSoLuong();            
            ViewBag.Tongtien = TongTien();
           // ViewBag.IDDatHang = data.f_GetIdentDatHang();
            return View(lstGiohang);

        }

        public ActionResult DatHang(FormCollection collection)
         {
             DATHANG ddh = new DATHANG();
             NGUOIDUNG tk = (NGUOIDUNG)Session["TenTK_User"];
             List<GioHang> gh = LayGioHang();
             ddh.MaTK_User = tk.MaTK_User;
             ddh.ThoiGianDatHang = DateTime.Now;
             ddh.TinhTrangGiao = false;
            // var diachigiaohang = tk.DiaChi;
             ddh.DiaChiGiaoHang = tk.DiaChi;
            ddh.DaThanhToan = false;
             double iTongTien = 0;
             List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;           
            if (lstGiohang != null)
            {
                iTongTien = lstGiohang.Sum(n => n.dThanhTien);
            }
            ddh.ThanhTien = decimal.Parse(iTongTien.ToString());
            //goi mail
            string user = "npphu97@gmail.com";
            string pass = "18021997";

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(user, pass),
                EnableSsl = true
            };

            string from = user;
            string to = tk.Email;
            MailMessage message = new MailMessage(from, to);
            message.Subject = "[THÔNG TIN ĐƠN HÀNG]"+"Khách hàng:" + "-" + tk.HoTen;
          
            //-------------------------------------

            data.DATHANGs.InsertOnSubmit(ddh);
             data.SubmitChanges();
            ViewBag.IDDatHang = ddh.ID_DatHang;
            foreach (var item in gh)
             {
                 DONHANG ctdn = new DONHANG();
                 ctdn.ID_DatHang = ddh.ID_DatHang;
                 ctdn.ID_MonAn = item.iIDMonAn;                               
                 ctdn.SoLuong = item.iSoLuong;
                message.Body ="ĐẶT HÀNG THÀNH CÔNG"+" "+"Mã đơn hàng:" + " " + ddh.ID_DatHang + ", " + ",Tổng tiền:"+ ddh.ThanhTien +", Địa chỉ giao hàng:"+ tk.DiaChi ;
                data.DONHANGs.InsertOnSubmit(ctdn);
             }
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Index");
            }
            data.SubmitChanges();
         
             Session["GioHang"] = null;
             return RedirectToAction("Xacnhandonhang", "GioHang");
         }

         public ActionResult Xacnhandonhang()
         {
             return View();
         }
        public ActionResult LichSuMuaHang()
        {
            
            if (Session["TenTK_User"] == null)
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            NGUOIDUNG tk = (NGUOIDUNG)Session["TenTK_User"];
            var monan = from m in data.DONHANGs where m.DATHANG.MaTK_User == tk.MaTK_User select m;
            return View(monan);
        }
    }
}