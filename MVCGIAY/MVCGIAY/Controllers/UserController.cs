using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCGIAY.Models;
using System.Text;
using Facebook;
using System.Configuration;

namespace MVCGIAY.Controllers
{
    public class UserController : Controller
    {

        // GET: User
        dbQUANLYBANGIAYDataContext db = new dbQUANLYBANGIAYDataContext();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }
        //post: hàm này nhận dữ kiệu từ trang Dangky và tạo mới dữ liệu
        [HttpPost]
        public ActionResult Dangky(FormCollection collection, KHACHHANG kh)
        {
            var hoten = collection["HotenKH"];
            var gioitinh = collection["GioiTinh"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var matkhaunhaplai = collection["Matkhaunhaplai"];
            var diachi = collection["Diachi"];
            var dienthoai = collection["Dienthoai"];
            var email = collection["Email"];
            var ngaysinh = String.Format("{0://MM/dd/yyyy}", collection["Ngaysinh"]);
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi"] = "Họ tên khách hàng không được để trống";
            }
            else if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Phải nhập mật khẩu";
            }
            //else if (String.IsNullOrEmpty(matkhaunhaplai))
            //{
            //    ViewData["Loi4"] = "Phải nhập lại mật khẩu";
            //}
            else if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "Email không được để trống";
            }
            else if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi6"] = "Phải nhập số điện thoại";
            }
            else
            {
                kh.HoTen = hoten;
                if (gioitinh == "Nam")
                {
                    kh.GioiTinh = true;
                }
                else
                {
                    kh.GioiTinh = false;
                }
                kh.TaiKhoan = tendn;
                kh.MatKhau = matkhau;
                kh.Email = email;
                kh.DiaChi = diachi;
                kh.SDT = dienthoai;
                kh.NgaySinh = DateTime.Parse(ngaysinh);
                db.KHACHHANGs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("Dangnhap");
            }
            return this.Dangky();
        }
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
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
                    KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == tendn && n.MatKhau == matkhau);
                    if (kh != null)
                    {
                    //ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                    Session["Taikhoan"] = kh;
                    Session["Name"] = kh.HoTen;
                        return RedirectToAction("Index", "Giay");
                    }
                    else
                        ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            return View();
        }
        //Dangxuat
        public ActionResult Dangxuat()
        {
            Session["Taikhoan"] = null;
            return RedirectToAction("Index", "Giay");
        }
        //Login With Facebook
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppID"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            });
            return Redirect(loginUrl.AbsoluteUri);
        }
        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });
            var accessToken = result.access_token;
            Session["AccessToken"] = accessToken;
            fb.AccessToken = accessToken;
            if (!string.IsNullOrEmpty(accessToken))
            {
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string userName = me.email;
                string firstName = me.first_name;
                string middleName = me.middle_name;
                string lastName = me.last_name;
                if (me != null)
                {
                    Session["Taikhoan"] = me;
                    Session["Name"] = me.first_name + me.middle_name + " " + me.last_name; ;
                    return RedirectToAction("Index", "Giay");
                }
                else
                {
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng!!!";
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}