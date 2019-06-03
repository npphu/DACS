using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S_FOOD.Models;

namespace S_FOOD.Controllers
{
    public class DoanhThuController : Controller
    {
        DBSfoodDataContext data = new DBSfoodDataContext();       
        // GET: DoanhThu       
        public ActionResult Index()
        {            
            if (Session["TenTK_CTV"] == null)
            {
                return RedirectToAction("DangNhap", "CTVTaiKhoan");
            }
            CONGTACVIEN tk = (CONGTACVIEN)Session["TenTK_CTV"];
            var sum = from m in data.DOANHTHUs where m.MaTK_CTV == tk.MaTK_CTV select m;
            return View(sum);
        }

        public ActionResult DoanhThuAll()
        {
            if (Session["TenTK_Admin"] == null)
            {
                return RedirectToAction("DangNhap", "AdminTaiKhoan");
            }
            List<DOANHTHU> all = data.DOANHTHUs.ToList();
            return View(all);
        }

        public ActionResult Tinh()
        {
            if (Session["TenTK_CTV"] == null)
            {
                return RedirectToAction("DangNhap", "CTVTaiKhoan");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Tinh(FormCollection collection, DOANHTHU dt)
        {

            var gt = collection["ID_ChiecKhau"];
            ViewBag.MessageFail = string.Empty;          
            if (!string.IsNullOrEmpty(ViewBag.MessageFail))
            {
                return View();

            }
            CONGTACVIEN tk = (CONGTACVIEN)Session["TenTK_CTV"];
            decimal sum = 0;  
            dt.MaTK_CTV = tk.MaTK_CTV;
            sum = Convert.ToDecimal(data.f_tongdt(tk.MaTK_CTV));
            dt.TongDoanhThu = sum;
            decimal tck = 0;
            tck = sum * Decimal.Parse(gt);
            dt.TienChiecKhau = tck;
           

            if (ModelState.IsValid)
            {
                data.DOANHTHUs.InsertOnSubmit(dt);
                try
                {
                    dt.Thang = DateTime.Now;
                    data.SubmitChanges();
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Violation of PRIMARY KEY constraint"))
                    {
                        ViewBag.MessageFail = string.Format("Doanh thu tháng {0} đã tồn tại, nếu muốn thay đổi, Vui lòng CẬP NHẬT.", dt.Thang);
                    }
                    else
                    {
                        ViewBag.ErrorBody = ex.ToString();
                    }
                    return View();
                }
            }

            ViewBag.MessageSuccess = "Tính doanh thu tháng : [" + dt.Thang + "] thành công";
            return View();

        }

        // GET: DoanhThu/Details/5
        
        // GET: DoanhThu/Edit/5
        public ActionResult Edit(int id)
        {
            DOANHTHU kv = data.DOANHTHUs.SingleOrDefault(i => i.MaTK_CTV == id);
            return View(kv);
        
        }



        // POST: DoanhThu/Edit/5
        [HttpPost]
        public ActionResult Edit(DOANHTHU dt, FormCollection collection)
        {
        
            DOANHTHU doanhthu = data.DOANHTHUs.SingleOrDefault(i => i.MaTK_CTV == dt.MaTK_CTV);
            var gt = collection["ID_ChiecKhau"];
            dt.TienChiecKhau = doanhthu.TongDoanhThu * Convert.ToDecimal(gt);
            doanhthu.TienChiecKhau = dt.TienChiecKhau;
            doanhthu.Thang = DateTime.Now;
            if (ModelState.IsValid)
            {

                UpdateModel(doanhthu);
                data.SubmitChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: DoanhThu/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DoanhThu/Delete/5
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

        public ActionResult AdminEdit(int id)
        {
            DOANHTHU kv = data.DOANHTHUs.SingleOrDefault(i => i.MaTK_CTV == id);
            return View(kv);

        }

        // POST: DoanhThu/Edit/5
        [HttpPost]
        public ActionResult AdminEdit(DOANHTHU dt, FormCollection collection)
        {
            DOANHTHU doanhthu = data.DOANHTHUs.SingleOrDefault(i => i.MaTK_CTV == dt.MaTK_CTV);
            var gt = collection["ID_ChiecKhau"];
            dt.TienChiecKhau = doanhthu.TongDoanhThu * Convert.ToDecimal(gt);
            doanhthu.TienChiecKhau = dt.TienChiecKhau;
            doanhthu.Thang = DateTime.Now;
            if (ModelState.IsValid)
            {

                UpdateModel(doanhthu);
                data.SubmitChanges();
            }
            return RedirectToAction("DoanhThuAll");
        }

        public ActionResult GiaTriCK(double ? id)
        {
            List<CHIECKHAU> all = data.CHIECKHAUs.ToList();
            if (id != null)
            {
                ViewBag.idLoai = id;
            }
            return PartialView(all);
        }
    }
}
