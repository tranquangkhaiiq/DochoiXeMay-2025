using DoChoiXeMay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoChoiXeMay.Areas.Admin.Data
{
    public class ActiveData
    {
        Model1 _context;
        public ActiveData()
        {
            _context = new Model1();
        }
        public static List<Ser_kichhoat> ChiTietkichhoatDBTEK(Model1 db, int IdCN, string strTK, string tu, string den)
        {
            var tungay = DateTime.Now;
            var denngay = DateTime.Now;
            strTK = strTK.Trim().ToLower();
            List<Ser_kichhoat> model = new List<Ser_kichhoat>();
            if (IdCN == 0)
            {
                model = db.Ser_kichhoat
                    .Where(kh => kh.Ser_sp.SerialSP.ToLower().Contains(strTK)
                            || kh.Ser_box.Serial.ToLower().Contains(strTK)
                            || kh.DiaChiKhach.ToLower().Contains(strTK)
                            || kh.TenKhachHang.ToLower().Contains(strTK)
                            || kh.Ser_box.LoSanXuat.Contains(strTK)).ToList();
            }
            else
            {
                model = db.Ser_kichhoat.Where(kh => kh.IdChiNhanh == IdCN
                        && (kh.Ser_sp.SerialSP.ToLower().Contains(strTK)
                            || kh.Ser_box.Serial.ToLower().Contains(strTK)
                            || kh.DiaChiKhach.ToLower().Contains(strTK)
                            || kh.TenKhachHang.ToLower().Contains(strTK)
                            || kh.Ser_box.LoSanXuat.Contains(strTK))).ToList();
            }
            if (tu != "" && den != "")
            {
                tungay = DateTime.Parse(tu);
                denngay = DateTime.Parse(den);
                model=model.Where(kh => kh.NgayKichHoat >= tungay && kh.NgayKichHoat <= denngay).ToList();
            }
            return model;
        }
        public List<Ser_kichhoat> getSNACTek(int Sec, int pageSize, int IdCN, string strTK, string tu, string den)
        {
            List<Ser_kichhoat> model1 = new List<Ser_kichhoat>();
            model1 = ChiTietkichhoatDBTEK(_context, IdCN, strTK, tu, den)
                    .OrderByDescending(kh => kh.NgayUpdate)
                    .ThenByDescending(kh => kh.IdChiNhanh)
                    .ThenBy(kh => kh.TrangThaiId)
                    .Skip(Sec * pageSize)
                    .Take(pageSize)
                    .ToList();
            return model1;
        }
        public int GetPageCountACTek(int IdCN, string strTK, string tu, string den)
        {
            var model1 = 0;
            model1 = ChiTietkichhoatDBTEK(_context, IdCN, strTK, tu, den).Count();
            return model1;
        }
        public int InsertKichHoatBH(string IdBox, string IdSP, int chinhanh, string email, string tenkh, string sdt, string khuvuc)
        {
            try
            {

                Ser_kichhoat kh = new Ser_kichhoat();
                kh.Id = Guid.NewGuid();
                kh.IDSer_box = new Guid(IdBox);
                kh.IDSer_sp = new Guid(IdSP);
                kh.NgayKichHoat = DateTime.Now;
                kh.NgayUpdate = DateTime.Now;
                kh.EmailKichHoat = email;
                kh.TenKhachHang = tenkh;
                kh.SDT = sdt;
                kh.IdChiNhanh = chinhanh;
                kh.TrangThaiId = 1;
                kh.DiaChiKhach = khuvuc;
                int tgbh = _context.Ser_sp.Find(kh.IDSer_sp).BaoHanh;
                kh.Ghichu = DateTime.Now.AddMonths(tgbh).ToShortDateString();
                _context.Ser_kichhoat.Add(kh);
                int kt = _context.SaveChanges();
                if (kt > 0)
                {
                    return tgbh;
                }
                return -1;
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return -1;
            }
        }
        public string InsertUserAotu()
        {
            UserTek model = new UserTek();
            var name = "TeK" + (_context.UserTeks.Count() + 1000).ToString();
            var checkname = _context.UserTeks.FirstOrDefault(kh => kh.UserName == name);
            if (checkname != null)
            {
                //Nếu name bị trùng, random 100 lần
                for (int j = 0; j < 100; j++)
                {
                    name = "TeK" + (_context.UserTeks.Count() + 1000 * (j + 2)).ToString();
                    checkname = _context.UserTeks.FirstOrDefault(kh => kh.UserName == name);
                    if (checkname == null)
                    {
                        model.UserName = name;
                        break;
                    }
                }
            }
            else
            {
                model.UserName = name;
            }
            model.Password = "4+szJJPdHNwGTpohvWoq5W0FS0TGKrNhny2zvF6cf64fgvm9EvAuew==";
            model.PasswordSalt = "cwYRNpQl/Jissz6PZo/oUjHBEsYJw8w=";
            model.IdLoai = 5;
            model.LoaiConnection = "";
            model.EmailConnection = "email@gmail.com";
            model.Islocked = false;
            model.lastPasswordChangedate = DateTime.Now;
            model.LastLokedChangedate = DateTime.Now;
            model.Createdate = DateTime.Now;
            model.CountFailedPassword = 0;
            model.GhiChu = "";
            model.Avatar = "";
            _context.UserTeks.Add(model);
            var kq = _context.SaveChanges();
            if (kq > 0)
            {
                return model.UserName;
            }
            return "No";
        }
        public bool CheckSNActive(string SN)
        {
            bool kq = false;
            if (SN !=null && SN.Length == 14)
            {
                var modelBox = _context.Ser_box.FirstOrDefault(kh => kh.Serial == SN);
                if (modelBox != null) {
                    var modelKH = _context.Ser_kichhoat.FirstOrDefault(kh => kh.IDSer_box == modelBox.Id);
                    if (modelKH != null) kq = true;
                }
            }
            else if(SN != null && SN.Length == 11)
            {
                var modelSP = _context.Ser_sp.FirstOrDefault(kh => kh.SerialSP == SN);
                if (modelSP != null) { 
                    var modelKH = _context.Ser_kichhoat.FirstOrDefault(kh=>kh.IDSer_sp == modelSP.Id);
                    if(modelKH != null)kq = true;
                }
            }
            return kq;
        }
    }
}