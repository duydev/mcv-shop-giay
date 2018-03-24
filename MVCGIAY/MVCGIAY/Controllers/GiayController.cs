using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCGIAY.Models;

using PagedList;
using PagedList.Mvc;
namespace MVCGIAY.Controllers
{
    public class GiayController : Controller
    {
        // GET: Giay
        dbQUANLYBANGIAYDataContext data = new dbQUANLYBANGIAYDataContext();
        private List<SANPHAM> Laygiaymoi(int count)
        {
            return data.SANPHAMs.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }
        public ActionResult Index( int ? page)
        {
            //quy dinh so sp tren moi trang ne
            int pageSize = 6;
            int pageNum = (page ?? 1);
            //lay 6sp moi nhat
            var giaymoi = Laygiaymoi(6);
            return View(giaymoi);
        }
        public ActionResult LoaiSP()
        {
            var loaisp = from sp in data.LOAIs select sp;
            return PartialView(loaisp);
        }
        public ActionResult SPtheoloai(int id)
        {
            var giay = from g in data.SANPHAMs where g.MaLoai == id select g;
            return View(giay);
        }
        public ActionResult Details(int id)
        {
            var giay = from g in data.SANPHAMs where g.MaSP == id select g;
            return View(giay.Single());
        }
        // GET: Books

        //Tên biến phải giống với tên của SearchBox (không phân biệt chữ hoa chữ thường)
        public ActionResult Search(string searchTerm)
        {
            var giay = from g in data.SANPHAMs select g;

            if (!String.IsNullOrEmpty(searchTerm))
            {
                giay = data.SANPHAMs.Where(g => g.TenSP.Contains(searchTerm));
            }
            ViewBag.SearchTerm = searchTerm;
            return View(giay.ToList());
        }
        public ActionResult Gioithieu()
        {
            return View();
        }
        public ActionResult Lienhe()
        {
            return View();
        }
    }
}