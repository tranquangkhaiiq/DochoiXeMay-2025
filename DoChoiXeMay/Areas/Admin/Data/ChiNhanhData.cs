using DoChoiXeMay.Models;
using DoChoiXeMay.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Windows.Media.Media3D;

namespace DoChoiXeMay.Areas.Admin.Data
{
    public class ChiNhanhData
    {
        Model1 _context;
        public ChiNhanhData()
        {
            _context = new Model1();
        }
        public KyXuatNhap GetKyByIdXuat(int Id)
        {
            var modelxuat = _context.Ser_XuatSN_CN.Find(Id);
            var modelKy = _context.KyXuatNhaps.Find(modelxuat.IdKyxuat);
            return modelKy;
        }
        public Ser_ChiNhanh GetChiNhanhByIdXuat(int Id)
        {
            var modelxuat = _context.Ser_XuatSN_CN.Find(Id);
            var modelCN = _context.Ser_ChiNhanh.Find(modelxuat.IdChiNhanh);
            return modelCN;
        }
        public Ser_ChiNhanh GetChiNhanhbyID(int IDCN)
        {
            //var chinhanh = _context.Ser_ChiNhanh.FirstOrDefault(kh=>kh.Id == IDCN);
            var chinhanh = _context.Ser_ChiNhanh.Find(IDCN);
            return chinhanh;
        }
        public string MakeNameChiNhanh(int khuvucold,int khuvucnew,int level, string sttofkv)
        {
            var kq = "";
            var stt = "";
            var listCN = _context.Ser_ChiNhanh.Where(kh=>kh.IdKhuVuc==khuvucnew).Count();
            var kv = _context.KhuVucs.Find(khuvucnew);
            var lv = _context.Ser_Levelchinhanh.Find(level);
            if (listCN == 0)
            {
                stt = "0001";
                kq = lv.Viettat+"_"+kv.Viettat+"_"+stt;
            }
            else
            {
                if (khuvucold == khuvucnew) //khong thay doi Khuvuc
                {
                    stt = sttofkv; //Xai stt cu
                }
                else
                {
                    stt = XString.makeSTT(listCN + 1);
                }
                
                kq = lv.Viettat + "_" + kv.Viettat + "_" + stt;
                var check = _context.Ser_ChiNhanh.FirstOrDefault(kh => kh.TenChiNhanh == kq);
                if (check != null)
                {
                    for (int i = 0; i < 100; i++)
                    {
                        stt = XString.makeSTT(listCN + i + 1);
                        kq = lv.Viettat + "_" + kv.Viettat + "_" + stt;
                        check = _context.Ser_ChiNhanh.FirstOrDefault(kh => kh.TenChiNhanh == kq);
                        if (check == null)
                        {
                            break;
                        }
                    }
                }
            }
            return kq;
        }
        public bool Update_ChiNhanh(Ser_ChiNhanh model, string DBname)
        {
            var update = _context.Database.ExecuteSqlCommand("update [" + DBname + "TechZone].[dbo].[Ser_ChiNhanh] set " +
                "TenChiNhanh=@TenChiNhanh,DaiDien=@DaiDien,SDT=@SDT," +
                "IdKhuVuc=@IdKhuVuc,STTCNOFTinh=@STTCNOFTinh,DiaChi=@DiaChi,TaiKhoanNH=@TaiKhoanNH,Gmail=@Gmail," +
                "Sudung=@Sudung,IdLevel=@IdLevel,IdUser=@IdUser,GhiChu=@GhiChu " +
                "where Id=@Id",
                new SqlParameter("@TenChiNhanh", model.TenChiNhanh),
                new SqlParameter("@DaiDien", model.DaiDien),
                new SqlParameter("@SDT", model.SDT),
                new SqlParameter("@IdKhuVuc", model.IdKhuVuc),
                new SqlParameter("@STTCNOFTinh", model.STTCNOFTinh),
                new SqlParameter("@DiaChi", model.DiaChi),
                new SqlParameter("@TaiKhoanNH", model.TaiKhoanNH),
                new SqlParameter("@Gmail", model.Gmail),
                new SqlParameter("@Sudung", model.Sudung),
                new SqlParameter("@IdLevel", model.IdLevel),
                new SqlParameter("@IdUser", model.IdUser),
                //new SqlParameter("@Hinh", model.Hinh),
                new SqlParameter("@GhiChu", model.GhiChu),
                new SqlParameter("@Id", model.Id));
            if (update > 0)
            {
                return true;
            }
            return false;
        }
    }
}