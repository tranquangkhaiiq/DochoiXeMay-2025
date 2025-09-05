using DoChoiXeMay.Areas.Admin.Data;
using DoChoiXeMay.Filters;
using DoChoiXeMay.Models;
using DoChoiXeMay.Utils;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace DoChoiXeMay.Areas.Admin.Controllers
{
    [Protect]
    public class ThuChiController : Controller
    {
        // GET: Admin/ThuChi
        Model1 dbc = new Model1();
        string DBname = ConfigurationManager.AppSettings["DBname"];
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListThuChiTeK()
        {
            Session["requestUri"] = "/Admin/ThuChi/ListThuChiTeK";
            return View();
        }
        public ActionResult GetListThuChiTek(string tu, string den,string TC,string TNO,string strk, int PageNo = 0, int PageSize = 8)
        {
            strk = strk.ToLower().Trim();
            List<ChiTietTC> modelTong = new List<ChiTietTC>();
            modelTong = Data.ThuChiData.ChiTietTCDBTEK(dbc, strk, TC,TNO, tu, den);
            var model = new Data.ThuChiData().getThuChiTek(PageNo, PageSize, strk, TC,TNO, tu, den);

            //var uid = int.Parse(Session["UserId"].ToString());
            ViewBag.ChitietTCTEK = model;

            double ThuTK = new Data.ThuChiData().TongbyHTvaThuChi(modelTong, 1, true);
            ViewBag.ThuTK = String.Format(new CultureInfo("vi-VN"), "{0:#,##0}", ThuTK);
            double ChiTK = new Data.ThuChiData().TongbyHTvaThuChi(modelTong, 1, false);
            ViewBag.ChiTK = String.Format(new CultureInfo("vi-VN"), "{0:#,##0}", ChiTK);

            double ThuTienMat = new Data.ThuChiData().TongbyHTvaThuChi(modelTong, 2, true);
            ViewBag.ThuTienMat = String.Format(new CultureInfo("vi-VN"), "{0:#,##0}", ThuTienMat);
            double ChiTienMat = new Data.ThuChiData().TongbyHTvaThuChi(modelTong, 2, false);
            ViewBag.ChiTienMat = String.Format(new CultureInfo("vi-VN"), "{0:#,##0}", ChiTienMat);

            double ThuTKVCB = new Data.ThuChiData().TongbyHTvaThuChi(modelTong, 4, true);
            ViewBag.ThuTKVCB = String.Format(new CultureInfo("vi-VN"), "{0:#,##0}", ThuTKVCB);
            double ChiTKVCB = new Data.ThuChiData().TongbyHTvaThuChi(modelTong, 4, false);
            ViewBag.ChiTKVCB = String.Format(new CultureInfo("vi-VN"), "{0:#,##0}", ChiTKVCB);
            double NoPhaiThu = new Data.ThuChiData().TongbyIndebtedvaThuChi(modelTong, true, true);
            ViewBag.NoPhaiThu = String.Format(new CultureInfo("vi-VN"), "{0:#,##0}", NoPhaiThu);
            double NoPhaiTra = new Data.ThuChiData().TongbyIndebtedvaThuChi(modelTong, true, false);
            ViewBag.NoPhaiTra = String.Format(new CultureInfo("vi-VN"), "{0:#,##0}", NoPhaiTra);

            double conlaiTK = ThuTK - ChiTK;
            ViewBag.conlaiTK = String.Format(new CultureInfo("vi-VN"), "{0:#,##0}", conlaiTK);
            double conlaiTienmat = ThuTienMat - ChiTienMat;
            ViewBag.conlaiTienmat = String.Format(new CultureInfo("vi-VN"), "{0:#,##0}", conlaiTienmat);
            double conlaiTKVCB = ThuTKVCB - ChiTKVCB;
            ViewBag.conlaiTKVCB = String.Format(new CultureInfo("vi-VN"), "{0:#,##0}", conlaiTKVCB);
            double TTconlai = conlaiTK + conlaiTienmat + conlaiTKVCB;
            ViewBag.TTconlai = String.Format(new CultureInfo("vi-VN"), "{0:#,##0}", TTconlai);

            return PartialView();
        }
        public ActionResult GetPageCountThuChiTek(string tu,string den,string TC,string TNO,string Keyword, int PageSize = 8)
        {
            var num = new Data.ThuChiData().GetPageCountThuChiTek(Keyword,TC,TNO,tu,den);
            var pageCount = Math.Ceiling(1.0 * num / PageSize);
            return Json(pageCount, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult InsertThuChi()
        {
            var model = new ChiTietTC();
            ViewBag.IdMa = new SelectList(dbc.MaTCs.Where(kh => kh.SuDung == true), "Id", "GhiChu");

            ViewBag.IdHT = new SelectList(dbc.HinhThucTCs.Where(kh => kh.SuDung == true), "Id", "TenHT");

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult InsertThuChikh(ChiTietTC TC)
        {
            var uid = int.Parse(Session["UserId"].ToString());
            var quyen = Session["quyen"].ToString();
            var username = Session["UserName"].ToString();
            ViewBag.IdHT = new SelectList(dbc.HinhThucTCs.Where(kh => kh.SuDung == true), "Id", "TenHT");
            ViewBag.IdMa = new SelectList(dbc.MaTCs.Where(kh => kh.SuDung == true), "Id", "TenMa");
            try
            {
                var file1 = Request.Files["Filesave1"];
                var file2 = Request.Files["Filesave2"];
                var file3 = Request.Files["HoaDon"];
                var ten1 = XstringAdmin.saveFile(file1, "imgthuchi/");
                var ten2 = XstringAdmin.saveFile(file2, "imgthuchi/");
                var ten3 = XstringAdmin.saveFile(file3, "imgthuchi/");
                TC.Filesave1 = ten1;
                TC.Filesave2 = ten2;
                TC.HoaDon = ten3;
                TC.UserId = uid;
                TC.IdKyxuatnhap = 1;
                var kq = new Data.ThuChiData().InsertThuChiTeK(TC, quyen, username);
                if (kq == true)
                {
                    Session["ThongBaoThuChiTEK"] = "Admin Insert thanh cong Thu Chi ngay: " + TC.NgayTC.ToString("{dd/MM/yyyy}");
                    //tro lai trang truoc do 
                    var requestUri = Session["requestUri"] as string;
                    if (requestUri != null)
                    {
                        return Redirect(requestUri);
                    }
                    return RedirectToAction("ListThuChiTeK");
                }
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                ModelState.AddModelError("", "Thêm mới Thất Bại !!!!!!!!!!. Có lỗi hệ thống");
                return View("InsertThuChi");
            }
            return RedirectToAction("Index");
        }
        public ActionResult getTongTien(int Id)
        {
            var tong = new XuatNhapData().getTongTienAuto(Id);
            return Json(tong, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult UpdateCTthuchi(string Id)
        {
            //không cho update KyXuatNhap
            var model = dbc.ChiTietTCs.Find(new Guid(Id));
            ViewBag.IdMa = new SelectList(dbc.MaTCs.Where(kh => kh.SuDung == true), "Id", "GhiChu",model.IdMa);

            ViewBag.IdHT = new SelectList(dbc.HinhThucTCs.Where(kh => kh.SuDung == true), "Id", "TenHT",model.IdHT);
            ViewBag.IdKyxuatnhap = new SelectList(dbc.KyXuatNhaps.Where(kh=>kh.AdminXNPUSH==true && kh.UYeuCauThuHoi==false),"Id","TenKy",model.IdKyxuatnhap);
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateCTthuchi(ChiTietTC TC)
        {
            try
            {
                var ngaythongbao = TC.NgayTC.ToString("{dd/MM/yyyy}");
                var file1 = Request.Files["Dinhkem1"];
                var file2 = Request.Files["Dinhkem2"];
                var file3 = Request.Files["Dinhkem3"];
                if (TC.IdKyxuatnhap == 1)
                {
                    if (file1.ContentLength > 0)
                    {
                        //Xoa hinh cu
                        bool xoahinhcu = XstringAdmin.Xoahinhcu("imgthuchi/", TC.Filesave1);
                        TC.Filesave1 = XstringAdmin.saveFile(file1, "imgthuchi/");
                    }
                    if (file2.ContentLength > 0)
                    {
                        //Xoa hinh cu
                        bool xoahinhcu = XstringAdmin.Xoahinhcu("imgthuchi/", TC.Filesave2);
                        TC.Filesave2 = XstringAdmin.saveFile(file2, "imgthuchi/");
                    }
                    if (file3.ContentLength > 0)
                    {
                        //Xoa hinh cu
                        bool xoahinhcu = XstringAdmin.Xoahinhcu("imgthuchi/", TC.HoaDon);
                        TC.HoaDon = XstringAdmin.saveFile(file3, "imgthuchi/");
                    }
                }
                else
                {
                    if (file1.ContentLength > 0)
                    {
                        //Không Xoa hinh cu
                        TC.Filesave1 = XstringAdmin.saveFile(file1, "imgxuatnhap/");
                    }
                    if (file2.ContentLength > 0)
                    {
                        //Không Xoa hinh cu
                        TC.Filesave2 = XstringAdmin.saveFile(file2, "imgxuatnhap/");
                    }
                    if (file3.ContentLength > 0)
                    {
                        //Không Xoa hinh cu
                        TC.HoaDon = XstringAdmin.saveFile(file3, "imgxuatnhap/");
                    }
                }
                TC.NgayAuto = DateTime.Now;
                var kq=new Data.ThuChiData().UPdateChiTietTC(TC);
                if (kq == true)
                {
                    var userid = int.Parse(Session["UserId"].ToString());
                    Session["ThongBaoThuChiTEK"] = "Update thanh cong Thu Chi ngay: " + ngaythongbao;
                    var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, userid, Session["quyen"].ToString()
                        , Session["UserName"].ToString(), "UpdateThuChi-" + ngaythongbao, "");
                    //tro lai trang truoc do 
                    var requestUri = Session["requestUri"] as string;
                    if (requestUri != null)
                    {
                        return Redirect(requestUri);
                    }
                    return RedirectToAction("ListThuChiTeK");
                }
                else
                {
                    ModelState.AddModelError("", "Update Thu Chi Thất Bại !!!!");
                    var model = dbc.ChiTietTCs.Find(TC.Id);
                    ViewBag.IdMa = new SelectList(dbc.MaTCs.Where(kh => kh.SuDung == true), "Id", "GhiChu", model.IdMa);
                    ViewBag.IdHT = new SelectList(dbc.HinhThucTCs.Where(kh => kh.SuDung == true), "Id", "TenHT", model.IdHT);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                ModelState.AddModelError("", "Update Thu Chi Thất Bại !!!!");
                var model = dbc.ChiTietTCs.Find(TC.Id);
                ViewBag.IdMa = new SelectList(dbc.MaTCs.Where(kh => kh.SuDung == true), "Id", "GhiChu", model.IdMa);
                ViewBag.IdHT = new SelectList(dbc.HinhThucTCs.Where(kh => kh.SuDung == true), "Id", "TenHT", model.IdHT);
                return View(model);
            }
            
        }
        public ActionResult DeleteThuChi(string Id)
        {
            var iiii = new Guid(Id);
            var model = dbc.ChiTietTCs.Find(iiii);
            if (model != null)
            {
                try
                {
                    //Xóa hình thu chi củ
                    var file1 = model.Filesave1;
                    var file2 = model.Filesave2;
                    var file3 = model.HoaDon;
                    bool xoa1 = XstringAdmin.Xoahinhcu("imgthuchi/", file1);
                    bool xoa2 = XstringAdmin.Xoahinhcu("imgthuchi/", file2);
                    bool xoa3 = XstringAdmin.Xoahinhcu("imgthuchi/", file3);
                    //Xóa hình thu chi củ
                    var ngay = model.NgayTC;
                    dbc.ChiTietTCs.Remove(model);
                    dbc.SaveChanges();
                    Session["ThongBaoThuChiTEK"] = "Delete thành công file thu chi ngày: " + ngay.ToString("{dd/MM/yyyy}");
                }
                catch (Exception ex)
                {
                    var loi = ex.Message;
                }
            }
            return RedirectToAction("ListThuChiTeK");
        }
        public ActionResult LoadOneXuatNhap(int Id)
        {
            ViewBag.OneXN = dbc.KyXuatNhaps.Where(kh=>kh.Id==Id).ToList();
            var model = dbc.ChitietXuatNhaps.Where(kh => kh.IdKy == Id)
                .OrderByDescending(kh=>kh.NgayAuto).ToList();
            for (int i = 0; i < model.Count(); i++)
            {
                model[i].GhiChu = (i + 1).ToString();
            }
            ViewBag.ListctxnbyOne = model;
            return PartialView();
        }
        
        public ActionResult updatedaxemMsg()
        {
            var uid = int.Parse(Session["UserId"].ToString());
            var ten = "";
            if(uid == 1) ten = "AdminDaxem";
            if (uid == 2) ten = "Sub2Daxem";
            if (uid == 4) ten = "Sub4Daxem";
            if (uid == 5) ten = "Sub5Daxem";
            //if (uid == 6) ten = "AdminDaxem";
            var kq = dbc.Database.ExecuteSqlCommand("Update ["+DBname+"TechZone].[dbo].[MsgAotu] set "+ ten+ "=1 where " + ten+"=0");
            return Json(kq, JsonRequestBehavior.AllowGet);
        }
    }
}