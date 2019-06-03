using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S_FOOD.Models
{
    public class GioHang
    {
        DBSfoodDataContext data = new DBSfoodDataContext();
        public string sAnh { set; get; }
        public double dGiaKhuyenMai { set; get; }
        public int iIDMonAn { set; get; }
        public string sTenMonAN { set; get; }
        public double dDonGia { set; get; }
        public int iSoLuong { set; get; }
        public decimal dPhiVanChuyen { set; get; }
        public double dThanhTien
        {
            get { return ((dDonGia-dDonGia*(dGiaKhuyenMai/100)) * iSoLuong)+ Convert.ToDouble(dPhiVanChuyen); }
           
        }
        //khoi tao gio hang theo ma thuc don duoc truyen vao
        public GioHang(int ID_MonAn)
        {
            iIDMonAn = ID_MonAn;     
            MONAN td = data.MONANs.SingleOrDefault(n => n.ID_MonAn == iIDMonAn);     
            if (td.KhuyenMai != null)
            {
                dGiaKhuyenMai = Double.Parse(td.KhuyenMai.GiamGia.ToString());
            }
            else
            dGiaKhuyenMai = 0;       
            sTenMonAN = td.TenMonAn;
            sAnh = td.AnhBia;
            dDonGia = double.Parse(td.GiaBan.ToString());
            dPhiVanChuyen = decimal.Parse(td.PhiVanChuyen.ToString());
            iSoLuong = 1;   
        }
    }
}