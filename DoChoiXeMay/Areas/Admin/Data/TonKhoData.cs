using DoChoiXeMay.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Windows.Media.Media3D;
using System.Xml.Linq;
using static QRCoder.PayloadGenerator;

namespace DoChoiXeMay.Areas.Admin.Data
{
    public class TonKhoData
    {
        Model1 _context = new Model1();
        //public List<ChitietXuatNhap> GetSumToTKDaBan(string name)
        //{
        //    var daban = _context.ChitietXuatNhaps.Where(kh => kh.Ten == name
        //         && kh.KyXuatNhap.XuatNhap == true && kh.IdDoiTra == 1 && kh.KyXuatNhap.IdLoaiHangXN == 1).ToList();
        //    return daban;
        //}
        //public List<ChitietXuatNhap> GetSumToTKDaBanLeSi(string name, int San, bool khachLe,int IDColor)
        //{
        //    var daban = _context.ChitietXuatNhaps.Where(kh => kh.Ten == name
        //         && kh.KyXuatNhap.XuatNhap == true && kh.IdDoiTra == 1 && kh.KyXuatNhap.IdLoaiHangXN == 1).ToList();
        //    var LeSi = daban.Where(kh => kh.KyXuatNhap.IdSan == San && kh.KyXuatNhap.KhachLe == khachLe).ToList();
        //    return LeSi;
        //}
        public List<ChiTietTonKho> GetTonKhobyHH(string name, int IDColor, int IDMF, int IDSize)
        {
            var model = _context.ChiTietTonKhoes.Where(kh=>kh.TenHang == name && kh.SanPham==false
                    && kh.IDMF == IDMF && kh.IDColor == IDColor && kh.IDSize == IDSize && kh.KyTonKho.SuDung==true).ToList();

            return model;
        }
        public List<ChiTietTonKho> GetListTKhoByKy(int idKytonkho)
        {
            List<ChiTietTonKho> modelCuoi = new List<ChiTietTonKho>();
            var modelCha = _context.ChiTietTonKhoes.Where(kh => kh.IdKyTonKho == idKytonkho && kh.ParentId==0)
                    .OrderByDescending(kh => kh.Id)
                    .ToList();
            var modelCon = _context.ChiTietTonKhoes.Where(kh => kh.IdKyTonKho == idKytonkho && kh.ParentId >0)
                    .OrderByDescending(kh => kh.Id)
                    .ToList();

            for (int i = 0; i < modelCha.Count(); i++)
            {
                modelCuoi.Add(modelCha[i]);
                for (int j = 0; j < modelCon.Count(); j++)
                {
                    if (modelCon[j].ParentId == modelCha[i].Id)
                    {
                        modelCuoi.Add(modelCon[j]);
                    }
                }
            }
            return modelCuoi;
        }
        public List<ChiTietTonKho> GetListTKhoBySP(int ParenId)
        {
            if (ParenId > 0)
            {
                var model = _context.ChiTietTonKhoes.Where(kh => kh.ParentId == ParenId)
                    .OrderByDescending(kh => kh.Id)
                    .ToList();
                for (int i = 0; i < model.Count(); i++)
                {
                    model[i].STT = (i + 1).ToString();
                }
                return model;
            }
            return null;
        }
        public bool InsertTonKhoAotu(int IdKy, string ten, int parId, string DBname)
        {
            try
            {

                string sql = "insert into [" + DBname + "TechZone].[dbo].[ChiTietTonKho] " +
                                            "values(" + IdKy + ",N'"+ten+"',0,0,0,0,GETDATE(),GETDATE(),0,'','',"+parId+",5,1,1,1)";
                var insert_SVL = _context.Database.ExecuteSqlCommand(sql);
                return true;
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return false;
            }

        }
        public static bool UpdateCTKytonKho(Model1 db, int IdKyton, string tenhang,int IDMF, int Color, int Size, int soluong)
        {
            //dùng cho kỳ nhập
            try
            {
                var modelhh = db.ChiTietTonKhoes.FirstOrDefault(kh => kh.TenHang.ToLower().Trim() == tenhang.ToLower().Trim() && kh.IDMF == IDMF
                                                && kh.IDColor == Color && kh.IDSize == Size && kh.IdKyTonKho==IdKyton);
                if (modelhh != null)
                {
                    var model = db.ChiTietTonKhoes.Find(modelhh.Id);
                    model.DaRap = model.DaRap + soluong;
                    model.ChuaRap = model.TonDauKy-model.DaRap-model.CoLoi;
                    model.NgayUpdate = DateTime.Now;
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                return true;
            }
            catch (Exception e)
            {
                string msg = e.Message;
                return false;
            }
        }
        //07/03/2026 thêm idkho
        public bool InsertChiTietSLHangHoa(int Idhh,int sl,int idkho,string ghichu, string DBname)
        {
            try
            {
                var Id = Guid.NewGuid();
                string sql = "insert into [" + DBname + "TechZone].[dbo].[ChiTietSLHangHoa] " +
                                            "values(N'" + Id.ToString() + "',"+Idhh+ "," + idkho + "," +sl+ ",GETDATE(),N'" + ghichu + "')";
                var insert_SVL = _context.Database.ExecuteSqlCommand(sql);
                return true;
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return false;
            }

        }
        public bool UPdateChiTietSLHangHoa(ChiTietSLHangHoa model, string DBname)
        {
            try
            {
                var update = _context.Database.ExecuteSqlCommand("update [" + DBname + "TechZone].[dbo].[ChiTietSLHangHoa] set " +
                "IdHangHoa=@IdHangHoa,IdKho=@IdKho,SoLuong=@SoLuong,NgayAuto=@NgayAuto,GhiChu=@GhiChu " +
                "where Id=@Id",
                new SqlParameter("@IdHangHoa", model.IdHangHoa),
                new SqlParameter("@IdKho", model.IdKho),
                new SqlParameter("@SoLuong", model.SoLuong),
                new SqlParameter("@NgayAuto", model.NgayAuto),
                new SqlParameter("@GhiChu", model.GhiChu),
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
        public bool AutoChiTietSLHangHoa(int idhh,int soluong, string DBname)
        {
            try
            {
                var modelktSPTeK = _context.HangHoas.FirstOrDefault(kh => kh.IDMF == 5 && kh.IDKy == 0 && kh.Id == idhh);
                if (modelktSPTeK != null)
                {
                    var listct = _context.ChiTietSLHangHoas.FirstOrDefault(kh => kh.IdHangHoa == idhh && kh.IdKho == modelktSPTeK.IdKho);
                    if (listct == null)
                    {
                        var kqcthh = new TonKhoData().InsertChiTietSLHangHoa(idhh, soluong, modelktSPTeK.IdKho, "Auto", DBname);
                    }
                    else if (listct != null && listct.SoLuong != soluong)
                    {
                        var Max = _context.ChiTietSLHangHoas.Where(kh => kh.IdHangHoa == idhh && kh.IdKho == modelktSPTeK.IdKho)
                                    .OrderByDescending(kh => kh.NgayAuto).Take(1).Single();
                        var dayyy = Max.NgayAuto.ToShortDateString();
                        if (dayyy != DateTime.Now.ToShortDateString())
                        {
                            //Qua ngay new thi them dong new
                            var kqcthh = new TonKhoData().InsertChiTietSLHangHoa(idhh, soluong, modelktSPTeK.IdKho, "Auto", DBname);
                        }
                        else
                        {
                            Max.SoLuong = soluong;
                            Max.NgayAuto = DateTime.Now;
                            var kqcthh = new TonKhoData().UPdateChiTietSLHangHoa(Max, DBname);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex) { 
                string loi = ex.ToString();
                return false;
            }
        }
    }
}