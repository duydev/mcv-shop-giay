using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCGIAY.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
namespace MVCGIAY.Controllers
{
    public class AdminController : Controller
    {
        dbQUANLYBANGIAYDataContext data = new dbQUANLYBANGIAYDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login");
            }
            else
                return View();
        }
        public ActionResult Giay(int? page)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                int pageNumber = (page ?? 1);
                int pageSize = 4;
                return View(data.SANPHAMs.ToList().OrderBy(n => n.MaSP).ToPagedList(pageNumber, pageSize));
            }

        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection colletion)
        {
            var tendn = colletion["username"];
            var matkhau = colletion["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                ADMIN ad = data.ADMINs.SingleOrDefault(n => n.UserName == tendn && n.PassWord == matkhau);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    Session["Name"] = ad.UserName;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        [HttpGet]
        public ActionResult Themmoigiay()
        {
            ViewBag.MaLoai = new SelectList(data.LOAIs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaNCC = new SelectList(data.NHACCs.ToList().OrderBy(n => n.TenNNC), "MaNCC", "TenNNC");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoigiay(SANPHAM giay, HttpPostedFileBase fileupload)
        {
            ViewBag.MaLoai = new SelectList(data.LOAIs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaNCC = new SelectList(data.NHACCs.ToList().OrderBy(n => n.TenNNC), "MaNCC", "TenNNC");
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //them vao csdl
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/images/home"), fileName);
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    giay.Anh = fileName;
                    data.SANPHAMs.InsertOnSubmit(giay);
                    data.SubmitChanges();
                }
            }
            return RedirectToAction("Giay");
        }
        public ActionResult Chitietgiay(int id)
        {
            //lay giay theo ma
            SANPHAM giay = data.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = giay.MaSP;
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(giay);
        }
        [HttpGet]
        public ActionResult Xoagiay(int id)
        {
            //lay sp muốn xoa theo ma
            SANPHAM giay = data.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = giay.MaSP;
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(giay);
        }
        [HttpPost, ActionName("Xoagiay")]
        public ActionResult Xacnhanxoa(int id)
        {
            SANPHAM giay = data.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = giay.MaSP;
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.SANPHAMs.DeleteOnSubmit(giay);
            data.SubmitChanges();
            return RedirectToAction("Giay");
        }
        [HttpGet]
        public ActionResult Suagiay(int id)
        {
            SANPHAM giay = data.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = giay.MaSP;
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaLoai = new SelectList(data.LOAIs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai", giay.MaLoai);
            ViewBag.MaNCC = new SelectList(data.NHACCs.ToList().OrderBy(n => n.TenNNC), "MaNCC", "TenNCC", giay.MaNCC);
            return View(giay);
        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Suagiay(SANPHAM giay, HttpPostedFileBase fileupload)
        {
            ViewBag.MaLoai = new SelectList(data.LOAIs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaNCC = new SelectList(data.NHACCs.ToList().OrderBy(n => n.TenNNC), "MaNCC", "TenNCC");
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/images/home"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh này đã tồn tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    giay.Anh = fileName;
                    UpdateModel(giay);
                    data.SubmitChanges();
                }
            }
            return RedirectToAction("Giay");
        }
    }
}