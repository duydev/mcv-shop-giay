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
    public class HoaDonController : Controller
    {
        // GET: HoaDon
        dbQUANLYBANGIAYDataContext data = new dbQUANLYBANGIAYDataContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult HoaDon()
        {
            return View(data.DONDATHANGs.ToList().OrderBy(n => n.MaDDH));
        }
        public ActionResult Chitiethoadon(int id)
        {
            //lay giay theo ma
            DONDATHANG hd = data.DONDATHANGs.SingleOrDefault(n => n.MaDDH == id);
            ViewBag.MaDDH = hd.MaDDH;
            if (hd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(hd);
        }
        [HttpGet]
        public ActionResult Xoahd(int id)
        {
            //lay sp muốn xoa theo ma
            DONDATHANG hd = data.DONDATHANGs.SingleOrDefault(n => n.MaDDH == id);
            ViewBag.MaDDH = hd.MaDDH;
            if (hd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(hd);
        }
        [HttpPost, ActionName("Xoahd")]
        public ActionResult Xacnhanxoahd(int id)
        {
            DONDATHANG hd = data.DONDATHANGs.SingleOrDefault(n => n.MaDDH == id);
            ViewBag.MaSP = hd.MaDDH;
            if (hd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.DONDATHANGs.DeleteOnSubmit(hd);
            data.SubmitChanges();
            return RedirectToAction("HoaDon");
        }
        [HttpGet]
        public ActionResult Suahd(int id)
        {
            DONDATHANG hd = data.DONDATHANGs.SingleOrDefault(n => n.MaDDH == id);
            ViewBag.MaDDH = hd.MaDDH;
            if (hd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaSP = new SelectList(data.SANPHAMs.ToList().OrderBy(n => n.MaSP), "MaSP", "TenSP");

            return View(hd);
        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Suahd(DONDATHANG hd)
        {
            ViewBag.MaSP = new SelectList(data.SANPHAMs.ToList().OrderBy(n => n.MaSP), "MaSP", "TenSP");

                    UpdateModel(hd);
                    data.SubmitChanges();
            return RedirectToAction("HoaDon");
        }
    }
}