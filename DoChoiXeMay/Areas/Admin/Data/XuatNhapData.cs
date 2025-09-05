using DoChoiXeMay.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace DoChoiXeMay.Areas.Admin.Data
{
    public class XuatNhapData
    {
        Model1 _context = new Model1();
        public double getTongTienAuto(int IdKy)
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
        public List<ChitietXuatNhap> GetListByKy(int id)
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
        public List<KyXuatNhap> getXuatNhapTek(int Sec, int pageSize,int UserId)
        {
            List<KyXuatNhap> model1 = new List<KyXuatNhap>();
            if (UserId == 0)
            {
                model1 = _context.KyXuatNhaps.Where(kh => kh.Id > 1 && kh.AdminXNPUSH == true
                    && kh.UPush == true)
                    .OrderBy(kh => kh.NgayAuto)
                    .ToList();
            }
            else
            {
                model1 = _context.KyXuatNhaps.Where(kh => kh.Id > 1 && kh.AdminXNPUSH == true
                    && kh.UPush == true && kh.UserId == UserId)
                    .OrderBy(kh => kh.NgayAuto)
                    .ToList();
            }
            
            for (int i = 0; i < model1.Count(); i++)
            {
                model1[i].STT = (i +1).ToString();
            }

            model1 = model1
                .OrderByDescending(kh => kh.NgayAuto)
                .Skip(Sec * pageSize)
                            .Take(pageSize)
                            .ToList();
            return model1;
        }
        public int GetPageCountXuatNhapTek(int UserId=0)
        {
            var model1 = 0;
            if (UserId == 0)
            {
                model1 = _context.KyXuatNhaps.Where(kh => kh.Id > 1 && kh.AdminXNPUSH == true
                        && kh.UPush == true).Count();
            }
            else
            {
                model1 = _context.KyXuatNhaps.Where(kh => kh.Id > 1 && kh.AdminXNPUSH == true
                        && kh.UPush == true && kh.UserId == UserId).Count();
            }
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
        public static string[] CheckHHTEKaotu(Model1 db,string Tenhh, int Hangsx = 0, int Mau = 0, int Size = 0)
        {
            var model = db.HangHoas.Where(kh => kh.Ten.ToLower().Trim() == Tenhh.ToLower().Trim()).ToList();
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
                        ThongbLog[i + 1] = ThongbLog[i + 1] + " , Khác Hãng(" + model[i].Manufacturer.Name + ")";
                    }
                    if (model[i].IDColor != Mau)
                    {
                        ThongbLog[i + 1] = ThongbLog[i + 1] + " , Khác Màu(màu " + model[i].Color.TenColor + ")";
                    }
                    if (model[i].IDSize != Size)
                    {
                        ThongbLog[i + 1] = ThongbLog[i + 1] + " , Khác Size(size " + model[i].Size.TenSize + ")";
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
        public static bool GhibangHangHoa(Model1 db,string Ten, int Hangsx, int Mau, int Size, int soluong, double gianhap, string hinh1,string hinh2, string hinh3)
        {
            //dùng cho kỳ xuất (thu hồi)
            try
            {
                var modelhh = db.HangHoas.FirstOrDefault(kh => kh.Ten.ToLower().Trim() == Ten.ToLower().Trim() && kh.IDMF == Hangsx
                                                && kh.IDColor == Mau && kh.IDSize == Size);
                if (modelhh != null)
                {
                    var model = db.HangHoas.Find(modelhh.Id);
                    model.SoLuong = model.SoLuong + soluong;
                    model.GiaNhap = gianhap;
                    model.Hinh1 = hinh1 !=""?hinh1:model.Hinh1;
                    model.Hinh2 = hinh2 !=""?hinh2:model.Hinh2;
                    model.Hinh3 = hinh3 !=""?hinh3:model.Hinh3;
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    HangHoa model = new HangHoa();
                    model.Ten = Ten;
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
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                string msg = e.Message;
                return false;
            }
        }
        public static bool XuatHangHoa(Model1 db, string Ten, int Hangsx, int Mau, int Size, int soluong)
        {
            //dùng cho kỳ nhập (thu hồi)
            try
            {
                var modelhh = db.HangHoas.FirstOrDefault(kh => kh.Ten.ToLower().Trim() == Ten.ToLower().Trim() && kh.IDMF == Hangsx
                                                && kh.IDColor == Mau && kh.IDSize == Size);
                if (modelhh != null)
                {
                    if(modelhh.SoLuong == soluong)//delete
                    {
                        var model = db.HangHoas.Find(modelhh.Id);
                        //Xoa hinh cu
                        bool xoahinhcu1 = XstringAdmin.Xoahinhcu("imgxuatnhap/", model.Hinh1);
                        bool xoahinhcu2 = XstringAdmin.Xoahinhcu("imgxuatnhap/", model.Hinh2);
                        bool xoahinhcu3 = XstringAdmin.Xoahinhcu("imgxuatnhap/", model.Hinh3);
                        db.HangHoas.Remove(modelhh);
                        db.SaveChanges();
                        return true;
                    }
                    if(modelhh.SoLuong > soluong)//UPDATE SOLUONG
                    {
                        var model = db.HangHoas.Find(modelhh.Id);
                        model.SoLuong = model.SoLuong - soluong;
                        model.NgayAuto = DateTime.Now;
                        db.Entry(model).State = EntityState.Modified;
                        db.SaveChanges();
                        return true;
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
        public static bool kiemtrasoluongHH(Model1 db,int id)
        {
            var modelct = db.ChitietXuatNhaps.Where(kh => kh.IdKy == id).ToList();
            for (int i = 0; i < modelct.Count(); i++)
            {
                string tt = modelct[i].Ten.ToLower().Trim(); int IDMF = modelct[i].IDMF; int IDColor = modelct[i].IDColor; int IDSize = modelct[i].IDSize;
                var listhhkt = db.HangHoas.FirstOrDefault(kh => kh.Ten.ToLower().Trim() == tt &&
                        kh.IDMF == IDMF && kh.IDColor == IDColor && kh.IDSize == IDSize);
                if (listhhkt == null || listhhkt.SoLuong < modelct[i].SoLuong)
                {
                    return false;
                }
            }
            return true;
        }
        }
}