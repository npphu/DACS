using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S_FOOD.Models
{
    public class DoanhThu
    {
        DBSfoodDataContext data = new DBSfoodDataContext();
      
        public double dThanhTien { set; get; }
        public int iIDDatHang { set; get; }       
        public float fGiaTriChiecKhau { set; get; }
        //khoi tao gio hang theo ma thuc don duoc truyen vao
        public DoanhThu(int ID_MonAn, CHIECKHAU ck)
        {
            iIDDatHang = ID_MonAn;
            DONHANG td = data.DONHANGs.SingleOrDefault(n => n.ID_DatHang == iIDDatHang);
           
            td.DATHANG.ThanhTien =Convert.ToDecimal(dThanhTien);
            ck.GiaTriCK = fGiaTriChiecKhau;
           
        }
    }
}