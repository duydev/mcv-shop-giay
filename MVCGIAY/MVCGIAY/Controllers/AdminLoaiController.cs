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
    public class AdminLoaiController : Controller
    {
        // GET: AdminLoai
        dbQUANLYBANGIAYDataContext data = new dbQUANLYBANGIAYDataContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoaiGiay()
        {
            return View(data.LOAIs.ToList().OrderBy(n => n.MaLoai));
        }
        [HttpGet]
        public ActionResult Themloai()
        {
            ViewBag.MaNCC = new SelectList(data.LOAIs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themloai(LOAI loaigiay, HttpPostedFileBase fileupload)
        {
            ViewBag.MaLoai = new SelectList(data.LOAIs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            data.LOAIs.InsertOnSubmit(loaigiay);
            data.SubmitChanges();
            return RedirectToAction("LoaiGiay");
        }
        public ActionResult Chitietloai(int id)
        {
            //lay giay theo ma
            LOAI loai = data.LOAIs.SingleOrDefault(n => n.MaLoai == id);
            ViewBag.MaSP = loai.MaLoai;
            if (loai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(loai);
        }
        [HttpGet]
        public ActionResult Xoaloai(int id)
        {
            //lay sp muốn xoa theo ma
            LOAI loai = data.LOAIs.SingleOrDefault(n => n.MaLoai == id);
            ViewBag.MaLoai = loai.MaLoai;
            if (loai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(loai);
        }
        [HttpPost, ActionName("Xoaloai")]
        public ActionResult Xacnhanxoaloai(int id)
        {
            LOAI loai = data.LOAIs.SingleOrDefault(n => n.MaLoai == id);
            ViewBag.MaLoai = loai.MaLoai;
            if (loai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.LOAIs.DeleteOnSubmit(loai);
            data.SubmitChanges();
            return RedirectToAction("LoaiGiay");
        }
        //Sửa sản phẩm
        [HttpGet]
        public ActionResult Sualoaigiay(int id)
        {
            LOAI loai = data.LOAIs.SingleOrDefault(n => n.MaLoai == id);
            if (loai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(loai);
        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Sualoaigiay(LOAI loai)
        {
            UpdateModel(loai);
            data.SubmitChanges();
            return RedirectToAction("LoaiGiay");
        }
    }
}