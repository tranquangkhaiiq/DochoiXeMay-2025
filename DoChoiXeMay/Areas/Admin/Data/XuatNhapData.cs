using DoChoiXeMay.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Windows.Media.Media3D;
using System.Xml.Linq;

namespace DoChoiXeMay.Areas.Admin.Data
{
    public class XuatNhapData
    {
        Model1 _context = new Model1();
        public double getTongTienAuto(int IdKy = 0)
        {
            var Ky = _context.KyXuatNhaps.FirstOrDefault(kh => kh.Id == IdKy);
            if(Ky != null)
            {
                return Ky.TongTienAuto;
            }
            return 0;
        }
        public bool UPdateKyXN(KyXuatNhap XN)
        {
            try
            {
                _context.Entry(XN).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return false;
            }
        }
        public bool UPdateChiTietKy(ChitietXuatNhap model, string DBname)
        {
            try
            {
                var update = _context.Database.ExecuteSqlCommand("update [" + DBname + "TechZone].[dbo].[ChitietXuatNhap] set " +
                "Ten=@Ten,IdKy=@IdKy,SoLuong=@SoLuong," +
                "Gianhap=@Gianhap,NgayAuto=@NgayAuto,Hinh1=@Hinh1,Hinh2=@Hinh2,Hinh3=@Hinh3," +
                "IDMF=@IDMF,IDColor=@IDColor,IDSize=@IDSize,GhiChu=@GhiChu,SerialHop=@SerialHop," +
                "SerialSP=@SerialSP,DaActive=@DaActive,IdDoiTra=@IdDoiTra " +
                "where Id=@Id",
                new SqlParameter("@Ten", model.Ten),
                new SqlParameter("@IdKy", model.IdKy),
                new SqlParameter("@SoLuong", model.SoLuong),
                new SqlParameter("@Gianhap", model.Gianhap),
                new SqlParameter("@NgayAuto", model.NgayAuto),
                new SqlParameter("@Hinh1", model.Hinh1),
                new SqlParameter("@Hinh2", model.Hinh2),
                new SqlParameter("@Hinh3", model.Hinh3),
                new SqlParameter("@IDMF", model.IDMF),
                new SqlParameter("@IDColor", model.IDColor),
                new SqlParameter("@IDSize", model.IDSize),
                new SqlParameter("@GhiChu", ""),
                new SqlParameter("@SerialHop", model.SerialHop),
                new SqlParameter("@SerialSP", model.SerialSP),
                new SqlParameter("@DaActive", model.DaActive),
                new SqlParameter("@IdDoiTra", model.IdDoiTra),
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
        public static bool InsertNhatKy_Admin(Model1 dbc, int UserID, string LoaiUser, string UserName, string CongViec, string GhiChu)
        {
            try
            {
                if (LoaiUser != "Guest")
                {
                    NhatKyUTek model = new NhatKyUTek();
                    model.Id = Guid.NewGuid();
                    model.UserID = UserID;
                    model.UserName = UserName;
                    model.LoaiUser = LoaiUser;
                    model.CreateDate = DateTime.Now;
                    model.CongViec = CongViec;
                    model.GhiChu = GhiChu;
                    dbc.NhatKyUTeks.Add(model);
                    dbc.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return false;
            }

        }
        public List<ChitietXuatNhap> GetListByKy(int id = 0)
        {
            var model = _context.ChitietXuatNhaps.Where(kh=>kh.IdKy == id)
                    .OrderByDescending(kh=>kh.NgayAuto)
                    .ToList();
            for(int i = 0; i < model.Count(); i++)
            {
                model[i].GhiChu = (i + 1).ToString();
            }
            return model;
        }
        public double kiemtraNgayCTXN(string id)
        {
            var model = _context.ChitietXuatNhaps.Find(new Guid(id));
            DateTime ngayHienTai = DateTime.Now;
            var kt = (ngayHienTai - model.NgayAuto).TotalDays;
            
            return kt;
        }
        public bool kiemtrahethanBH(string id)
        {
            var ht = DateTime.Now;
            var model = _context.ChitietXuatNhaps.Find(new Guid(id));
            if(model.IdDoiTra==1 && model.DaActive != null)
            {
                var idserBox = _context.Ser_box.FirstOrDefault(kh => kh.Serial == model.SerialHop);
                var idserSP = _context.Ser_sp.FirstOrDefault(kh => kh.SerialSP == model.SerialSP);
                if (idserBox != null && idserSP != null) {
                    var modelkt = _context.Ser_kichhoat.FirstOrDefault(kh => kh.IDSer_box == idserBox.Id
                    && kh.IDSer_sp == idserSP.Id && kh.TenKhachHang == model.KyXuatNhap.TenKy);
                    if(modelkt != null)
                    {
                        int tgbh = _context.Ser_sp.Find(modelkt.IDSer_sp).BaoHanh;
                        var hethan = modelkt.NgayKichHoat.AddMonths(tgbh);
                        if (ht > hethan)
                        {
                            return true;
                        }
                        return false;
                    }
                    return false;
                }
                return false;
            }
            return false;
        }
        public static List<KyXuatNhap> ChiTietKyXuatNhapTEK(Model1 db, string strk="",string ngay="", int idLHXN=0, int IdSan = 0, int Iddoitra=0, int UserId = 0)
        {
            List<KyXuatNhap> model = new List<KyXuatNhap>();
            List<KyXuatNhap> model1 = new List<KyXuatNhap>();
            var tungay = DateTime.Now;
            if (ngay != "")
            {
                tungay = DateTime.Parse(ngay);
            }
            
            var ctxnbySerial = db.ChitietXuatNhaps.FirstOrDefault(kh=>kh.SoLuong == 1 && (kh.SerialHop.ToLower() == strk || kh.SerialSP.ToLower() == strk));
            
            if (ctxnbySerial == null)//Không phải Serial thì dò theo tên
            {
                model = db.KyXuatNhaps.Where(kh => kh.Id > 1 && kh.AdminXNPUSH == true
                    && kh.UPush == true && kh.TenKy.ToLower().Contains(strk))
                    .OrderBy(kh => kh.NgayAuto).ToList();
            }
            else if(ctxnbySerial !=null)
            {
                //lấy tất cả các dòng
                var ctxnbySerialList = db.ChitietXuatNhaps.Where(kh => kh.SerialHop.ToLower() == strk || kh.SerialSP.ToLower() == strk).ToList();
                for (int i = 0; i < ctxnbySerialList.Count; i++)
                {
                    var id = ctxnbySerialList[i].IdKy;
                    KyXuatNhap getky = db.KyXuatNhaps.FirstOrDefault(kh => kh.Id > 1 && kh.AdminXNPUSH == true
                    && kh.UPush == true && kh.Id == id);
                    model.Add(getky);
                }
                
            }
            if (Iddoitra > 0)
            {
                model = new List<KyXuatNhap>();
                var ctxndoitra = db.ChitietXuatNhaps.Where(kh => kh.IdDoiTra == Iddoitra).ToList();
                for (int i = 0; i < ctxndoitra.Count; i++)
                {
                    var id = ctxndoitra[i].IdKy;
                    var check = model.FirstOrDefault(kh => kh.Id == id);
                    if (check == null)
                    {
                        KyXuatNhap getky = db.KyXuatNhaps.FirstOrDefault(kh => kh.Id > 1 && kh.AdminXNPUSH == true
                        && kh.UPush == true && kh.Id == id);
                        model.Add(getky);
                    }
                }
            }
            if (ngay != "")
            {
                model=model.Where(kh=>kh.NgayXuatNhap.Date==tungay.Date).ToList();
            }
            if (UserId == 0 && idLHXN == 0 && IdSan==0)
            {
                model1 = model; 
            }
            else if (UserId > 0 && idLHXN == 0 && IdSan==0)
            {
                model1 = model.Where(kh=> kh.UserId == UserId).ToList();
            }else if (UserId == 0 && idLHXN > 0 && IdSan == 0)
            {
                model1 = model.Where(kh => kh.IdLoaiHangXN == idLHXN).ToList();
            }
            else if (UserId == 0 && idLHXN == 0 && IdSan > 0)
            {
                model1 = model.Where(kh => kh.IdSan == IdSan).ToList();
            }
            else if(UserId == 0 && idLHXN > 0 && IdSan > 0)
            {
                model1 = model.Where(kh => kh.IdLoaiHangXN == idLHXN && kh.IdSan == IdSan).ToList();
            }
            else if(UserId > 0 && idLHXN > 0 && IdSan == 0)
            {
                model1 = model.Where(kh => kh.IdLoaiHangXN == idLHXN && kh.UserId == UserId).ToList();
            }
            else if(UserId > 0 && idLHXN == 0 && IdSan > 0)
            {
                model1 = model.Where(kh => kh.IdSan == IdSan && kh.UserId == UserId).ToList();
            }
            else
            {
                model1 = model.Where(kh => kh.IdSan == IdSan && kh.UserId == UserId && kh.IdLoaiHangXN == idLHXN).ToList();
            }
            return model1;
        }
        public List<KyXuatNhap> getXuatNhapTek(string ngay="",string strk="",int idLHXN = 0,int IdSan = 0, int Iddoitra = 0, int Sec = 0, int pageSize = 0,int UserId = 0)
        {
            List<KyXuatNhap> model1 = new List<KyXuatNhap>();
            model1 = ChiTietKyXuatNhapTEK(_context, strk,ngay, idLHXN,IdSan,Iddoitra, UserId)
                .OrderByDescending(kh => kh.NgayAuto)
                .ToList();


            for (int i = 0; i < model1.Count(); i++)
            {
                model1[i].STT = (i +1).ToString();
            }

            model1 = model1
                .Skip(Sec * pageSize)
                            .Take(pageSize)
                            .ToList();
            return model1;
        }
        public int GetPageCountXuatNhapTek(string ngay="",string strk = "",int idLHXN=0,int IdSan = 0, int Iddoitra = 0, int UserId=0)
        {
            var model1 = 0;
            model1 = ChiTietKyXuatNhapTEK(_context, strk,ngay, idLHXN,IdSan,Iddoitra, UserId).Count();
            return model1;
        }
        public static bool InsertMsgAotu(Model1 dbc,int UserId, string MsgSys,bool AdminDaxem, bool Sub2Daxem,bool Sub4Daxem,bool Sub5Daxem,bool Sub6Daxem)
        {
            try
            {
                MsgAotu model = new MsgAotu();
                model.Id = Guid.NewGuid();
                model.UserIdmsgAotu = UserId;
                model.MsgHeThong = MsgSys;
                model.NgayTao = DateTime.Now;
                model.AdminDaxem = AdminDaxem;
                model.Sub2Daxem = Sub2Daxem;
                model.Sub4Daxem = Sub4Daxem;
                model.Sub5Daxem = Sub5Daxem;
                model.Sub6Daxem = Sub6Daxem;
                dbc.MsgAotus.Add(model);
                dbc.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return false;
            }
            
        }
        public static string[] CheckHHTEKaotu(Model1 db,string Tenhh, int Hangsx = 0, int Mau = 0, int Size = 0, int idkho=0)
        {
            var model = db.HangHoas.Where(kh =>kh.IdKho == idkho && kh.Ten.ToLower().Trim() == Tenhh.ToLower().Trim()).ToList();
            if (model.Count() > 1)
            {
                //nhiều dòng
                string[] ThongbLog = new string[model.Count()+1];
                ThongbLog[0] = "Kho hiện có " + model.Sum(kh=>kh.SoLuong).ToString() + " sản phẩm cùng tên gồm:";
                for (int i = 0; i < model.Count(); i++)
                {
                    ThongbLog[i+1] = "_Có " + model[i].SoLuong.ToString() + " sản phẩm cùng Tên";
                    if (model[i].IDMF == Hangsx)
                    {
                        ThongbLog[i+1] = ThongbLog[i+1] + " - cùng Hãng";
                    }
                    if (model[i].IDColor == Mau)
                    {
                        ThongbLog[i + 1] = ThongbLog[i + 1] + " - cùng Màu";
                    }
                    if (model[i].IDSize == Size)
                    {
                        ThongbLog[i + 1] = ThongbLog[i + 1] + " - cùng Size";
                    }
                    if (model[i].IDMF != Hangsx)
                    {
                        ThongbLog[i + 1] = ThongbLog[i + 1] + " , Khác Hãng(" + model[i].Manufacturer.Name + ")!!!";
                    }
                    if (model[i].IDColor != Mau)
                    {
                        ThongbLog[i + 1] = ThongbLog[i + 1] + " , Khác Màu(màu " + model[i].Color.TenColor + ")!!!";
                    }
                    if (model[i].IDSize != Size)
                    {
                        ThongbLog[i + 1] = ThongbLog[i + 1] + " , Khác Size(size " + model[i].Size.TenSize + ")!!!";
                    }
                }
                return ThongbLog;
            }else if(model.Count() == 1)
            {
                //1 dòng
                string[] ThongbLog = new string[model.Count()];
                for (int i = 0; i < model.Count(); i++)
                {
                    ThongbLog[i] = "Kho Có " + model[i].SoLuong.ToString() + " sản phẩm cùng Tên";
                    if (model[i].IDMF == Hangsx)
                    {
                        ThongbLog[i] = ThongbLog[i] + " - cùng Hãng";
                    }
                    if (model[i].IDColor == Mau)
                    {
                        ThongbLog[i] = ThongbLog[i] + " - cùng Màu";
                    }
                    if (model[i].IDSize == Size)
                    {
                        ThongbLog[i] = ThongbLog[i] + " - cùng Size";
                    }
                    if (model[i].IDMF != Hangsx)
                    {
                        ThongbLog[i] = ThongbLog[i] + " , Khác Hãng(" + model[i].Manufacturer.Name + ")";
                    }
                    if (model[i].IDColor != Mau)
                    {
                        ThongbLog[i] = ThongbLog[i] + " , Khác Màu(màu " + model[i].Color.TenColor + ")";
                    }
                    if (model[i].IDSize != Size)
                    {
                        ThongbLog[i] = ThongbLog[i] + " , Khác Size(size " + model[i].Size.TenSize + ")";
                    }
                }
                return ThongbLog;
            }   
            return null;    
        }
        public static bool GhibangHangHoa(Model1 db,string DBname, string Ten, int Hangsx, int Mau, int Size, int soluong, double gianhap, string hinh1,string hinh2, string hinh3, int idkho)
        {
            //dùng cho kỳ xuất (thu hồi)
            //07/03/2026 thêm idkho
            try
            {
                var modelhh = db.HangHoas.FirstOrDefault(kh => kh.Ten.ToLower().Trim() == Ten.ToLower().Trim() && kh.IDMF == Hangsx
                                                && kh.IDColor == Mau && kh.IDSize == Size && kh.IdKho == idkho);
                if (modelhh != null)
                {
                    var model = db.HangHoas.Find(modelhh.Id);
                    model.SoLuong = model.SoLuong + soluong;
                    model.GiaNhap = gianhap;
                    model.Hinh1 = hinh1 !=""?hinh1:model.Hinh1;
                    model.Hinh2 = hinh2 !=""?hinh2:model.Hinh2;
                    model.Hinh3 = hinh3 !=""?hinh3:model.Hinh3;
                    model.NgayAuto = DateTime.Now;
                    db.Entry(model).State = EntityState.Modified;
                    var kq= db.SaveChanges();
                    //ChiTietSLHangHoas 13 thang 2
                    //Chua co thi Insert, co roi thi update
                    if (kq > 0)
                    {
                        new TonKhoData().AutoChiTietSLHangHoa(model.Id, model.SoLuong, DBname);
                        return true;
                    }
                    return false;
                }
                else
                {
                    HangHoa model = new HangHoa();
                    model.Ten = Ten;
                    model.IdKho = idkho;
                    model.IDKy = 1;//NVL = 1; SP=0
                    model.SoLuong = soluong;
                    model.GiaNhap = gianhap;
                    model.NgayAuto = DateTime.Now;
                    model.IdLoai = 1;
                    model.Hinh1 = hinh1;
                    model.Hinh2 = hinh2;
                    model.Hinh3 = hinh3;
                    model.IDMF = Hangsx;
                    model.IDColor = Mau;
                    model.IDSize = Size;
                    model.GhiChu = "";
                    db.HangHoas.Add(model);
                    var kq= db.SaveChanges();
                    //ChiTietSLHangHoas 13 thang 2
                    if (kq > 0)
                    {
                        var modelktSPTeK = db.HangHoas.FirstOrDefault(kh => kh.IDMF == 5 && kh.IDKy == 0 && kh.Id == model.Id);
                        if (modelktSPTeK !=null)
                        {
                            new TonKhoData().InsertChiTietSLHangHoa(model.Id, model.SoLuong,model.IdKho, "Auto", DBname);
                        }
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                string msg = e.Message;
                return false;
            }
        }
        
        public static bool XuatHangHoa(Model1 db,string DBname, string Ten, int Hangsx = 0, int Mau = 0, int Size = 0, int soluong = 0, int idkho=0)
        {
            //dùng cho kỳ nhập (thu hồi) ??
            //07/03/2026 thêm idkho
            try
            {
                var modelhh = db.HangHoas.FirstOrDefault(kh => kh.Ten.ToLower().Trim() == Ten.ToLower().Trim() && kh.IDMF == Hangsx
                                                && kh.IDColor == Mau && kh.IDSize == Size && kh.IdKho == idkho);
                if (modelhh != null)
                {
                    //Cancel code delete hang hoa
                    if(modelhh.SoLuong >= soluong)//UPDATE SOLUONG
                    {
                        var model = db.HangHoas.Find(modelhh.Id);
                        model.SoLuong = model.SoLuong - soluong;
                        model.NgayAuto = DateTime.Now;
                        db.Entry(model).State = EntityState.Modified;
                        var kq = db.SaveChanges();
                        //ChiTietSLHangHoas 13 thang 2
                        //Chua co thi Insert, co roi thi update
                        if (kq > 0 && modelhh.IDKy==0)
                        {
                            new TonKhoData().AutoChiTietSLHangHoa(model.Id, model.SoLuong, DBname);
                            return true;
                        }
                        return false;
                    }
                    if(modelhh.SoLuong < soluong)
                    {
                        return false;
                    }
                }
                    return true;
            }
            catch (Exception e)
            {
                string msg = e.Message;
                return false;
            }
            
        }
        public static bool kiemtrasoluongHH(Model1 db,int id = 0)
        {
            var xn = db.KyXuatNhaps.FirstOrDefault(kh => kh.Id == id);
            var modelct = db.ChitietXuatNhaps.Where(kh => kh.IdKy == id).ToList();
            
            for (int i = 0; i < modelct.Count(); i++)
            {
                string tt = modelct[i].Ten.ToLower().Trim(); int IDMF = modelct[i].IDMF; 
                int IDColor = modelct[i].IDColor; int IDSize = modelct[i].IDSize; int idkho = xn.IdKho;
                //07/03/2026 thêm idKho
                var listhhkt = db.HangHoas.FirstOrDefault(kh => kh.Ten.ToLower().Trim() == tt &&
                        kh.IDMF == IDMF && kh.IDColor == IDColor && kh.IDSize == IDSize && kh.IdKho==idkho);
                if (listhhkt == null || listhhkt.SoLuong < modelct[i].SoLuong)
                {
                    return false;
                }
            }
            return true;
        }
        public static string GetSerialbySerial(Model1 db, string str,int idkho)
        {
            string kq = "";
            if (str !=null && str.Length == 14)
            {
                var kq1 = db.Ser_sp.FirstOrDefault(kh =>kh.IdKho==idkho && kh.Ser_box.Serial == str);
                kq = kq1 != null ? kq1.SerialSP : "";
            }
            if (str != null && str.Length == 11)
            {
                var kq2 = db.Ser_sp.FirstOrDefault(kh => kh.IdKho == idkho && kh.SerialSP == str);
                kq = kq2 != null ? kq2.Ser_box.Serial : "";
            }
                return kq;
        }
     }
}