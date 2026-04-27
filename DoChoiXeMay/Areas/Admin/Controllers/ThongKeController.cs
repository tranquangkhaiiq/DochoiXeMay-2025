using DoChoiXeMay.Filters;
using DoChoiXeMay.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Media.Media3D;

namespace DoChoiXeMay.Areas.Admin.Controllers
{
    [Protect]
    public class ThongKeController : Controller
    {
        // GET: Admin/ThongKe
        Model1 dbc = new Model1();
        string DBname = ConfigurationManager.AppSettings["DBname"];
        public ActionResult Index()
        {
            Session["requestUri"] = "/Admin/ThongKe/Index";
            var be = dbc.ChitietXuatNhaps.Where(kh => kh.Ten == "Xi Nhan Wave TNT BLOCKX G2 ZEN 1").ToList();
            var betraLoi = dbc.ChitietXuatNhaps.Where(kh => kh.Ten == "Xi Nhan Wave G2 ZEN 1-NhanBaoHanh"
                                && kh.KyXuatNhap.XuatNhap == true).ToList();
            var beg = be.Where(kh=>kh.KyXuatNhap.XuatNhap == true).ToList();
            var begsanxuat = be.Where(kh => kh.KyXuatNhap.XuatNhap == false && kh.IdDoiTra == 1 && kh.KyXuatNhap.IdKho == 1).ToList();
            var begin = beg.Where(kh=> kh.IdDoiTra == 1).ToList();
            var daban = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 1 &&
                                dbc.Ser_XuatSN_CN.FirstOrDefault(kk => kk.IdKyxuat == kh.IdKy && kk.ChuyenKho == true) == null).ToList();
            
