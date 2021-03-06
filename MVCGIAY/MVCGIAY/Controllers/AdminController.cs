﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCGIAY.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
using MVCGIAY.Commons;

namespace MVCGIAY.Controllers
{
    public class AdminController : Controller
    {
        dbQUANLYBANGIAYDataContext data = new dbQUANLYBANGIAYDataContext();

        // DB Oracle
        private DBShopGiayEntities db = new DBShopGiayEntities();

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

                IPagedList<PRODUCT> products = db.PRODUCTS.OrderBy(a => a.ID).ToPagedList(pageNumber, pageSize);

                return View(products);
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
            string tendn = ((string)colletion["username"]).Trim();
            string matkhau = ((string)colletion["password"]).Trim();

            List<string> errors = new List<string>();

            // Validating
            if (String.IsNullOrEmpty(tendn))
            {
                errors.Add("Phải nhập tên đăng nhập");
            }

            if (String.IsNullOrEmpty(matkhau))
            {
                errors.Add("Phải nhập mật khẩu");
            }

            if( errors.Count == 0 )
            {
                ADMIN admin = db.ADMINS.FirstOrDefault( a => a.USERNAME == tendn );
                if (admin != null)
                {
                    string currPassHash = Helper.md5(matkhau);
                    if (admin.PASSWORD.CompareTo(currPassHash) == 0)
                    {
                        Session["Taikhoanadmin"] = admin;
                        Session["Name"] = admin.USERNAME;
                        return RedirectToAction("Index", "Admin");
                    }
                }

                // case wrong username or password
                errors.Add("Tên đăng nhập hoặc mật khẩu không đúng.");

            }

            ViewBag.errors = errors;
            return View();
        }

        [HttpGet]
        public ActionResult Themmoigiay()
        {
            List<CATEGORY> categories = db.CATEGORIES.OrderBy(a => a.NAME).ToList();
            List<SUPPLIER> suppliers = db.SUPPLIERS.OrderBy(a => a.NAME).ToList();

            ViewBag.CATEGORY_ID = new SelectList(categories, "ID", "NAME");
            ViewBag.SUPPLIER_ID = new SelectList(suppliers, "ID", "NAME");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoigiay(PRODUCT product, HttpPostedFileBase fileupload)
        {
            List<CATEGORY> categories = db.CATEGORIES.OrderBy(a => a.NAME).ToList();
            List<SUPPLIER> suppliers = db.SUPPLIERS.OrderBy(a => a.NAME).ToList();

            ViewBag.CATEGORY_ID = new SelectList(categories, "ID", "NAME");
            ViewBag.SUPPLIER_ID = new SelectList(suppliers, "ID", "NAME");

            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
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

                    product.IMAGE = fileName;

                    try
                    {
                        db.PRODUCTS.Add(product);
                        db.SaveChanges();
                    }
                    catch(Exception e)
                    {
                        throw e;
                    }
                }
            }
            return RedirectToAction("Giay");
        }

        public ActionResult Chitietgiay(int id)
        {
            PRODUCT product = db.PRODUCTS.SingleOrDefault(a => a.ID == id);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(product);
        }

        [HttpGet]
        public ActionResult Xoagiay(int id)
        {
            PRODUCT product = db.PRODUCTS.SingleOrDefault(a => a.ID == id);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(product);
        }

        [HttpPost, ActionName("Xoagiay")]
        public ActionResult Xacnhanxoa(int id)
        {
            PRODUCT product = db.PRODUCTS.SingleOrDefault(a => a.ID == id);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            try
            {
                db.PRODUCTS.Remove(product);
                db.SaveChanges();
            }
            catch(Exception e)
            {
                throw e;
            }

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
            //ViewBag.MaLoai = new SelectList(data.LOAIs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai", giay.MaLoai);
            //ViewBag.MaNCC = new SelectList(data.NHACCs.ToList().OrderBy(n => n.TenNNC), "MaNCC", "TenNCC", giay.MaNCC);
            return View(giay);
        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Suagiay(SANPHAM giay, HttpPostedFileBase fileupload)
        {
            //ViewBag.MaLoai = new SelectList(data.LOAIs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            //ViewBag.MaNCC = new SelectList(data.NHACCs.ToList().OrderBy(n => n.TenNNC), "MaNCC", "TenNCC");
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