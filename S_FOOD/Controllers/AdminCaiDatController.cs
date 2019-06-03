using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Mvc;
using S_FOOD.Models;

namespace S_FOOD.Controllers
{
    public class AdminCaiDatController : Controller
    {
        private DBSfoodDataContext data = new DBSfoodDataContext();
        // GET: AdminCaiDat
        public ActionResult Index(bool? result)
        {
            if (Session["TenTK_Admin"] == null)
            {
                return RedirectToAction("DangNhap", "AdminTaiKhoan");
            }
            List<CAIDAT> configs = data.CAIDATs.ToList();
            if (result != null)
            {
                ViewBag.Result = (bool)result ? "Cập nhật thành công." : null;
            }
            return View(configs);
        }

        [HttpPost]
        public ActionResult ApplyConfig(FormCollection form, HttpPostedFileBase[] fileUpload)
        {
            List<CAIDAT> configs = data.CAIDATs.ToList();
            string validateMsg = string.Empty;

            foreach (CAIDAT cf in configs)
            {
                switch (cf.TenThamSo)
                {
                    case "top_thuc_don_take":
                        string input = form["top_thuc_don_take"];
                        if (string.IsNullOrWhiteSpace(input) || !IsDigit(input))
                        {
                            ViewData["Error" + cf.TenThamSo] = "Giá trị không hợp lệ";
                            return View("Index", configs);
                        }
                        else
                        {
                            int iinput = Convert.ToInt32(input);
                            if (iinput < 1 || iinput > 10)
                            {
                                ViewData["Error" + cf.TenThamSo] = "Hãy nhập giá trị trong khoảng từ 1-10";
                                return View("Index", configs);
                            }
                        }
                        break;
                    case "top_tai_khoan_take":
                        input = form["top_tai_khoan_take"];
                        if (string.IsNullOrWhiteSpace(input) || !IsDigit(input))
                        {
                            ViewData["Error" + cf.TenThamSo] = "Giá trị không hợp lệ";
                            return View("Index", configs);
                        }
                        else
                        {
                            int iinput = Convert.ToInt32(input);
                            if (iinput < 1 || iinput > 10)
                            {
                                ViewData["Error" + cf.TenThamSo] = "Hãy nhập giá trị trong khoảng từ 1-10";
                                return View("Index", configs);
                            }
                        }
                        break;
                    case "hinh_logo":
                        if (fileUpload[0] != null)
                        {
                            string name = Path.GetFileName(fileUpload[0].FileName);
                            string ServerSavePath = Path.Combine(Server.MapPath("~/images/LogoBanner/") + name);

                            if (!System.IO.File.Exists(ServerSavePath))
                            {
                                fileUpload[0].SaveAs(ServerSavePath);
                            }
                            form[cf.TenThamSo] = name;
                        }
                        else
                        {
                            continue;
                        }
                        break;
                    case "hinh_banner_1":
                        if (fileUpload[1] != null)
                        {
                            string name = Path.GetFileName(fileUpload[1].FileName);
                            string ServerSavePath = Path.Combine(Server.MapPath("~/images/LogoBanner/") + name);

                            if (!System.IO.File.Exists(ServerSavePath))
                            {
                                fileUpload[1].SaveAs(ServerSavePath);
                            }
                            form[cf.TenThamSo] = name;
                        }
                        else
                        {
                            continue;
                        }
                        break;
                    case "hinh_banner_2":
                        if (fileUpload[2] != null)
                        {
                            string name = Path.GetFileName(fileUpload[2].FileName);
                            string ServerSavePath = Path.Combine(Server.MapPath("~/images/LogoBanner/") + name);

                            if (!System.IO.File.Exists(ServerSavePath))
                            {
                                fileUpload[2].SaveAs(ServerSavePath);
                            }
                            form[cf.TenThamSo] = name;
                        }
                        else
                        {
                            continue;
                        }
                        break;
                    case "hinh_banner_3":
                        if (fileUpload[3] != null)
                        {
                            string name = Path.GetFileName(fileUpload[3].FileName);
                            string ServerSavePath = Path.Combine(Server.MapPath("~/images/LogoBanner/") + name);

                            if (!System.IO.File.Exists(ServerSavePath))
                            {
                                fileUpload[3].SaveAs(ServerSavePath);
                            }
                            form[cf.TenThamSo] = name;
                        }
                        else
                        {
                            continue;
                        }
                        break;
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        string tenThamSo = cf.TenThamSo;
                        string giaTri = form[cf.TenThamSo];

                       data.p_UpdateCaiDat(tenThamSo, giaTri);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Error = ex.ToString();
                        View("Index", configs);
                    }
                }
            }

            return RedirectToAction("Index", new { result = true });
        }

        private bool IsDigit(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }
        [ChildActionOnly]
        public ActionResult PV_Header()
        {
            CAIDAT logo = data.CAIDATs.SingleOrDefault(i => i.TenThamSo.Equals("hinh_logo"));
            ViewBag.UrlLogo = Url.Content("~/images/LogoBanner/" + logo.GiaTri);

            string[] banners = { "hinh_banner_1", "hinh_banner_2", "hinh_banner_3" };
            foreach (string sbanner in banners)
            {
                CAIDAT banner = data.CAIDATs.SingleOrDefault(i => i.TenThamSo.Equals(sbanner));
                ViewData["Url_" + sbanner] = Url.Content("~/images/LogoBanner/" + banner.GiaTri);
            }

            return PartialView();
        }

    }
}
    