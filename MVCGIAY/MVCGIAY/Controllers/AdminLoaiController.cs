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
        private DBShopGiayEntities db = new DBShopGiayEntities();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoaiGiay()
        {
            List<CATEGORY> categories = db.CATEGORIES.OrderBy(a => a.ID).ToList();
            return View( categories );
        }

        [HttpGet]
        public ActionResult Themloai()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themloai(CATEGORY category)
        {
            try
            {
                db.CATEGORIES.Add(category);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }

            return RedirectToAction("LoaiGiay");
        }

        public ActionResult Chitietloai(int id)
        {
            //lay giay theo ma
            CATEGORY category = db.CATEGORIES.SingleOrDefault(a => a.ID == id);
            if (category == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(category);
        }

        [HttpGet]
        public ActionResult Xoaloai(int id)
        {
            CATEGORY category = db.CATEGORIES.SingleOrDefault(a => a.ID == id);
            if (category == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(category);
        }

        [HttpPost, ActionName("Xoaloai")]
        public ActionResult Xacnhanxoaloai(int id)
        {
            CATEGORY category = db.CATEGORIES.SingleOrDefault(a => a.ID == id);
            if (category == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            try
            {
                db.CATEGORIES.Remove(category);
                db.SaveChanges();
            }
            catch(Exception e)
            {
                throw e;
            }

            return RedirectToAction("LoaiGiay");
        }

        [HttpGet]
        public ActionResult Sualoaigiay(int id)
        {
            CATEGORY category = db.CATEGORIES.SingleOrDefault(a => a.ID == id);
            if (category == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(category);
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Sualoaigiay(CATEGORY category)
        {
            try
            {
                db.CATEGORIES.Attach(category);
                db.SaveChanges();
            }
            catch(Exception e)
            {
                throw e;
            }

            return RedirectToAction("LoaiGiay");
        }
    }
}