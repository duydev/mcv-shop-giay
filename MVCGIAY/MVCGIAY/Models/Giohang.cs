using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCGIAY.Models
{
    public class Giohang
    {
        dbQUANLYBANGIAYDataContext data = new dbQUANLYBANGIAYDataContext();
        public int iMaSP { set; get; }
        public string sTenSP { set; get; }
        public string sAnh { set; get; }
        public Double dDongia { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien {
            get { return iSoluong * dDongia; }
        }
        public Giohang(int MaSP)
        {
            iMaSP = MaSP;
            SANPHAM giay = data.SANPHAMs.Single(n => n.MaSP == iMaSP);
            sTenSP = giay.TenSP;
            sAnh = giay.Anh;
            dDongia = double.Parse(giay.DonGia.ToString());
            iSoluong = 1;
        }
    }
}