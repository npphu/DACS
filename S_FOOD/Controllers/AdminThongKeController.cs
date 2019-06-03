using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S_FOOD.Models;

namespace S_FOOD.Controllers
{
    public class AdminThongKeController : Controller
    {
        DBSfoodDataContext data = new DBSfoodDataContext();
        public List<DOANHTHU> LayTop3(int count)
        {
            return data.DOANHTHUs.OrderByDescending(a => a.TongDoanhThu).Take(count).ToList();

        }
        public ActionResult Index()
        {

            if (Session["TenTK_Admin"] == null)
            {
                return RedirectToAction("DangNhap", "AdminTaiKhoan");
            }
            var monanmoi = LayTop3(3);
            return View(monanmoi.ToList());
        }
    }
}