            var DaBanTikTok = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 1 && kh.KyXuatNhap.IdSan == 2 && kh.KyXuatNhap.KhachLe == true).ToList();
            var DaBanShopee = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 1 && kh.KyXuatNhap.IdSan == 3 && kh.KyXuatNhap.KhachLe == true).ToList();
            var DaBanLeNSan = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 1 && kh.KyXuatNhap.IdSan == 1 && kh.KyXuatNhap.KhachLe == true).ToList();
            
            var DaTraHangKhachLe = beg.Where(kh => kh.IdDoiTra == 4).ToList(); //4:Không Lỗi//3:có lỗi//2:Mới Nhận
            var DaTraHangKhachLeTrong = beg.Where(kh => kh.IdDoiTra == 4 && kh.IDColor==5).ToList();
            var DaTraHangKhachLeTrongTikTok = beg.Where(kh => kh.IdDoiTra == 4 && kh.IDColor == 5 && kh.KyXuatNhap.IdSan == 2).ToList();
            var DaTraHangKhachLeTrongShopee = beg.Where(kh => kh.IdDoiTra == 4 && kh.IDColor == 5 && kh.KyXuatNhap.IdSan == 3).ToList();
            var DaTraHangKhachLeTrongNSan = beg.Where(kh => kh.IdDoiTra == 4 && kh.IDColor == 5 && kh.KyXuatNhap.IdSan == 1).ToList();
            var DaTraHangKhachLeKhoi = beg.Where(kh => kh.IdDoiTra == 4 && kh.IDColor == 7).ToList();
            var DaTraHangKhachLeKhoiTikTok = beg.Where(kh => kh.IdDoiTra == 4 && kh.IDColor == 7 && kh.KyXuatNhap.IdSan == 2).ToList();
            var DaTraHangKhachLeKhoiShopee = beg.Where(kh => kh.IdDoiTra == 4 && kh.IDColor == 7 && kh.KyXuatNhap.IdSan == 3).ToList();
            var DaTraHangKhachLeKhoiNSan = beg.Where(kh => kh.IdDoiTra == 4 && kh.IDColor == 7 && kh.KyXuatNhap.IdSan == 1).ToList();

            var DaTraHangKhachLeLoi = be.Where(kh => kh.IdDoiTra == 3).ToList(); //4:Không Lỗi//3:có lỗi//2:Mới Nhận
            var DaTraHangKhachLeLoiTrong = be.Where(kh => kh.IdDoiTra == 3 && kh.IDColor == 5).ToList();
            var DaTraHangKhachLeLoiTrongTikTok = be.Where(kh => kh.IdDoiTra == 3 && kh.IDColor == 5 && kh.KyXuatNhap.IdSan == 2).ToList();
            var DaTraHangKhachLeLoiTrongShopee = be.Where(kh => kh.IdDoiTra == 3 && kh.IDColor == 5 && kh.KyXuatNhap.IdSan == 3).ToList();
            var DaTraHangKhachLeLoiTrongNSan = be.Where(kh => kh.IdDoiTra == 3 && kh.IDColor == 5 && kh.KyXuatNhap.IdSan == 1).ToList();
            var DaTraHangKhachLeLoiKhoi = be.Where(kh => kh.IdDoiTra == 3 && kh.IDColor == 7).ToList();
            var DaTraHangKhachLeLoiKhoiTikTok = be.Where(kh => kh.IdDoiTra == 3 && kh.IDColor == 7 && kh.KyXuatNhap.IdSan == 2).ToList();
            var DaTraHangKhachLeLoiKhoiShopee = be.Where(kh => kh.IdDoiTra == 3 && kh.IDColor == 7 && kh.KyXuatNhap.IdSan == 3).ToList();
            var DaTraHangKhachLeLoiKhoiNSan = be.Where(kh => kh.IdDoiTra == 3 && kh.IDColor == 7 && kh.KyXuatNhap.IdSan == 1).ToList();


            var DaBanLSiNSan = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 1 && kh.KyXuatNhap.IdSan == 1 && kh.KyXuatNhap.KhachLe == false
                    && dbc.Ser_XuatSN_CN.FirstOrDefault(kk => kk.IdKyxuat == kh.IdKy && kk.ChuyenKho == true) == null).ToList();

            var TrongDaBan = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 1 && kh.IDColor == 5).ToList();
            var TrongDaBanTikTok = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 1 && kh.IDColor == 5 && kh.KyXuatNhap.IdSan == 2).ToList();
            var trongDaBanShopee = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 1 && kh.IDColor == 5 && kh.KyXuatNhap.IdSan == 3).ToList();
            var trongDaBanLeNSan = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 1 && kh.IDColor == 5 && kh.KyXuatNhap.IdSan == 1 && kh.KyXuatNhap.KhachLe == true).ToList();
            var trongDaBanLSiNSan = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 1 && kh.IDColor == 5 && kh.KyXuatNhap.IdSan == 1 && kh.KyXuatNhap.KhachLe == false
                            && dbc.Ser_XuatSN_CN.FirstOrDefault(kk => kk.IdKyxuat == kh.IdKy && kk.ChuyenKho == true) == null).ToList();
            
            var KhoiDaBan = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 1 && kh.IDColor == 7).ToList();
            var KhoiDaBanTikTok = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 1 && kh.IDColor == 7 && kh.KyXuatNhap.IdSan == 2).ToList();
            var KhoiDaBanShopee = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 1 && kh.IDColor == 7 && kh.KyXuatNhap.IdSan == 3).ToList();
            var KhoiDaBanLeNSan = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 1 && kh.IDColor == 7 && kh.KyXuatNhap.IdSan == 1 && kh.KyXuatNhap.KhachLe == true).ToList();
            var KhoiDaBanLSiNSan = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 1 && kh.IDColor == 7 && kh.KyXuatNhap.IdSan == 1 && kh.KyXuatNhap.KhachLe == false
                            && dbc.Ser_XuatSN_CN.FirstOrDefault(kk => kk.IdKyxuat == kh.IdKy && kk.ChuyenKho == true) == null).ToList();

            var MauDaXuat = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 2).ToList();
            var KhoiMauDaXuat = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 2 && kh.IDColor == 7).ToList();
            var TrongMauDaXuat = begin.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 2 && kh.IDColor == 5).ToList();

            var Tonkho = dbc.HangHoas.Where(kh => kh.Id == 55 || kh.Id == 56 || kh.Id == 1066 || kh.Id == 66).Sum(kh => kh.SoLuong);
            ViewBag.TongXiNhanGen1Tek = Tonkho;
            var TonkhoT= dbc.HangHoas.Where(kh => kh.Id == 56 || kh.Id == 1066).Sum(kh => kh.SoLuong);
            ViewBag.TongXiNhanGen1TrongTK = TonkhoT;
            var TonkhoK = dbc.HangHoas.Where(kh => kh.Id == 55 || kh.Id == 66).Sum(kh => kh.SoLuong);
            ViewBag.TongXiNhanGen1KhoiTK = TonkhoK;

            var modeldaban = daban == null ? 0 : daban.Sum(kh => kh.SoLuong);
            ViewBag.daban = modeldaban;
            var modelDaBanTikTok = DaBanTikTok == null ? 0 : DaBanTikTok.Sum(kh => kh.SoLuong);
            ViewBag.DaBanTikTok = modelDaBanTikTok;
            var modelDaBanShopee = DaBanShopee == null ? 0 : DaBanShopee.Sum(kh => kh.SoLuong);
            ViewBag.DaBanShopee = modelDaBanShopee;
            var modelDaBanLeNSan = DaBanLeNSan == null ? 0 : DaBanLeNSan.Sum(kh => kh.SoLuong);
            ViewBag.DaBanLeNSan = modelDaBanLeNSan;
            var modelDaBanLSiNSan = DaBanLSiNSan == null ? 0 : DaBanLSiNSan.Sum(kh => kh.SoLuong);
            ViewBag.DaBanLSiNSan = modelDaBanLSiNSan;

            

            ViewBag.DaTraHangKhachLe = DaTraHangKhachLe == null ? 0 : DaTraHangKhachLe.Sum(kh => kh.SoLuong);
            ViewBag.DaTraHangKhachLeTrong = DaTraHangKhachLeTrong == null ? 0 : DaTraHangKhachLeTrong.Sum(kh => kh.SoLuong);
            ViewBag.DaTraHangKhachLeTrongTikTok = DaTraHangKhachLeTrongTikTok == null ? 0 : DaTraHangKhachLeTrongTikTok.Sum(kh => kh.SoLuong);
            ViewBag.DaTraHangKhachLeTrongShopee = DaTraHangKhachLeTrongShopee == null ? 0 : DaTraHangKhachLeTrongShopee.Sum(kh => kh.SoLuong);
            ViewBag.DaTraHangKhachLeTrongNSan = DaTraHangKhachLeTrongNSan == null ? 0 : DaTraHangKhachLeTrongNSan.Sum(kh => kh.SoLuong);
            //Lỗi
            var TraHangLeLoi = DaTraHangKhachLeLoi == null ? 0 : DaTraHangKhachLeLoi.Sum(kh => kh.SoLuong);
            ViewBag.DaTraHangKhachLeLoi = TraHangLeLoi;
            //Lỗi trong
            ViewBag.DaTraHangKhachLeLoiTrong = DaTraHangKhachLeLoiTrong == null ? 0 : DaTraHangKhachLeLoiTrong.Sum(kh => kh.SoLuong);
            ViewBag.DaTraHangKhachLeLoiTrongTikTok = DaTraHangKhachLeLoiTrongTikTok == null ? 0 : DaTraHangKhachLeLoiTrongTikTok.Sum(kh => kh.SoLuong);
            ViewBag.DaTraHangKhachLeLoiTrongShopee = DaTraHangKhachLeLoiTrongShopee == null ? 0 : DaTraHangKhachLeLoiTrongShopee.Sum(kh => kh.SoLuong);
            ViewBag.DaTraHangKhachLeLoiTrongNSan = DaTraHangKhachLeLoiTrongNSan == null ? 0 : DaTraHangKhachLeLoiTrongNSan.Sum(kh => kh.SoLuong);

            ViewBag.DaTraHangKhachLeKhoi = DaTraHangKhachLeKhoi == null ? 0 : DaTraHangKhachLeKhoi.Sum(kh => kh.SoLuong);
            ViewBag.DaTraHangKhachLeKhoiTikTok = DaTraHangKhachLeKhoiTikTok == null ? 0 : DaTraHangKhachLeKhoiTikTok.Sum(kh => kh.SoLuong);
            ViewBag.DaTraHangKhachLeKhoiShopee = DaTraHangKhachLeKhoiShopee == null ? 0 : DaTraHangKhachLeKhoiShopee.Sum(kh => kh.SoLuong);
            ViewBag.DaTraHangKhachLeKhoiNSan = DaTraHangKhachLeKhoiNSan == null ? 0 : DaTraHangKhachLeKhoiNSan.Sum(kh => kh.SoLuong);
            //Lỗi Khói
            ViewBag.DaTraHangKhachLeLoiKhoi = DaTraHangKhachLeLoiKhoi == null ? 0 : DaTraHangKhachLeLoiKhoi.Sum(kh => kh.SoLuong);
            ViewBag.DaTraHangKhachLeLoiKhoiTikTok = DaTraHangKhachLeLoiKhoiTikTok == null ? 0 : DaTraHangKhachLeLoiKhoiTikTok.Sum(kh => kh.SoLuong);
            ViewBag.DaTraHangKhachLeLoiKhoiShopee = DaTraHangKhachLeLoiKhoiShopee == null ? 0 : DaTraHangKhachLeLoiKhoiShopee.Sum(kh => kh.SoLuong);
            ViewBag.DaTraHangKhachLeLoiKhoiNSan = DaTraHangKhachLeLoiKhoiNSan == null ? 0 : DaTraHangKhachLeLoiKhoiNSan.Sum(kh => kh.SoLuong);
            var modelTrongDaBan = TrongDaBan == null ? 0 : TrongDaBan.Sum(kh => kh.SoLuong);
            ViewBag.TongXiNhanGen1TrongDaBan = modelTrongDaBan;

            //27thang11 trong
            var modelTrongDaBanTikTok= TrongDaBanTikTok == null ? 0 : TrongDaBanTikTok.Sum(kh => kh.SoLuong);
            ViewBag.TongXiNhanGen1TrongDaBanTikTok = modelTrongDaBanTikTok;
            var modeltrongDaBanShopee = trongDaBanShopee == null ? 0 : trongDaBanShopee.Sum(kh => kh.SoLuong);
            ViewBag.TongXiNhanGen1trongDaBanShopee = modeltrongDaBanShopee;
            var modeltrongDaBanLeNSan= trongDaBanLeNSan == null ? 0 : trongDaBanLeNSan.Sum(kh => kh.SoLuong);
            ViewBag.TongXiNhanGen1trongDaBanLeNSan = modeltrongDaBanLeNSan;
            var modeltrongDaBanLSiNSan= trongDaBanLSiNSan == null ? 0 : trongDaBanLSiNSan.Sum(kh => kh.SoLuong);
            ViewBag.TongXiNhanGen1trongDaBanSiNSan = modeltrongDaBanLSiNSan;

            //27thang11 Khói
            var modelKhoiDaBan = KhoiDaBan == null ? 0 : KhoiDaBan.Sum(kh => kh.SoLuong);
            ViewBag.TongXiNhanGen1KhoiDaBan = modelKhoiDaBan;
            var modelKhoiDaBanTikTok= KhoiDaBanTikTok == null ? 0 : KhoiDaBanTikTok.Sum(kh => kh.SoLuong);
            ViewBag.TongXiNhanGen1KhoiDaBanTikTok = modelKhoiDaBanTikTok;
            var modelKhoiDaBanShopee = KhoiDaBanShopee == null ? 0 : KhoiDaBanShopee.Sum(kh => kh.SoLuong);
            ViewBag.TongXiNhanGen1KhoiDaBanShopee = modelKhoiDaBanShopee;
            var modelKhoiDaBanLSiNSan= KhoiDaBanLSiNSan == null ? 0 : KhoiDaBanLSiNSan.Sum(kh => kh.SoLuong);
            ViewBag.TongXiNhanGen1KhoiDaBanSiNSan = modelKhoiDaBanLSiNSan;
            var modelKhoiDaBanLeNSan= KhoiDaBanLeNSan == null ? 0 : KhoiDaBanLeNSan.Sum(kh => kh.SoLuong);
            ViewBag.TongXiNhanGen1KhoiDaBanLeNSan = modelKhoiDaBanLeNSan;
            //27 tháng 11 ************
            var modelMauDaXuat = MauDaXuat == null ? 0 : MauDaXuat.Sum(kh => kh.SoLuong);
            ViewBag.TongXiNhanGen1MauDaXuat = modelMauDaXuat;
            ViewBag.TongXiNhanGen1KhoiMauDaXuat = KhoiMauDaXuat == null ? 0 : KhoiMauDaXuat.Sum(kh => kh.SoLuong);
            ViewBag.TongXiNhanGen1TrongMauDaXuat = TrongMauDaXuat == null ? 0 : TrongMauDaXuat.Sum(kh => kh.SoLuong);
            
            var kytrabaohanhct = beg.Where(kh=>kh.KyXuatNhap.IdLoaiHangXN == 3 || kh.KyXuatNhap.IdLoaiHangXN == 4).ToList();
            if (kytrabaohanhct.Count() == 0)
            {
                ViewBag.TongtraBH = 0;
            }
            else
            {
                ViewBag.TongtraBH = kytrabaohanhct.Sum(kh=>kh.SoLuong);
            }
            //27/4/2026 => update dasanxuat 
            var traNoBox = beg.Where(kh => kh.KyXuatNhap.IdLoaiHangXN == 4).Sum(kh => kh.SoLuong);
            var trahangnhanBH = betraLoi.Sum(kh => kh.SoLuong);
            //27/4/2026 => update dasanxuat
            var dsx = begsanxuat.Sum(kh => kh.SoLuong);
            ViewBag.DaSanXuat=dsx + traNoBox + trahangnhanBH;
            //Vẽ đồ thị pie
            Session["daban"] = modeldaban;
            Session["DaTraHangKhachLeLoi"] = TraHangLeLoi;
            Session["TongtraBH"] = kytrabaohanhct.Count() == 0 ? 0 : kytrabaohanhct.Sum(kh => kh.SoLuong);
            //Session["TonKhoXiNhanGen1Tek"] = Tonkho;
            Session["TonKhoXiNhanGen1TekT"] = TonkhoT;
            Session["TonKhoXiNhanGen1TekK"] = TonkhoK;
            Session["MauDaXuat"] = modelMauDaXuat;
            Session["daSX"] = dsx;
            //Đồ thị dạng cột
            Session["TrongDaBanTikTok"] = modelTrongDaBanTikTok;
            Session["trongDaBanShopee"] = modeltrongDaBanShopee;
            Session["trongDaBanLeNSan"] = modeltrongDaBanLeNSan;
            Session["trongDaBanLSiNSan"] = modeltrongDaBanLSiNSan;
            Session["KhoiDaBanTikTok"] = modelKhoiDaBanTikTok;
            Session["KhoiDaBanShopee"] = modelKhoiDaBanShopee;
            Session["KhoiDaBanLSiNSan"] = modelKhoiDaBanLSiNSan;
            Session["KhoiDaBanLeNSan"] = modelKhoiDaBanLeNSan;

            Session["DaBanTikTok"] = modelDaBanTikTok;
            Session["DaBanShopee"] = modelDaBanShopee;
            Session["DaBanLeNSan"] = modelDaBanLeNSan;
            Session["DaBanSiNSan"] = modelDaBanLSiNSan;
            Session["TrongDaBan"] = modelTrongDaBan;
            Session["KhoiDaBan"] = modelKhoiDaBan;
            return View();
        }
        public ActionResult DoThiThongKe()
        {
            ViewBag.phantramdaban = (100* float.Parse(Session["daban"].ToString())/ float.Parse(Session["daSX"].ToString())).ToString("#0.00");
            ViewBag.phantramTraHangLeLoi = (100 * float.Parse(Session["DaTraHangKhachLeLoi"].ToString()) / float.Parse(Session["daSX"].ToString())).ToString("#0.00");
            ViewBag.phantramTongtraBH = (100 * float.Parse(Session["TongtraBH"].ToString()) / float.Parse(Session["daSX"].ToString())).ToString("#0.00");
            //ViewBag.phantramTonkho = (100 * float.Parse(Session["TonKhoXiNhanGen1Tek"].ToString()) / float.Parse(Session["daSX"].ToString())).ToString("#0.00");
            ViewBag.phantramTonkhoT = (100 * float.Parse(Session["TonKhoXiNhanGen1TekT"].ToString()) / float.Parse(Session["daSX"].ToString())).ToString("#0.00");
            ViewBag.phantramTonkhoK = (100 * float.Parse(Session["TonKhoXiNhanGen1TekK"].ToString()) / float.Parse(Session["daSX"].ToString())).ToString("#0.00");
            ViewBag.phantramMauDaXuat = (100 * float.Parse(Session["MauDaXuat"].ToString()) / float.Parse(Session["daSX"].ToString())).ToString("#0.00");

            return PartialView();
        }
        public ActionResult NVLTonKho()
        {
            //IDKy==1 => NVL
            var model = dbc.HangHoas.Where(kh => kh.IDKy == 1 && kh.IDMF == 5).ToList();
            ViewBag.NVLTonKho = model;
            return PartialView(model);
        }
        public ActionResult GetListKyTonKho()
        {
            
            List<KyTonKho> model = new List<KyTonKho>();
            model = dbc.KyTonKhoes.Where(kh=>kh.Id>1).OrderBy(kh => kh.Id).ToList();
            for (int i = 0; i < model.Count(); i++)
            {
                model[i].STT = (i + 1).ToString();
            }
            ViewBag.KyTonKho = model.OrderByDescending(kh => kh.Id).ToList();
            return PartialView();
        }
        public ActionResult InsertKyTonKho()
        {
            try
            {
                KyTonKho model = new KyTonKho();
                model.TenKy = "Kỳ Auto";
                model.LuuKho = "Auto Kho Số 1";
                model.GhiChu = "";
                model.NgayTao =DateTime.Now;
                model.SuDung = false;
                model.HoanThanh = false;
                dbc.KyTonKhoes.Add(model);
                dbc.SaveChanges();
                Session["ThongBaoKyTonKhoOK"] = "Auto thêm mới kỳ tồn kho thành công, cần update để sử dụng.";
                //tro lai trang truoc do 
                var requestUri = Session["requestUri"] as string;
                if (requestUri != null)
                {
                    return Redirect(requestUri);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Session["ThongBaoKyTonKhoLoi"] = "Auto thêm mới bị Lỗi: "+ex.Message;
                string message = ex.Message;
                return RedirectToAction("Index");
            }
        }
        public ActionResult InsertChiTietKyTonKho(int id)
        {
            try
            {
                var kq = new Data.TonKhoData().InsertTonKhoAotu(id,"Auto Kỳ",0,DBname);
                if (kq)
                {
                    Session["ThongBaoKyTonKhoOK"] = "Auto thêm mới CT kỳ tồn kho thành công, cần update để sử dụng.";

                }
                //tro lai trang truoc do 
                var requestUri = Session["requestUri"] as string;
                if (requestUri != null)
                {
                    return Redirect(requestUri);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Session["ThongBaoKyTonKhoLoi"] = "Auto thêm mới Chi Tiết Kỳ bị Lỗi: " + ex.Message;
                string message = ex.Message;
                return RedirectToAction("Index");
            }
        }
        public ActionResult DeleteKyTonKho(int id)
        {
            try
            {
                var model = dbc.KyTonKhoes.Find(id);
                if (model != null)
                {
                    dbc.KyTonKhoes.Remove(model);
                    dbc.SaveChanges();
                    Session["ThongBaoKyTonKhoOK"] = "Delete thành công Kỳ Tồn Kho: " + model.NgayTao.ToString("{dd/MM/yyyy}");
                }
            }
            catch (Exception ex)
            {
                var loi = ex.Message;
                Session["ThongBaoKyTonKhoLoi"] = "Có Lỗi Delete Kỳ Tồn Kho : "+loi;
            }
            return RedirectToAction("Index");
        }
        public ActionResult DeleteCTKyTonKho(int id)
        {
            try
            {
                var model = dbc.ChiTietTonKhoes.Find(id);
                if (model != null)
                {
                    dbc.ChiTietTonKhoes.Remove(model);
                    dbc.SaveChanges();
                    Session["ThongBaoKyTonKhoOK"] = "Delete thành công "+model.TenHang + model.NgayTao.ToString("{dd/MM/yyyy}");
                }
            }
            catch (Exception ex)
            {
                var loi = ex.Message;
                Session["ThongBaoKyTonKhoLoi"] = "Có Lỗi Delete : " + loi;
            }
            return RedirectToAction("Index");
        }
        public ActionResult UpdateKyTonKho(int id)
        {
            var model = dbc.KyTonKhoes.Find(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult UpdateKyTonKho(KyTonKho modelTK)
        {
            try
            {
                KyTonKho model = new KyTonKho();
                model = modelTK;
                dbc.Entry(model).State = EntityState.Modified;
                dbc.SaveChanges();
                Session["ThongBaoKyTonKhoOK"] = "Update thành công Kỳ Tồn Kho " + model.TenKy;
                //tro lai trang truoc do 
                var requestUri = Session["requestUri"] as string;
                if (requestUri != null)
                {
                    return Redirect(requestUri);
                }
            }
            catch (Exception ex)
            {
                var loi = ex.Message;
                ModelState.AddModelError("", "Có Lỗi Update !!!!"+loi);
                var model = dbc.KyTonKhoes.Find(modelTK.Id);
                return View(model);
            }
            return RedirectToAction("Index");
        }
        public ActionResult UpdateCTTK(int id)
        {
            var model = dbc.ChiTietTonKhoes.Find(id);
            ViewBag.IDMF = new SelectList(dbc.Manufacturers.Where(kh => kh.Sudung == true), "Id", "Name", model.IDMF);
            ViewBag.IDColor = new SelectList(dbc.Colors.OrderByDescending(kh => kh.Id), "Id", "TenColor",model.IDColor);
            ViewBag.IDSize = new SelectList(dbc.Sizes.OrderBy(kh => kh.Id), "Id", "TenSize",model.IDSize);
            ViewBag.IDCap = new SelectList(dbc.TonKhoCaps.OrderBy(kh => kh.Id), "Id", "Ten", model.IDCap);
            return View(model);
        }
        [HttpPost]
        public ActionResult UpdateCTTK(ChiTietTonKho CTTK)
        {
            try
            {
                ChiTietTonKho model = new ChiTietTonKho();
                model = CTTK;
                model.NgayUpdate = DateTime.Now;
                model.ChuaRap = model.TonDauKy - model.DaRap - model.CoLoi;
                dbc.Entry(model).State = EntityState.Modified;
                dbc.SaveChanges();
                var getcoutNVL = dbc.ChiTietTonKhoes.FirstOrDefault(kh => kh.ParentId == model.Id);
                if (model.SanPham ==true && getcoutNVL==null)
                {
                    new Data.TonKhoData().InsertTonKhoAotu(model.IdKyTonKho, "Đế Sáng phải", model.Id, DBname);
                    new Data.TonKhoData().InsertTonKhoAotu(model.IdKyTonKho, "Đế Sáng trái", model.Id, DBname);
                    new Data.TonKhoData().InsertTonKhoAotu(model.IdKyTonKho, "Đế đen và mạch điện", model.Id, DBname);
                    new Data.TonKhoData().InsertTonKhoAotu(model.IdKyTonKho, "Nanh Sáng ngắn Xi Nhan Wave", model.Id, DBname);
                    new Data.TonKhoData().InsertTonKhoAotu(model.IdKyTonKho, "Thanh Sáng dài Xi Nhan Wave phải", model.Id, DBname);
                    new Data.TonKhoData().InsertTonKhoAotu(model.IdKyTonKho, "Thanh Sáng dài Xi Nhan Wave trái", model.Id, DBname);
                    new Data.TonKhoData().InsertTonKhoAotu(model.IdKyTonKho, "Kính trong/khói Xi nhan wave", model.Id, DBname);
                }
                if (model.ParentId == 0)
                {
                    var chitietNVL = dbc.ChiTietTonKhoes.Where(kh => kh.ParentId == model.Id).ToList();
                    if (chitietNVL.Count() > 0)
                    {
                        foreach (var item in chitietNVL)
                        {
                            item.TonDauKy = model.TonDauKy * item.TonKhoCap.Cap;
                            item.DaRap = model.DaRap * item.TonKhoCap.Cap;
                            item.ChuaRap = item.TonDauKy - item.DaRap - item.CoLoi;
                            item.NgayUpdate = DateTime.Now;
                            var update = dbc.Database.ExecuteSqlCommand("update [" + DBname + "TechZone].[dbo].[ChiTietTonKho] set " +
                            "IdKyTonKho=@IdKyTonKho,TenHang=@TenHang,TonDauKy=@TonDauKy," +
                            "DaRap=@DaRap,CoLoi=@CoLoi,ChuaRap=@ChuaRap,NgayTao=@NgayTao,NgayUpdate=@NgayUpdate," +
                            "SanPham=@SanPham,GhiChu=@GhiChu,STT=@STT,ParentId=@ParentId,IDMF=@IDMF," +
                            "IDColor=@IDColor,IDSize=@IDSize,IDCap=@IDCap " +
                            "where Id=@Id",
                            new SqlParameter("@IdKyTonKho", item.IdKyTonKho),
                            new SqlParameter("@TenHang", item.TenHang),
                            new SqlParameter("@TonDauKy", item.TonDauKy),
                            new SqlParameter("@DaRap", item.DaRap),
                            new SqlParameter("@CoLoi", item.CoLoi),
                            new SqlParameter("@ChuaRap", item.ChuaRap),
                            new SqlParameter("@NgayTao", item.NgayTao),
                            new SqlParameter("@NgayUpdate", item.NgayUpdate),
                            new SqlParameter("@SanPham", item.SanPham),
                            new SqlParameter("@GhiChu", ""),
                            new SqlParameter("@STT", ""),
                            new SqlParameter("@ParentId", item.ParentId),
                            new SqlParameter("@IDMF", item.IDMF),
                            new SqlParameter("@IDColor", item.IDColor),
                            new SqlParameter("@IDSize", item.IDSize),
                            new SqlParameter("@IDCap", item.IDCap),
                            new SqlParameter("@Id", item.Id));
                        }
                    }
                }
                
                Session["ThongBaoKyTonKhoOK"] = "Update thành công Chi Tiết SP/NVL " + model.TenHang;
                //tro lai trang truoc do 
                var requestUri = Session["requestUri"] as string;
                if (requestUri != null)
                {
                    return Redirect(requestUri);
                }
            }
            catch (Exception ex)
            {
                var loi = ex.Message;
                ModelState.AddModelError("", "Có Lỗi Update !!!!" + loi);
                var model = dbc.ChiTietTonKhoes.Find(CTTK.Id);
                ViewBag.IDMF = new SelectList(dbc.Manufacturers.Where(kh => kh.Sudung == true), "Id", "Name", model.IDMF);
                ViewBag.IDColor = new SelectList(dbc.Colors.OrderByDescending(kh => kh.Id), "Id", "TenColor", model.IDColor);
                ViewBag.IDSize = new SelectList(dbc.Sizes.OrderBy(kh => kh.Id), "Id", "TenSize", model.IDSize);
                ViewBag.IDCap = new SelectList(dbc.TonKhoCaps.OrderBy(kh => kh.Id), "Id", "Ten", model.IDCap);
                return View(model);
            }
            return RedirectToAction("Index");
        }
    }
}