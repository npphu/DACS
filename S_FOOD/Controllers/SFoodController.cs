using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S_FOOD.Models;
using PagedList;
using PagedList.Mvc;

namespace S_FOOD.Controllers
{
    public class SFoodController : Controller
    {
        // GET: SFood
        DBSfoodDataContext data = new DBSfoodDataContext();
        public List<MONAN> LayMonMoi(int count)
        {
            return data.MONANs.OrderByDescending(a => a.CONGTACVIEN.VIP.SoTienCanTra).Take(count).ToList();
           
        }
        public List<MONAN> LoadMonAn(int count)
        {
            return data.MONANs.Take(count).ToList();
        }
        public ActionResult Index(int? page)
        {
            int pageSize = 12;
            int pageNum = (page ?? 1);
            var monanmoi = LayMonMoi(100);
            return View(monanmoi.ToPagedList(pageNum, pageSize));
        }
        public ActionResult BuaAn()
        {
            var buaan = from ba in data.LOAIBUAANs select ba;
            return PartialView(buaan);
        }
        public ActionResult KhuVuc()
        {
            var khuvuc = from kv in data.KHUVUCs select kv;
            return PartialView(khuvuc);
        }
        public ActionResult MonAnTheoBuaAn(int id, int? page)
        {
            var monan = from m in data.MONANs where m.ID_Loai == id select m;
            int pageSize = 4;
            int pageNum = (page ?? 1);
            return View(monan.ToPagedList(pageNum, pageSize));
        }
        public ActionResult MonAnTheoKhuVuc(int id, int? page)
        {
            var monan = from m in data.MONANs where m.ID_KhuVuc == id select m;
            int pageSize = 4;
            int pageNum = (page ?? 1);
            return View(monan.ToPagedList(pageNum, pageSize));
        }
        public ActionResult ChiTiet(int id)
        {
            var monan = from m in data.MONANs where m.ID_MonAn == id select m;
            return View(monan);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}