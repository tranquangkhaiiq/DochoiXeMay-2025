using DoChoiXeMay.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using static QRCoder.PayloadGenerator.SwissQrCode;

namespace DoChoiXeMay.Controllers
{
    public class LookController : Controller
    {
        // GET: Look
        Model1 dbc = new Model1();
        static String TangCa = ConfigurationManager.AppSettings["TangCa"];
        static String Le = ConfigurationManager.AppSettings["Le"];
        public ActionResult Partime()
        {
            // Lấy ngày hiện tại
            DateTime date = DateTime.Now;
            CultureInfo ci = CultureInfo.CurrentCulture; // Sử dụng hiện tại của hệ thống
            Calendar cal = ci.Calendar;
            CalendarWeekRule rule = ci.DateTimeFormat.CalendarWeekRule;
            DayOfWeek firstDayOfWeek = ci.DateTimeFormat.FirstDayOfWeek;
            int weekNumber = cal.GetWeekOfYear(date, rule, firstDayOfWeek);
            ViewBag.TuanCu4 = weekNumber - 4;
            ViewBag.TuanCu3 = weekNumber - 3;
            ViewBag.TuanCu2 = weekNumber - 2;
            ViewBag.TuanCu1 = weekNumber - 1;
            ViewBag.TuanHT = weekNumber;
            ViewBag.Tuanmoi = weekNumber + 1;
            ViewBag.Tuanmoihon = weekNumber + 2;
            ViewBag.Idnhanvien = new SelectList(dbc.NV_NhanVienTek.Where(kh => kh.NV_Vitrinhanvien.DonViTinh == "Gio"
                            && kh.DaNghiViec == false), "Id", "HoTen");
            //16thang12
            ViewBag.IdSan = new SelectList(dbc.SanThuongMais.Where(kh => kh.SuDung == true), "Id", "TenSan");
            ViewBag.IdLoaiHangXN = new SelectList(dbc.KyXuatNhap_LoaiHang.ToList(), "Id", "TenLoai");
            ViewBag.TrongTon311 = dbc.HangHoas.Where(kh => kh.Id == 56).Sum(kh => kh.SoLuong);
            ViewBag.TrongTonKho2 = dbc.HangHoas.Where(kh => kh.Id == 1066).Sum(kh => kh.SoLuong);

            ViewBag.KhoiTon311 = dbc.HangHoas.Where(kh => kh.Id == 55).Sum(kh => kh.SoLuong);
            ViewBag.KhoiTonKho2 = dbc.HangHoas.Where(kh => kh.Id == 66).Sum(kh => kh.SoLuong);

            ViewBag.TrongTonChuaVoHop = dbc.HangHoas.Where(kh => kh.Id == 1069).Sum(kh => kh.SoLuong);
            ViewBag.KhoiTonChuaVoHop = dbc.HangHoas.Where(kh => kh.Id == 1070).Sum(kh => kh.SoLuong);
            return View();
        }
        public ActionResult DoThiThongKe(int nam=0)
        {
            if(nam > 0)
            {
                Session["Year"] = nam;
            }
            else
            {
                Session.Remove("Year");
            }

            var be = dbc.ChitietXuatNhaps.Where(kh => kh.Ten == "Xi Nhan Wave TNT BLOCKX G2 ZEN 1").ToList();
            var beNhapLoi = dbc.ChitietXuatNhaps.Where(kh => kh.Ten == "Xi Nhan Wave G2 ZEN 1-NhanBaoHanh"
                                && kh.KyXuatNhap.XuatNhap==false).ToList();
            var betraLoi = dbc.ChitietXuatNhaps.Where(kh => kh.Ten == "Xi Nhan Wave G2 ZEN 1-NhanBaoHanh"
                                && kh.KyXuatNhap.XuatNhap == true).ToList();
            if (nam > 0)
            {
                be = dbc.ChitietXuatNhaps.Where(kh => kh.Ten == "Xi Nhan Wave TNT BLOCKX G2 ZEN 1" &&
                                kh.NgayAuto.Year == nam).ToList();
                beNhapLoi = dbc.ChitietXuatNhaps.Where(kh => kh.Ten == "Xi Nhan Wave G2 ZEN 1-NhanBaoHanh"
                                && kh.KyXuatNhap.XuatNhap == false && kh.NgayAuto.Year == nam).ToList();
                betraLoi = dbc.ChitietXuatNhaps.Where(kh => kh.Ten == "Xi Nhan Wave G2 ZEN 1-NhanBaoHanh"
                                && kh.KyXuatNhap.XuatNhap == true && kh.NgayAuto.Year == nam).ToList();
            }
            
            var beg = be.Where(kh => kh.KyXuatNhap.XuatNhap == true).ToList();
            var begsanxuat = be.Where(kh => kh.KyXuatNhap.XuatNhap == false && kh.IdDoiTra == 1 && kh.KyXuatNhap.IdKho==1).ToList();
            var begin = beg.Where(kh => kh.IdDoiTra == 1).ToList();
            var daban = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 1 &&
                                dbc.Ser_XuatSN_CN.FirstOrDefault(kk => kk.IdKyxuat == kh.IdKy && kk.ChuyenKho == true) == null).ToList();
            var DaBanTikTok = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 1 && kh.KyXuatNhap.IdSan == 2 && kh.KyXuatNhap.KhachLe == true).ToList();
            var DaBanShopee = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 1 && kh.KyXuatNhap.IdSan == 3 && kh.KyXuatNhap.KhachLe == true).ToList();
            var DaBanLeNSan = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 1 && kh.KyXuatNhap.IdSan == 1 && kh.KyXuatNhap.KhachLe == true).ToList();
            var DaBanLSiNSan = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 1 && kh.KyXuatNhap.IdSan == 1 &&
                                kh.KyXuatNhap.KhachLe == false && dbc.Ser_XuatSN_CN.FirstOrDefault(kk=>kk.IdKyxuat==kh.IdKy && kk.ChuyenKho==true)==null).ToList();

            //2/4/2026
            ViewBag.DaNhanHangDaiLyLoi = beNhapLoi == null ? 0 : beNhapLoi.Sum(kh => kh.SoLuong);
            ViewBag.DaGuiHangBHDaiLy = betraLoi == null ? 0 : betraLoi.Sum(kh => kh.SoLuong);
            //2/4/2026
            var DaTraHangKhachLe = beg.Where(kh => kh.IdDoiTra == 4).ToList();   //4:Không Lỗi
            var DaTraHangKhachLeLoi = be.Where(kh => kh.IdDoiTra == 3).ToList(); //4:Không Lỗi//3:có lỗi//2:Mới Nhận
            var modeldaban = daban == null ? 0 : daban.Sum(kh => kh.SoLuong);
            ViewBag.daban = modeldaban;
            var Tonkho = dbc.HangHoas.Where(kh => kh.Id == 55 || kh.Id == 56).Sum(kh => kh.SoLuong);
            ViewBag.TongXiNhanGen1Tek = Tonkho;
            var MauDaXuat = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 2).ToList();
            var modelMauDaXuat = MauDaXuat == null ? 0 : MauDaXuat.Sum(kh => kh.SoLuong);
            ViewBag.TongXiNhanGen1MauDaXuat = modelMauDaXuat;
            ViewBag.DaTraHangKhachLe = DaTraHangKhachLe == null ? 0 : DaTraHangKhachLe.Sum(kh => kh.SoLuong);
            var TraHangLeLoi = DaTraHangKhachLeLoi == null ? 0 : DaTraHangKhachLeLoi.Sum(kh => kh.SoLuong);
            ViewBag.DaTraHangKhachLeLoi = TraHangLeLoi;
            //KyXuatNhap.IdLoaiHangXN==4(NoBox)
            var kytrabaohanhct = beg.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 3 || kh.KyXuatNhap.IdLoaiHangXN == 4).ToList();
            if (kytrabaohanhct.Count() == 0)
            {
                ViewBag.TongtraBH = 0;
            }
            else
            {
                ViewBag.TongtraBH = kytrabaohanhct.Sum(kh => kh.SoLuong);
            }
            //27/4/2026 => update dasanxuat 
            var traNoBox = beg.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 4).Sum(kh => kh.SoLuong);
            var trahangnhanBH = betraLoi.Sum(kh => kh.SoLuong);
            //27/4/2026 => update dasanxuat
            var dsx = begsanxuat.Sum(kh => kh.SoLuong);
            var TonkhoT = dbc.HangHoas.Where(kh => kh.Id == 56 || kh.Id==1066).Sum(kh => kh.SoLuong);
            var TonkhoK = dbc.HangHoas.Where(kh => kh.Id == 55 || kh.Id==66).Sum(kh => kh.SoLuong);

            ViewBag.DaSanXuat = dsx + traNoBox+trahangnhanBH;
            Session["DaTraHangKhachLeLoi"] = TraHangLeLoi;
            Session["TongtraBH"] = kytrabaohanhct.Count() == 0 ? 0 : kytrabaohanhct.Sum(kh => kh.SoLuong);
            //Session["TonKhoXiNhanGen1Tek"] = Tonkho;
            Session["TonKhoXiNhanGen1TekT"] = TonkhoT;
            Session["TonKhoXiNhanGen1TekK"] = TonkhoK;
            Session["MauDaXuat"] = modelMauDaXuat;
            //Session["daSX"] = dsx;
            //Đồ thị dạng cột
            var TrongDaBanTikTok = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 1 && kh.IDColor == 5 && kh.KyXuatNhap.IdSan == 2).ToList();
            var modelTrongDaBanTikTok = TrongDaBanTikTok == null ? 0 : TrongDaBanTikTok.Sum(kh => kh.SoLuong);
            Session["TrongDaBanTikTok"] = modelTrongDaBanTikTok;
            var trongDaBanShopee = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 1 && kh.IDColor == 5 && kh.KyXuatNhap.IdSan == 3).ToList();
            var modeltrongDaBanShopee = trongDaBanShopee == null ? 0 : trongDaBanShopee.Sum(kh => kh.SoLuong);
            Session["trongDaBanShopee"] = modeltrongDaBanShopee;
            var trongDaBanLeNSan = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 1 && kh.IDColor == 5 && kh.KyXuatNhap.IdSan == 1 && kh.KyXuatNhap.KhachLe == true).ToList();
            var modeltrongDaBanLeNSan = trongDaBanLeNSan == null ? 0 : trongDaBanLeNSan.Sum(kh => kh.SoLuong);
            Session["trongDaBanLeNSan"] = modeltrongDaBanLeNSan;
            var trongDaBanLSiNSan = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 1 && kh.IDColor == 5 && kh.KyXuatNhap.IdSan == 1 && kh.KyXuatNhap.KhachLe == false
                            && dbc.Ser_XuatSN_CN.FirstOrDefault(kk => kk.IdKyxuat == kh.IdKy && kk.ChuyenKho == true) == null).ToList();
            var modeltrongDaBanLSiNSan = trongDaBanLSiNSan == null ? 0 : trongDaBanLSiNSan.Sum(kh => kh.SoLuong);
            Session["trongDaBanLSiNSan"] = modeltrongDaBanLSiNSan;
            var KhoiDaBanTikTok = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 1 && kh.IDColor == 7 && kh.KyXuatNhap.IdSan == 2).ToList();
            var modelKhoiDaBanTikTok = KhoiDaBanTikTok == null ? 0 : KhoiDaBanTikTok.Sum(kh => kh.SoLuong);
            Session["KhoiDaBanTikTok"] = modelKhoiDaBanTikTok;
            var KhoiDaBanShopee = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 1 && kh.IDColor == 7 && kh.KyXuatNhap.IdSan == 3).ToList();
            var modelKhoiDaBanShopee = KhoiDaBanShopee == null ? 0 : KhoiDaBanShopee.Sum(kh => kh.SoLuong);
            Session["KhoiDaBanShopee"] = modelKhoiDaBanShopee;
            var KhoiDaBanLSiNSan = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 1 && kh.IDColor == 7 && kh.KyXuatNhap.IdSan == 1 && kh.KyXuatNhap.KhachLe == false
                            && dbc.Ser_XuatSN_CN.FirstOrDefault(kk => kk.IdKyxuat == kh.IdKy && kk.ChuyenKho == true) == null).ToList();
            var modelKhoiDaBanLSiNSan = KhoiDaBanLSiNSan == null ? 0 : KhoiDaBanLSiNSan.Sum(kh => kh.SoLuong);
            Session["KhoiDaBanLSiNSan"] = modelKhoiDaBanLSiNSan;
            var KhoiDaBanLeNSan = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 1 && kh.IDColor == 7 && kh.KyXuatNhap.IdSan == 1 && kh.KyXuatNhap.KhachLe == true).ToList();
            var modelKhoiDaBanLeNSan = KhoiDaBanLeNSan == null ? 0 : KhoiDaBanLeNSan.Sum(kh => kh.SoLuong);
            Session["KhoiDaBanLeNSan"] = modelKhoiDaBanLeNSan;
            //Đã bán Trong + Khói
            var modelDaBanTikTok = DaBanTikTok == null ? 0 : DaBanTikTok.Sum(kh => kh.SoLuong);
            ViewBag.DaBanTikTok = modelDaBanTikTok;
            var modelDaBanShopee = DaBanShopee == null ? 0 : DaBanShopee.Sum(kh => kh.SoLuong);
            ViewBag.DaBanShopee = modelDaBanShopee;
            var modelDaBanLeNSan = DaBanLeNSan == null ? 0 : DaBanLeNSan.Sum(kh => kh.SoLuong);
            ViewBag.DaBanLeNSan = modelDaBanLeNSan;
            var modelDaBanLSiNSan = DaBanLSiNSan == null ? 0 : DaBanLSiNSan.Sum(kh => kh.SoLuong);
            ViewBag.DaBanLSiNSan = modelDaBanLSiNSan;

            ViewBag.phantramdaban = (100 * float.Parse(modeldaban.ToString()) / float.Parse(dsx.ToString())).ToString("#0.00");
            ViewBag.phantramTraHangLeLoi = (100 * float.Parse(Session["DaTraHangKhachLeLoi"].ToString()) / float.Parse(dsx.ToString())).ToString("#0.00");
            ViewBag.phantramTongtraBH = (100 * float.Parse(Session["TongtraBH"].ToString()) / float.Parse(dsx.ToString())).ToString("#0.00");
            ViewBag.phantramTonkhoT = (100 * float.Parse(Session["TonKhoXiNhanGen1TekT"].ToString()) / float.Parse(dsx.ToString())).ToString("#0.00");
            ViewBag.phantramTonkhoK = (100 * float.Parse(Session["TonKhoXiNhanGen1TekK"].ToString()) / float.Parse(dsx.ToString())).ToString("#0.00");
            ViewBag.phantramMauDaXuat = (100 * float.Parse(Session["MauDaXuat"].ToString()) / float.Parse(dsx.ToString())).ToString("#0.00");
            //mới ss theo dabannamht = 12 tháng và năm trước
            int namht = 0;
            int namc = 0;
            int thanght = DateTime.Now.Month;
            if (nam > 2025)
            {
                namht = nam;
            }else if (nam == 2025)
            {
                namht = 2026;
            }
            else
            {
                namht=DateTime.Now.Year;
            }
            namc = namht - 1;
            Session["YearHT"]=namht;
            Session["YearC"] = namc;
            var daban2 = dbc.ChitietXuatNhaps.Where(kh => kh.Ten == "Xi Nhan Wave TNT BLOCKX G2 ZEN 1"
                && kh.KyXuatNhap.XuatNhap == true && kh.IdDoiTra == 1 
                && dbc.Ser_XuatSN_CN.FirstOrDefault(kk => kk.IdKyxuat == kh.IdKy && kk.ChuyenKho == true) == null).ToList();
            for (int j = 1; j < 12; j++)
                {
                    if (Session["DaBantht" + j.ToString()] != null)
                    {
                        Session.Remove("DaBantht" + j.ToString());
                    }
                    if (Session["DaBantc" + j.ToString()] != null)
                    {
                        Session.Remove("DaBantc" + j.ToString());
                    }
                }
                for (int i = 1; i <= 12; i++)
                {
                    var dabanthangtht = daban2.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 1
                        && kh.KyXuatNhap.NgayAuto.Year == namht && kh.KyXuatNhap.NgayAuto.Month == i).ToList();
                    Session["DaBantht" + i.ToString()] = dabanthangtht == null ? 0 : dabanthangtht.Sum(kh => kh.SoLuong);
                    var dabanthangtc = daban2.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 1
                        && kh.KyXuatNhap.NgayAuto.Year == namc && kh.KyXuatNhap.NgayAuto.Month == i).ToList();
                    Session["DaBantc" + i.ToString()] = dabanthangtc == null ? 0 : dabanthangtc.Sum(kh => kh.SoLuong);
                }
            
            return PartialView();
        }
        public ActionResult GetListKyXNTeK(string ngay = "", string strk = "", int idLHXN = 0, int IdSan = 0, int Iddoitra = 0, int PageNo = 0, int PageSize = 8, int UserId = 0)
        {
            strk = strk.ToLower().Trim();
            ViewBag.KyXNTeK = new Areas.Admin.Data.XuatNhapData().getXuatNhapTek(ngay, strk, idLHXN, IdSan, Iddoitra, PageNo, PageSize, UserId);
            return PartialView();
        }
        public ActionResult GetPageCountXNTek(string ngay = "", string strk = "", int idLHXN = 0, int IdSan = 0, int Iddoitra = 0, int PageSize = 8, int UserId = 0)
        {
            var num = new Areas.Admin.Data.XuatNhapData().GetPageCountXuatNhapTek(ngay, strk, idLHXN, IdSan, Iddoitra, UserId);
            var pageCount = Math.Ceiling(1.0 * num / PageSize);
            return Json(pageCount, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadLoaiKyXN()
        {
            var IdLoaiHangXN = dbc.KyXuatNhap_LoaiHang.
                            Select(kh => new { id = kh.Id, ten = kh.TenLoai });

            return Json(IdLoaiHangXN, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadSanTM()
        {
            var IdSan = dbc.SanThuongMais.Where(kh => kh.SuDung == true).
                            Select(kh => new { id = kh.Id, ten = kh.TenSan });

            return Json(IdSan, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListTinhGioCong(DateTime dtInput, int Id = 0)
        {
            double giothang = 0;
            double honbon = 0;
            DateTime date = DateTime.Now;
            List<NV_GioCong> model = new List<NV_GioCong>();
            var nv = dbc.NV_NhanVienTek.Find(Id);
            var nvcong = dbc.NV_Cong.FirstOrDefault(kh => kh.IdNhanVien == Id && kh.Thang == dtInput.Month && kh.Nam == dtInput.Year);
            if (dtInput.Month == date.Month && dtInput.Year == date.Year)
            {
                model = dbc.NV_GioCong.Where(kh => kh.Month == dtInput.Month && kh.Year == dtInput.Year
                                && kh.IdNhanVien == Id && kh.Day <= DateTime.Now.Day)
                        .OrderByDescending(kh => kh.Day)
                        .ToList();
            }
            else
            {
                model = dbc.NV_GioCong.Where(kh => kh.Month == dtInput.Month && kh.Year == dtInput.Year
                                && kh.IdNhanVien == Id)
                        .OrderByDescending(kh => kh.Day)
                        .ToList();
            }
            for (int i = 0; i < model.Count(); i++)
            {
                var kqT = double.Parse((model[i].GioRaSang - model[i].GioVaoSang +
                    (model[i].GioRaChieu - model[i].GioVaoChieu)).TotalHours.ToString("0.00"));
                var kqTC = double.Parse(((model[i].GioRaTangCa - model[i].GioVaoTangCa).TotalHours * float.Parse(TangCa)).ToString("0.00"));
                var kqLe = double.Parse(((model[i].GioRaLe - model[i].GioVaoLe).TotalHours * float.Parse(Le)).ToString("0.00"));
                model[i].GhiChu = (kqT + kqTC + kqLe).ToString();
                kqT = kqT > 0?kqT: 0;
                kqTC = kqTC > 0?kqTC: 0;
                kqLe = kqLe > 0?kqLe: 0;
                giothang = giothang + kqT + kqTC + kqLe;
                var ktbon = kqT + kqTC;
                if (ktbon >= 4)
                {
                    honbon = honbon + 1;
                }
            }
            ViewBag.NgayGioCong = model;
            ViewBag.Hoten = nv.HoTen;
            ViewBag.TongSoSoGioThang = giothang;
            ViewBag.SLcom = honbon;
            
            return PartialView(model);
        }
        public ActionResult GetListTuan(int tuanht = 0)
        {
            //Lấy số tuần hiện tại trong năm :IndexStaff
            DateTime date = DateTime.Now;
            var model = dbc.NV_LichTuanParTime.Where(kh => kh.Year == date.Year
                            && kh.SoTuanTrongNam == tuanht)
                            .OrderByDescending(kh=>kh.NV_NhanVienTek.HoTen)
                            .ToList();
            ViewBag.GetLichTuan = model;
            return PartialView(model);
        }
    }
}