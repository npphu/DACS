using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S_FOOD.Models;
using PagedList.Mvc;
using PagedList;

namespace S_FOOD.Controllers
{
    public class TimKiemController : Controller
    {
        // GET: TimKiem
        private DBSfoodDataContext data = new DBSfoodDataContext();
        [HttpPost]
        public ActionResult KetQuaTimKiem(int? page, FormCollection f)
        {
            if (string.IsNullOrWhiteSpace(f["txtTimkiem"]))
            {
                return RedirectToAction("Index", "Home");
            }
            string tukhoa = f["txtTimkiem"].ToString();
            ViewBag.TK = tukhoa;
            List<MONAN> lsKQTK = data.MONANs.Where(n => n.TenMonAn.Contains(tukhoa)).ToList();
            int pageSize = 8;
            int pageNum = (page ?? 1);
            if (lsKQTK.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy";
                return View(data.MONANs.OrderBy(n => n.TenMonAn).ToPagedList(pageNum, pageSize));
            }
            ViewBag.ThongBao = "Đã tìm thấy" + " " + lsKQTK.Count + " " + "kết quả";
            return View(lsKQTK.OrderBy(n => n.TenMonAn).ToPagedList(pageNum, pageSize));
        }
        [HttpGet]
        public ActionResult KetQuaTimKiem()
        {
            return RedirectToAction("Index", "Sfood");
        }
    }
}