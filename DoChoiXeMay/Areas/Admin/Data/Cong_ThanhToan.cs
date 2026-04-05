using DoChoiXeMay.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace DoChoiXeMay.Areas.Admin.Data
{
    public class Cong_ThanhToan
    {
        Model1 _context;
        static String TangCa = ConfigurationManager.AppSettings["TangCa"];
        static String Le = ConfigurationManager.AppSettings["Le"];
        static String luongcb = ConfigurationManager.AppSettings["MucLuongCoBan"];
        static String TCom = ConfigurationManager.AppSettings["TCom"];
        static String GiaoHang = ConfigurationManager.AppSettings["GiaoHang"];
        static String HoTro = ConfigurationManager.AppSettings["HoTro"];
        static String NgayCong = ConfigurationManager.AppSettings["NgayCong"];
        string DBname = ConfigurationManager.AppSettings["DBname"];
        public Cong_ThanhToan()
        {
            _context = new Model1();
        }
        public bool UpdateCongvaThanhToan_Auto(NV_GioCong model, int uid)
        {
            //Lay gio Cong
            double kqgc = 0; double kqtc = 0; double kqle = 0;
            var modelkt = _context.NV_GioCong.Where(kh => kh.IdNhanVien == model.IdNhanVien && kh.Month == model.Month
                                && kh.Year == model.Year && kh.Day <= DateTime.Now.Day).ToList();
            for (int i = 0; i < modelkt.Count(); i++)
            {
                var kqg = double.Parse((modelkt[i].GioRaSang - modelkt[i].GioVaoSang +
                    (modelkt[i].GioRaChieu - modelkt[i].GioVaoChieu)).TotalHours.ToString("0.00"));
                kqg = kqg > 0 ? kqg : 0;
                kqgc = kqgc + kqg;
                var kqt = double.Parse((modelkt[i].GioRaTangCa - modelkt[i].GioVaoTangCa).TotalHours.ToString("0.00"));
                kqt = kqt > 0 ? kqt : 0;
                kqtc = kqtc + kqt;
                var kql = double.Parse((modelkt[i].GioRaLe - modelkt[i].GioVaoLe).TotalHours.ToString("0.00"));
                kql = kql > 0 ? kql : 0;
                kqle = kqle + kql;
            }
            //kiểm tra bảng Công
            //Chưa có thì Insert, có rồi thì update
            var TinhCongPartimeAu = new Areas.Admin.Data.Cong_ThanhToan().TinhCongAutoPartime(model, kqgc, kqtc, kqle);
            if (TinhCongPartimeAu)
            {
                //kiểm tra bảng thanh toan luong
                //Chưa có thì Insert, có rồi thì update
                var dvt = _context.NV_NhanVienTek.Find(model.IdNhanVien).NV_Vitrinhanvien.DonViTinh;
                var updateThanhToan = new Areas.Admin.Data.Cong_ThanhToan().ThanhToanLuongAuto(model.IdNhanVien
                                                , model.Month, model.Year, dvt, kqgc, kqtc, kqle);
                if (updateThanhToan)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        public bool InsertCong(string DBname,int idnv,double snc, double sntc, double snle,int slcom,int slgiaohang,
            int slhotro, double sgcongthang, double sgtcathang, double sglethang, int thang, int nam)
        {
            try
            {
                DateTime date1 = DateTime.Now;
                string date = date1.ToString("yyyy-MM-dd HH:mm:ss");
                var snc1 = float.Parse(snc.ToString());
                var sntc1 = float.Parse(sntc.ToString());
                var snle1 = float.Parse(snle.ToString());
                var sgcongthang1 = float.Parse(sgcongthang.ToString());
                var sgtcathang1 = float.Parse(sgtcathang.ToString());
                var sglethang1 = float.Parse(sglethang.ToString());
                //var sgcongthang1 = float.Parse(sgcongthang.ToString());
                //var sgtcathang1 = float.Parse(sgtcathang.ToString());
                //var sglethang1 = float.Parse(sglethang.ToString());

                string sql = "insert into [" + DBname + "TechZone].[dbo].[NV_Cong] " +
                              "values(" + idnv + "," + snc1 + "," + sntc1 + ","+snle1+","+slcom+","+slgiaohang+"" +
                              ","+slhotro+",0,0,0,"+thang+","+nam+ "" +
                              ",convert(datetime, '" + date + "', 120),N'Insert Auto')";
                var insert_SVL = _context.Database.ExecuteSqlCommand(sql);
                return true;
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return false;
            }

        }
        public bool UPdateCong(NV_Cong model, string DBname)
        {
            try
            {
                var update = _context.Database.ExecuteSqlCommand("update [" + DBname + "TechZone].[dbo].[NV_Cong] set " +
                "IdNhanVien=@IdNhanVien,SoNgayCong=@SoNgayCong,SoNgayTangCa=@SoNgayTangCa," +
                "SoNgayLe=@SoNgayLe,SLCom=@SLCom,SLGiaoHang=@SLGiaoHang,SLHoTro=@SLHoTro,SoGioCongThang=@SoGioCongThang," +
                "SoGioTangCaThang=@SoGioTangCaThang,SoGioLeThang=@SoGioLeThang,Thang=@Thang,Nam=@Nam,NgayUpdate=@NgayUpdate," +
                "GiaiThich=@GiaiThich where Id=@Id",
                new SqlParameter("@IdNhanVien", model.IdNhanVien),
                new SqlParameter("@SoNgayCong", model.SoNgayCong),
                new SqlParameter("@SoNgayTangCa", model.SoNgayTangCa),
                new SqlParameter("@SoNgayLe", model.SoNgayLe),
                new SqlParameter("@SLCom", model.SLCom),
                new SqlParameter("@SLGiaoHang", model.SLGiaoHang),
                new SqlParameter("@SLHoTro", model.SLHoTro),
                new SqlParameter("@SoGioCongThang", model.SoGioCongThang),
                new SqlParameter("@SoGioTangCaThang", model.SoGioTangCaThang),
                new SqlParameter("@SoGioLeThang", model.SoGioLeThang),
                new SqlParameter("@Thang", model.Thang),
                new SqlParameter("@Nam", model.Nam),
                new SqlParameter("@NgayUpdate", DateTime.Now),
                new SqlParameter("@GiaiThich", model.GiaiThich),
                new SqlParameter("@Id", model.Id));
                if (update > 0)
                    return true;
                else return false;
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return false;
            }
        }
        public bool InsertThanhToanLuong(string DBname, Guid Id, int idnv, double tiencong, double tiencom, double pcgiaohang
            , double pcxangxe, double pcchucvu, double pckhac, double thuong, double khautrubh,
            double Ungluong,double thuclinh, int thang, int nam)
        {
            try
            {
                //Số tiền, không có dấu phẩy, không lỗi, không cần float
                DateTime date1 = DateTime.Now;
                string ngaytao = date1.ToString("yyyy-MM-dd HH:mm:ss");

                string sql = "insert into [" + DBname + "TechZone].[dbo].[NV_ThanhToanLuong] " +
                              "values('" + Id.ToString() + "'," + idnv + "," + tiencong + "," + tiencom + "," + pcgiaohang + "," + pcxangxe + "" +
                              "," + pcchucvu + "," + pckhac + "," + thuong + "," + khautrubh + "," + Ungluong + "," + thuclinh + "" +
                              ",0,"+thang+ ","+nam+",convert(datetime, '" + ngaytao + "', 120),convert(datetime, '" + ngaytao + "', 120))";
                var insert_SVL = _context.Database.ExecuteSqlCommand(sql);
                return true;
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return false;
            }

        }
        public bool UPdateThanhToanLuong(NV_ThanhToanLuong model, string DBname)
        {
            try
            {
                var update = _context.Database.ExecuteSqlCommand("update [" + DBname + "TechZone].[dbo].[NV_ThanhToanLuong] set " +
                "IdNhanVien=@IdNhanVien,TienCong=@TienCong,TienCom=@TienCom," +
                "PCGiaoHang=@PCGiaoHang,PCXangXe=@PCXangXe,PCChucVu=@PCChucVu,PCKhac=@PCKhac,Thuong=@Thuong," +
                "KhauTruBH=@KhauTruBH,DaUngLuong=@DaUngLuong,ThucLinh=@ThucLinh,DaNhanLuong=@DaNhanLuong," +
                "Thang=@Thang,Nam=@Nam,NgayTao=@NgayTao,NgayUpdate=@NgayUpdate where Id=@Id",
                new SqlParameter("@IdNhanVien", model.IdNhanVien),
                new SqlParameter("@TienCong", model.TienCong),
                new SqlParameter("@TienCom", model.TienCom),
                new SqlParameter("@PCGiaoHang", model.PCGiaoHang),
                new SqlParameter("@PCXangXe", model.PCXangXe),
                new SqlParameter("@PCChucVu", model.PCChucVu),
                new SqlParameter("@PCKhac", model.PCKhac),
                new SqlParameter("@Thuong", model.Thuong),
                new SqlParameter("@KhauTruBH", model.KhauTruBH),
                new SqlParameter("@DaUngLuong", model.DaUngLuong),
                new SqlParameter("@ThucLinh", model.ThucLinh),
                new SqlParameter("@DaNhanLuong", model.DaNhanLuong),
                new SqlParameter("@Thang", model.Thang),
                new SqlParameter("@Nam", model.Nam),
                new SqlParameter("@NgayTao", model.NgayTao),
                new SqlParameter("@NgayUpdate", DateTime.Now),
                new SqlParameter("@Id", model.Id));
                if (update > 0)
                    return true;
                else return false;
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return false;
            }
        }
        public bool InsertChiTietNangLuong(string DBname, int Idnv, int mucluong,int idhsl)
        {
            try
            {
                DateTime date1 = DateTime.Now;
                string ngaytao = date1.ToString("yyyy-MM-dd HH:mm:ss");
                string sql = "insert into [" + DBname + "TechZone].[dbo].[NV_ChiTietNangLuong] " +
                    "values(" + Idnv + "," + mucluong + "," + idhsl + ",convert(datetime, '" + ngaytao + "', 120)" +
                    ",1,N'Auto')";
                var insert_SVL = _context.Database.ExecuteSqlCommand(sql);
                return true;
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return false;
            }
        }
        public bool ThanhToanLuongAuto(int Idnv, int thang, int nam,string dvt, double giocong, double tangca, double gioLe)
        {
            try
            {
                //kiểm tra bảng thanh toan luong
                var ktbangthanhtoan = _context.NV_ThanhToanLuong.FirstOrDefault(kh => kh.Thang == thang && kh.Nam == nam
                                && kh.IdNhanVien == Idnv);
                var hsg = _context.NV_ChiTietNangLuong.FirstOrDefault(kh => kh.IdNhanVien == Idnv).NV_HeSoGio.HeSo;
                var nhanvien = _context.NV_NhanVienTek.Find(Idnv);
                if (ktbangthanhtoan == null)
                {
                    //insert 1 dòng
                    double tiencong = 0;
                    var ktCong1 = _context.NV_Cong.FirstOrDefault(kh => kh.Thang == thang && kh.Nam == nam
                                && kh.IdNhanVien == Idnv);
                    var id = Guid.NewGuid();
                    if (dvt == "Gio")//pratime
                    {
                        tiencong = (giocong + tangca * float.Parse(TangCa) + gioLe * float.Parse(Le)) * hsg;
                    }
                    else //chính thức ==>lấy mucluong tư bảng NV_ChiTietNangLuong
                    {
                        if (ktCong1 == null)
                        {
                            var mucluong = _context.NV_ChiTietNangLuong.FirstOrDefault(kh => kh.IdNhanVien == Idnv);
                            tiencong = mucluong.MucLuong;
                        }
                        else
                        {
                            tiencong = ktCong1.SoNgayCong * float.Parse(NgayCong);
                        }
                        
                    }
                    var pccv = nhanvien.NV_Vitrinhanvien.PhuCapChucVu;
                        var pck = nhanvien.NV_Vitrinhanvien.PhuCapChucKhac;
                        var thuclinh = tiencong + pccv + pck;
                        var iserpt = InsertThanhToanLuong(DBname, id, Idnv
                            , tiencong, 0, 0, 0, pccv, pck, 0, 0, 0, thuclinh, thang, nam);
                    if (iserpt == false) return false;
                }
                else
                {
                    //update b1: lấy bảng Công
                    double tiencong = 0;
                    var ktCong2 = _context.NV_Cong.FirstOrDefault(kh => kh.Thang == thang && kh.Nam == nam
                                && kh.IdNhanVien == Idnv);
                    if (dvt == "Gio")
                    {
                        tiencong = (giocong + tangca * float.Parse(TangCa) + gioLe * float.Parse(Le)) * hsg;
                    }
                    else
                    {
                        tiencong = ktCong2.SoNgayCong * float.Parse(NgayCong)
                                + ktCong2.SoNgayTangCa * float.Parse(NgayCong) * float.Parse(TangCa)
                                + ktCong2.SoNgayLe * float.Parse(NgayCong) * float.Parse(Le);
                    }
                    var pck = nhanvien.NV_Vitrinhanvien.PhuCapChucKhac;
                    var pccv = nhanvien.NV_Vitrinhanvien.PhuCapChucVu;
                    ktbangthanhtoan.TienCong = tiencong;
                    ktbangthanhtoan.TienCom = ktCong2.SLCom * float.Parse(TCom);
                    ktbangthanhtoan.PCGiaoHang = ktCong2.SLGiaoHang * float.Parse(GiaoHang);
                    ktbangthanhtoan.PCKhac = ktCong2.SLHoTro * float.Parse(HoTro) + pck;
                    if (ktbangthanhtoan.PCChucVu == 0)
                    {
                        ktbangthanhtoan.PCChucVu = pccv;
                    }
                    ktbangthanhtoan.ThucLinh = tiencong + ktbangthanhtoan.TienCom + ktbangthanhtoan.PCGiaoHang
                        + ktbangthanhtoan.PCXangXe + ktbangthanhtoan.PCChucVu + ktbangthanhtoan.PCKhac
                        + ktbangthanhtoan.Thuong - ktbangthanhtoan.KhauTruBH - ktbangthanhtoan.DaUngLuong;
                    var updateThanhToan = UPdateThanhToanLuong(ktbangthanhtoan, DBname);
                    if (updateThanhToan == false) return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return false;
            }
        }
        public bool TinhCongAutoPartime(NV_GioCong model, double giocong, double tangca, double gioLe)
        {
            var ktCong = _context.NV_Cong.FirstOrDefault(kh => kh.Thang == model.Month && kh.Nam == model.Year
                                    && kh.IdNhanVien == model.IdNhanVien);
            if (ktCong == null)
            {
                //insert 1 dòng
                var iserpt = InsertCong(DBname, model.IdNhanVien, 0, 0, 0, 0, 0, 0,
                    giocong, tangca, gioLe, model.Month, model.Year);
                if (iserpt == false)return false;
            }
            else
            {
                //update
                ktCong.SoGioCongThang = giocong;
                ktCong.SoGioTangCaThang = tangca;
                ktCong.SoGioLeThang = gioLe;
                var updateCong = new Data.Cong_ThanhToan().UPdateCong(ktCong, DBname);
                if (updateCong == false) return false;
            }
            return true;
        }
        public bool AddCongAutoChinhThuc()
        {
            var date = DateTime.Now;
            var listChinhThuc = _context.NV_NhanVienTek.Where(kh => kh.DaNghiViec == false && kh.NV_Vitrinhanvien.DonViTinh != "Gio").ToList();
            var ktfirt = _context.NV_Cong.Where(kh => kh.Thang == date.Month && kh.Nam == date.Year
                    && kh.NV_NhanVienTek.NV_Vitrinhanvien.DonViTinh != "Gio" && kh.NV_NhanVienTek.DaNghiViec ==false).ToList();
            if(listChinhThuc.Count() != ktfirt.Count())
            {
                foreach (NV_NhanVienTek item in listChinhThuc)
                {
                    var ktCong = _context.NV_Cong.FirstOrDefault(kh => kh.Thang == date.Month && kh.Nam == date.Year
                        && kh.IdNhanVien == item.Id);
                    if (ktCong == null)
                    {
                        var iserpt = InsertCong(DBname, item.Id, 0, 0, 0, 0, 0, 0, 0, 0, 0, date.Month, date.Year);
                        
                    }

                }
            }
            
            return true;
        }
        public double GetHSGbyIdNV(int IDnv)
        {
            double hsg = 0;
            var ChiTietNangLuong = _context.NV_ChiTietNangLuong.FirstOrDefault(kh=>kh.IdNhanVien==IDnv);
            if (ChiTietNangLuong != null) {
                hsg = ChiTietNangLuong.NV_HeSoGio.HeSo;
            }
            return hsg;
        } 
    }
}