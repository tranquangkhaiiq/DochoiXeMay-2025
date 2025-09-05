using DoChoiXeMay.Areas.Admin.Data;
using DoChoiXeMay.Filters;
using DoChoiXeMay.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Tokenizer.Symbols;
using System.Web.UI.WebControls.Expressions;

namespace DoChoiXeMay.Areas.Admin.Controllers
{
    [Protect]
    public class XuatNhapController : Controller
    {
        // GET: Admin/XuatNhap
        Model1 dbc=new Model1();
        string DBname = ConfigurationManager.AppSettings["DBname"];
        public ActionResult LocUserName()
        {
            var UserId = dbc.UserTeks.Where(kh => kh.IdLoai <3).
                            Select(kh => new { id = kh.Id, ten = kh.UserName });

            return Json(UserId, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListXuatNhapUser()
        {
            ViewBag.IdMaTC = new SelectList(dbc.MaTCs.Where(kh => kh.SuDung == true && kh.XuatNhap==true), "Id", "GhiChu");
            Session["requestUri"] = "/Admin/XuatNhap/ListXuatNhapUser";
            return View();
        }
        public ActionResult GetListKyXNUser()
        {
            var uid = int.Parse(Session["UserId"].ToString());
            if(Session["quyen"].ToString() == "1")
            {
                ViewBag.KyXN = dbc.KyXuatNhaps.Where(kh => kh.UserId == uid && kh.Id > 1 && kh.UPush == false
                        ||(kh.UserId != uid && kh.Id>1 && kh.UPush ==true && kh.AdminXNPUSH ==false))
                    .OrderByDescending(kh => kh.Id)
                    .ToList();
            }
            else
            {
                ViewBag.KyXN = dbc.KyXuatNhaps.Where(kh => kh.UserId == uid && kh.Id > 1 && kh.AdminXNPUSH == false)
                    .OrderByDescending (kh => kh.Id)
                    .ToList();
            }
            
            return PartialView();
        }
        public ActionResult ListXuatNhapTeK()
        {
            Session["requestUri"] = "/Admin/XuatNhap/ListXuatNhapTeK";
            ViewBag.UserId = new SelectList(dbc.UserTeks.Where(kh => kh.IdLoai < 3), "Id", "UserName");
            return View();
        }
        public ActionResult GetListKyXNTeK(int PageNo = 0, int PageSize = 8,int UserId = 0)
        {
            ViewBag.KyXNTeK = new Data.XuatNhapData().getXuatNhapTek(PageNo, PageSize,UserId);
            return PartialView();
        }
        public ActionResult GetPageCountXNTek(int PageSize = 8,int UserId=0)
        {
            var num = new Data.XuatNhapData().GetPageCountXuatNhapTek(UserId);
            var pageCount = Math.Ceiling(1.0 * num / PageSize);
            return Json(pageCount, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult UpdateKyXNUser(int id)
        {
            var model = dbc.KyXuatNhaps.Find(id);
            ViewBag.IdMaTC = new SelectList(dbc.MaTCs.Where(kh => kh.SuDung == true && kh.XuatNhap == true), "Id", "GhiChu",model.IdMaTC);
            return View(model);
        }
        public ActionResult DayXuatNhapTek(int id,int thuchi)
        {
            //có thuchi => thuchi=0
            var uid = int.Parse(Session["UserId"].ToString());
            var XN = dbc.KyXuatNhaps.Find(id);
            if(XN != null)
            {
                if (Session["quyen"].ToString() == "1")
                {
                    //Admin thì đẩy lên luôn
                    if (XN.UPush == false)
                    {
                        XN.UPush = true;
                        XN.AdminXNPUSH = true;
                        var modelct = dbc.ChitietXuatNhaps.Where(kh=>kh.IdKy==id).ToList();
                        //add vao bang hang hoa
                        if (XN.XuatNhap)
                        {
                            //Kiểm tra số lượng bảng hh >= so luong xuất
                            var kqktHH = Data.XuatNhapData.kiemtrasoluongHH(dbc, id);
                            if (kqktHH == false)
                            {
                                Session["ThongBaoXuatNhapUser"] = "Có Lỗi xuất hàng !!! HH trong bảng HH không tồn tại hoặc không đủ số lượng.";
                                //tro lai trang truoc do
                                return RedirectToAction("ListXuatNhapUser");
                            }
                            else
                            {
                                for (int i = 0; i < modelct.Count(); i++)
                                {
                                    var kq = Data.XuatNhapData.XuatHangHoa(dbc, modelct[i].Ten, modelct[i].IDMF,
                                        modelct[i].IDColor, modelct[i].IDSize, modelct[i].SoLuong);
                                    if (kq == false)
                                    {
                                        Session["ThongBaoXuatNhapUserct"] = "Có Lỗi xuất hàng: " + modelct[i].Ten + " không đủ đk để xuất!!!.";
                                    }
                                }
                            }
                        }
                        else
                        {
                            //ky nhập + vào, không cần kiểm tra
                            for (int i = 0; i < modelct.Count(); i++)
                            {
                                var kq = Data.XuatNhapData.GhibangHangHoa(dbc, modelct[i].Ten, modelct[i].IDMF,
                                    modelct[i].IDColor, modelct[i].IDSize, modelct[i].SoLuong, modelct[i].Gianhap,
                                    modelct[i].Hinh1, modelct[i].Hinh2, modelct[i].Hinh3);
                            }
                        }
                        

                    }
                }
                else
                {
                    //Sub thì phải xin, Nếu lên rồi thì phải "Thu Hồi"
                    if(XN.UPush == false && XN.AdminXNPUSH == false)
                    {
                        XN.UPush = true;
                    }
                    else if(XN.UPush==true && XN.AdminXNPUSH == false)
                    {
                        XN.UPush = false;
                    }
                    XN.STT = thuchi.ToString();//mới 25/3/2025
                }
                dbc.Entry(XN).State = EntityState.Modified;
                var update = dbc.SaveChanges();
                if (update > 0)
                {
                    if (thuchi == 0)//Có thuchi
                    {
                        if (XN.UPush == true && XN.AdminXNPUSH == true)
                        {
                            var kqins = InsertThuChiTeKauto(id);
                        }
                        //Insert Nhật Ký
                        var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, uid, Session["quyen"].ToString()
                                , Session["UserName"].ToString(), "Thay đổi yêu cầu DayXuatNhapTek - Đẩy= " + XN.UPush, "");
                        //Thoong bao Msg cho SubAd
                        if (Session["quyen"].ToString() != "1" && XN.UPush == true)
                        {
                            var sms = Session["UserName"].ToString().ToUpper() + " đã yêu cầu đẩy -" + XN.TenKy + "- của mình lên Tek." + XN.NgayAuto.ToString("{dd/MM/yyyy}") +
                                "-ADMIN vào Bảng XN của mình để xác nhận.";
                            var Msg = Data.XuatNhapData.InsertMsgAotu(dbc, uid, sms, false, false, false, false, false);
                        }
                        if (Session["quyen"].ToString() != "1" && XN.UPush == false)
                        {
                            var sms = Session["UserName"].ToString().ToUpper() + " đã Hủy yêu cầu đẩy -" + XN.TenKy + "- lên Tek của mình." + XN.NgayAuto.ToString("{dd/MM/yyyy}");
                            var Msg = Data.XuatNhapData.InsertMsgAotu(dbc, uid, sms, false, false, false, false, false);
                        }
                        Session["ThongBaoXuatNhapUser"] = "Thay đổi yêu cầu DayXuatNhapTek thành công.";
                    }
                    else
                    {
                        //Nhập Sản phẩm do TeK tạo ra, hoặc xuất tặng, cho mượn,...
                        //Không có InsertThuChiTeKauto(id)
                        //Insert Nhật Ký
                        var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, uid, Session["quyen"].ToString()
                                , Session["UserName"].ToString(), "Thay đổi yêu cầu DayXNTek(Không có TC) - Đẩy= " + XN.UPush, "");
                        //Thoong bao Msg cho SubAd
                        if (Session["quyen"].ToString() != "1" && XN.UPush == true)
                        {
                            var sms = Session["UserName"].ToString().ToUpper() + " đã yêu cầu đẩy -" + XN.TenKy + "- của mình lên Tek(Không có TC)." + XN.NgayAuto.ToString("{dd/MM/yyyy}") +
                                "-ADMIN vào Bảng XN của mình để xác nhận.";
                            var Msg = Data.XuatNhapData.InsertMsgAotu(dbc, uid, sms, false, false, false, false, false);
                        }
                        if (Session["quyen"].ToString() != "1" && XN.UPush == false)
                        {
                            var sms = Session["UserName"].ToString().ToUpper() + " đã Hủy yêu cầu đẩy -" + XN.TenKy + "- lên Tek(Không có TC) của mình." + XN.NgayAuto.ToString("{dd/MM/yyyy}");
                            var Msg = Data.XuatNhapData.InsertMsgAotu(dbc, uid, sms, false, false, false, false, false);
                        }
                        Session["ThongBaoXuatNhapUser"] = "Thay đổi yêu cầu DayXNTek(Không có TC) thành công.";
                    }
                    //tro lai trang truoc do 
                    var requestUri = Session["requestUri"] as string;
                    if (requestUri != null)
                    {
                        return Redirect(requestUri);
                    }
                }
                
            }
            else
            {
                Session["ThongBaoXuatNhapUser"] = "Kỳ Xuất nhập không tồn tại !!!";
            }
            return RedirectToAction("ListXuatNhapUser");
        }
        public bool InsertThuChiTeKauto(int idky)
        {
            var uid = int.Parse(Session["UserId"].ToString());
            var XN = dbc.KyXuatNhaps.Find(idky);
            //Insert ThuChiTeK auto
            ChiTietTC cttc = new ChiTietTC();
            cttc.Id = Guid.NewGuid();
            cttc.IdMa = XN.IdMaTC;
            cttc.IdHT = 1;
            cttc.IdKyxuatnhap = idky;
            var xuatnhap = XN.XuatNhap == true ? "Xuất" : "Nhập";
            cttc.Noidung = "Auto từ kỳ " + xuatnhap + " Ngày:" + XN.NgayXuatNhap.ToString("{dd/MM/yyyy}");
            cttc.SoTien = XN.TongTienAuto;
            cttc.ThuChi = XN.XuatNhap;  //ThuChi = false= chira=nhaphang
            cttc.HoaDon = XN.HoaDon;
            cttc.Filesave1 = XN.Filesave2;
            cttc.Filesave2 = XN.Filesave3;
            //***
            cttc.NgayTC = XN.NgayXuatNhap;
            cttc.UserId = XN.UserId;
            var kqinsert = new Data.ThuChiData().InsertThuChiTeK(cttc, Session["quyen"].ToString(), Session["UserName"].ToString());
            if(kqinsert)
                return true;
            return false;
        }
        public ActionResult XacNhanXuatNhapTek(int id)
        {
            var uid = int.Parse(Session["UserId"].ToString());
            var XN = dbc.KyXuatNhaps.Find(id);
            if (XN != null)
            {
                if (XN.UPush == true && XN.AdminXNPUSH ==false)
                {
                    if (XN.XuatNhap)
                    {
                        //Kiểm tra số lượng bảng hh >= so luong xuat
                        var kqktHH = Data.XuatNhapData.kiemtrasoluongHH(dbc, id);
                        if (kqktHH == false)
                        {
                            Session["ThongBaoXuatNhapTeK"] = "Xác nhận Xuất hàng thất bại !!! HH trong bảng HH không tồn tại hoặc không đủ số lượng.";
                            return RedirectToAction("ListXuatNhapUser");
                        }
                        else
                        {
                            XN.AdminXNPUSH = true;
                            dbc.Entry(XN).State = EntityState.Modified;
                            var update = dbc.SaveChanges();
                            if (update > 0)
                            {
                                if (XN.STT == "0")//Sản phẩm có thu chi
                                {
                                    var kqaotu = InsertThuChiTeKauto(id);
                                }
                                //Trừ vao bang hang hoa
                                var modelct = dbc.ChitietXuatNhaps.Where(kh => kh.IdKy == id).ToList();
                                for (int i = 0; i < modelct.Count(); i++)
                                {
                                    var kq = Data.XuatNhapData.XuatHangHoa(dbc, modelct[i].Ten, modelct[i].IDMF,
                                        modelct[i].IDColor, modelct[i].IDSize, modelct[i].SoLuong);
                                    if (kq == false)
                                    {
                                        Session["ThongBaoXuatNhapTeKct"] = "Có Lỗi xuất hàng: " + modelct[i].Ten + " không đủ đk để xuất!!!.";
                                        Session["ThongBaoXuatNhapUserct"] = "Có Lỗi xuất hàng: " + modelct[i].Ten + " không đủ đk để xuất!!!.";
                                    }
                                }
                                //Insert Nhật Ký
                                var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, uid, Session["quyen"].ToString()
                                        , Session["UserName"].ToString(), "XacNhanXuat HH Tek va THUCHI - Đẩy File Xuất nhập- " + XN.TenKy + "- của user: " + XN.UserTek.UserName, "");

                            }
                        }
                    }
                    else //kỳ nhập
                    {
                        XN.AdminXNPUSH = true;
                        dbc.Entry(XN).State = EntityState.Modified;
                        var update = dbc.SaveChanges();
                        if (update > 0)
                        {
                            if(XN.STT == "0")//Sản phẩm có thu chi
                            {
                                var kqaotu = InsertThuChiTeKauto(id);
                            }
                            //add vao bang hang hoa
                            var modelct = dbc.ChitietXuatNhaps.Where(kh => kh.IdKy == id).ToList();
                            for (int i = 0; i < modelct.Count(); i++)
                            {
                                var kq = Data.XuatNhapData.GhibangHangHoa(dbc, modelct[i].Ten, modelct[i].IDMF,
                                    modelct[i].IDColor, modelct[i].IDSize, modelct[i].SoLuong, modelct[i].Gianhap,
                                    modelct[i].Hinh1, modelct[i].Hinh2, modelct[i].Hinh3);
                            }
                            //Insert Nhật Ký
                            var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, uid, Session["quyen"].ToString()
                                    , Session["UserName"].ToString(), "XacNhanNhap HH Tek va THUCHI - Đẩy File Xuất nhập- " + XN.TenKy + "- của user: " + XN.UserTek.UserName, "");
                            
                        }
                    }
                    Session["ThongBaoXuatNhapUser"] = "XacNhanXuatNhapTek thành công File Xuất nhập- " + XN.TenKy + "- của user: " + XN.UserTek.UserName;
                    Session["ThongBaoXuatNhapTeK"] = "XacNhanXuatNhapTek thành công File Xuất nhập- " + XN.TenKy + "- của user: " + XN.UserTek.UserName;
                    //Thoong bao Msg cho SubAd
                    if (Session["quyen"].ToString() == "1")
                    {
                        var sms = "Yêu cầu đẩy -" + XN.TenKy + "- của -" + XN.UserTek.UserName.ToUpper() + "- được chấp nhận." + XN.NgayAuto.ToString("{dd/MM/yyyy}")
                            + "-" + XN.UserTek.UserName.ToUpper() + " vào Bảng XN TeK và Bảng TC Tek để kiểm tra.";
                        var Msg = Data.XuatNhapData.InsertMsgAotu(dbc, XN.UserId, sms, false, false, false, false, false);
                    }
                }
                else
                {
                    Session["ThongBaoXuatNhapUser"] = "XacNhanXuatNhapTek không thành công: User chưa yêu cầu đẩy!!!";
                }
                
            }
            else
            {
                Session["ThongBaoXuatNhapUser"] = "Kỳ Xuất nhập không tồn tại !!!";
            }
            //tro lai trang truoc do 
            var requestUri = Session["requestUri"] as string;
            if (requestUri != null)
            {
                return Redirect(requestUri);
            }
            return RedirectToAction("ListXuatNhapUser");
        }
        [HttpPost]
        public ActionResult UpdateKyXNUser(KyXuatNhap XN)
        {
            try
            {
                var NgayXuatNhap = XN.NgayXuatNhap.ToString("{dd/MM/yyyy}");
                var file1 = Request.Files["Dinhkem1"];
                var file2 = Request.Files["Dinhkem2"];
                var file3 = Request.Files["Dinhkem3"];
                if (file1.ContentLength>0)
                {
                    //Xoa hinh cu
                    bool xoahinhcu = XstringAdmin.Xoahinhcu("imgxuatnhap/", XN.HoaDon);
                    XN.HoaDon = XstringAdmin.saveFile(file1, "imgxuatnhap/");
                }
                if (file2.ContentLength > 0)
                {
                    //Xoa hinh cu
                    bool xoahinhcu = XstringAdmin.Xoahinhcu("imgxuatnhap/", XN.Filesave2);
                    XN.Filesave2 = XstringAdmin.saveFile(file2, "imgxuatnhap/");
                }
                if (file3.ContentLength > 0)
                {
                    //Xoa hinh cu
                    bool xoahinhcu = XstringAdmin.Xoahinhcu("imgxuatnhap/", XN.Filesave3);
                    XN.Filesave3 = XstringAdmin.saveFile(file3, "imgxuatnhap/");
                }
                XN.NgayAuto = DateTime.Now;
                //Tính lại Tổng tiền
                XN.TongTienAuto=TinhTongtienKy(XN.Id, XN.VAT, XN.Shipper, XN.CKtienmat, XN.CKphantram);
                var kq = new Data.XuatNhapData().UPdateKyXN(XN);
                if (kq == true)
                {
                    var userid = int.Parse(Session["UserId"].ToString());
                    
                    Session["ThongBaoXuatNhapUser"] = "Update thành công kỳ xuất nhập ngay: " + NgayXuatNhap;
                    Session["ThongBaoXuatNhapTeK"] = "Update thành công kỳ xuất nhập ngay: " + NgayXuatNhap;
                    var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, userid, Session["quyen"].ToString()
                        , Session["UserName"].ToString(), "UpdateKyXNUser-" + NgayXuatNhap, "");
                    //tro lai trang truoc do 
                    var requestUri = Session["requestUri"] as string;
                    if (requestUri != null)
                    {
                        return Redirect(requestUri);
                    }
                    return RedirectToAction("ListXuatNhapUser");
                }
                else
                {
                    ModelState.AddModelError("", "Update Kỳ XN Thất Bại !!!!");
                    var model = dbc.ChiTietTCs.Find(XN.Id);
                    ViewBag.IdMaTC = new SelectList(dbc.MaTCs.Where(kh => kh.SuDung == true && kh.XuatNhap == true), "Id", "GhiChu", XN.IdMaTC);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                ModelState.AddModelError("", "Update Xuất Nhập Thất Bại !!!! Có Lỗi hệ thống.");
                var model = dbc.ChiTietTCs.Find(XN.Id);
                ViewBag.IdMaTC = new SelectList(dbc.MaTCs.Where(kh => kh.SuDung == true && kh.XuatNhap == true), "Id", "GhiChu", XN.IdMaTC);
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult InsertKyXuatNhap(KyXuatNhap XN)
        {
            try
            {
                var uid = int.Parse(Session["UserId"].ToString());
                KyXuatNhap model = new KyXuatNhap();
                model = XN;
                model.UserId = uid;
                model.UPush = false;
                if (Session["quyen"].ToString() == "1")
                {
                    model.AdminXNPUSH = true;
                }
                else
                {
                    model.AdminXNPUSH = false;
                }
                model.UYeuCauThuHoi = false;
                model.TongTienAuto = 0;
                model.NgayAuto = DateTime.Now;
                var file1 = Request.Files["HoaDon"];
                var file2 = Request.Files["Filesave2"];
                var file3 = Request.Files["Filesave3"];
                var ten1 = XstringAdmin.saveFile(file1, "imgxuatnhap/");
                var ten2 = XstringAdmin.saveFile(file2, "imgxuatnhap/");
                var ten3 = XstringAdmin.saveFile(file3, "imgxuatnhap/");

                model.HoaDon = ten1;
                model.Filesave2 = ten2;
                model.Filesave3 = ten3;
                dbc.KyXuatNhaps.Add(model);
                int kt = dbc.SaveChanges();
                if (kt > 0)
                {
                    Session["ThongBaoXuatNhapUser"] = "Thêm mới thành công Kỳ xuất nhập ngày:" + model.NgayXuatNhap.ToString("{dd/MM/yyyy}");
                    var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, uid, Session["quyen"].ToString(),
                         Session["UserName"].ToString(), "Insert thành công kỳ XN:" + model.NgayXuatNhap.ToString("{dd/MM/yyyy}"), "");
                    //tro lai trang truoc do 
                    var requestUri = Session["requestUri"] as string;
                    if (requestUri != null)
                    {
                        return Redirect(requestUri);
                    }
                    return RedirectToAction("ListXuatNhapUser");
                }
                else
                {
                    Session["ThongBaoXuatNhapUser"] = "Thêm mới thất bại Kỳ xuất nhập ngày:" + model.NgayXuatNhap.ToString("{dd/MM/yyyy}") + " !!!";
                    return RedirectToAction("ListXuatNhapUser");
                }
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                Session["ThongBaoXuatNhapUser"]="Thêm mới Thất Bại !!!!!!!!!!. Có lỗi hệ thống";
            }
            
            return RedirectToAction("ListXuatNhapUser");
        }
        [HttpGet]
        public ActionResult InsertChiTietXNbyKy(int id) {
            var ky = dbc.KyXuatNhaps.Find(id);
            Session["TenKy"]=ky.TenKy;
            Session["IDKy"] = id;
            Session["xuatnhap"] = ky.XuatNhap==true?"Xuat":"Nhap";
            Session["CKphantram"] = ky.CKphantram;
            Session["CKtienmat"] = ky.CKtienmat;
            ViewBag.IDMF = new SelectList(dbc.Manufacturers.Where(kh => kh.Sudung == true), "Id", "Name");
            ViewBag.IDColor = new SelectList(dbc.Colors.OrderByDescending(kh => kh.Id), "Id", "TenColor");
            ViewBag.IDSize = new SelectList(dbc.Sizes.OrderBy(kh => kh.Id), "Id", "TenSize");
            ViewBag.NameSP = dbc.HangHoas.DistinctBy(kh=>kh.Ten);
            
            return View();
        }
        [HttpPost]
        public ActionResult InsertChiTietXNbyKy(ChitietXuatNhap ctxn)
        {
            ViewBag.IDMF = new SelectList(dbc.Manufacturers.Where(kh => kh.Sudung == true), "Id", "Name");
            ViewBag.IDColor = new SelectList(dbc.Colors.OrderByDescending(kh => kh.Id), "Id", "TenColor");
            ViewBag.IDSize = new SelectList(dbc.Sizes.OrderBy(kh => kh.Id), "Id", "TenSize");
            ViewBag.NameSP = dbc.HangHoas.DistinctBy(kh => kh.Ten);
            //check trùng hàng cùng kỳ
            var Checkctxn = dbc.ChitietXuatNhaps.FirstOrDefault(kh => kh.IdKy == ctxn.IdKy && kh.Ten.ToLower().Trim() == ctxn.Ten.ToLower().Trim() && kh.IDMF == ctxn.IDMF
                                                && kh.IDColor == ctxn.IDColor && kh.IDSize == ctxn.IDSize);
            if (Checkctxn == null)
            {
                ChitietXuatNhap model = new ChitietXuatNhap();
                if (Session["xuatnhap"].ToString() == "Nhap")
                {
                    model = ctxn;
                    model.Id = Guid.NewGuid();
                    model.IdKy = int.Parse(Session["IDKy"].ToString());
                    model.NgayAuto = DateTime.Now;
                    var file1 = Request.Files["Hinh1"];
                    var file2 = Request.Files["Hinh2"];
                    var file3 = Request.Files["Hinh3"];
                    var ten1 = XstringAdmin.saveFile(file1, "imgxuatnhap/");
                    var ten2 = XstringAdmin.saveFile(file2, "imgxuatnhap/");
                    var ten3 = XstringAdmin.saveFile(file3, "imgxuatnhap/");
                    model.Hinh1 = ten1;
                    model.Hinh2 = ten2;
                    model.Hinh3 = ten3;
                }
                else  /////Kỳ Xuất
                {
                    var checkhh = dbc.HangHoas.FirstOrDefault(kh => kh.Ten.ToLower().Trim() == ctxn.Ten.ToLower().Trim() && kh.IDMF == ctxn.IDMF
                                                    && kh.IDColor == ctxn.IDColor && kh.IDSize == ctxn.IDSize);
                    if (checkhh != null)
                    {
                        if (checkhh.SoLuong >= ctxn.SoLuong)
                        {
                            model = ctxn;
                            model.Id = Guid.NewGuid();
                            model.IdKy = int.Parse(Session["IDKy"].ToString());
                            model.NgayAuto = DateTime.Now;
                            var file1 = Request.Files["Hinh1"];
                            var file2 = Request.Files["Hinh2"];
                            var file3 = Request.Files["Hinh3"];
                            var ten1 = XstringAdmin.saveFile(file1, "imgxuatnhap/");
                            var ten2 = XstringAdmin.saveFile(file2, "imgxuatnhap/");
                            var ten3 = XstringAdmin.saveFile(file3, "imgxuatnhap/");
                            model.Hinh1 = ten1;
                            model.Hinh2 = ten2;
                            model.Hinh3 = ten3;
                        }
                        else
                        {
                            ModelState.AddModelError("", "Thêm hh xuất Thất Bại !!!! Trong kho không đủ hàng.");
                            return View();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thêm hh xuất Thất Bại !!!! Không có hàng trong kho.");
                        return View();
                    }
                }

                dbc.ChitietXuatNhaps.Add(model);
                int kq = dbc.SaveChanges();
                if (kq > 0)
                {
                    //Update tổng tiền
                    KyXuatNhap XN = dbc.KyXuatNhaps.Find(model.IdKy);
                    var tt = TinhTongtienKy(model.IdKy, XN.VAT, XN.Shipper, XN.CKtienmat, XN.CKphantram);
                    XN.TongTienAuto = tt;
                    dbc.Entry(XN).State = EntityState.Modified;
                    var update = dbc.SaveChanges();
                    var uid = int.Parse(Session["UserId"].ToString());
                    var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, uid, Session["quyen"].ToString()
                            , Session["UserName"].ToString(), "InsertChiTietXNbyKy-" + model.SoLuong + " " + model.Ten, "");
                    Session["ThongBaoXuatNhapUser"] = "Thêm mới thành công " + model.SoLuong + " " + model.Ten + ".";
                }
                //tro lai trang truoc do 
                var requestUri = Session["requestUri"] as string;
                if (requestUri != null)
                {
                    return Redirect(requestUri);
                }
                return RedirectToAction("ListXuatNhapUser");
            }
            else
            {
                ModelState.AddModelError("", "Thêm hh Thất Bại !!!! đã có hàng trong kỳ.");
                return View();
            }
            
        }
        public ActionResult XoaChiTietXNbyID(string id)
        {
            var model = dbc.ChitietXuatNhaps.Find(new Guid(id));
            var sl = model.SoLuong;
            var ten = model.Ten;
            var idky = model.IdKy;
            var tenhinh1 = model.Hinh1;
            var tenhinh2 = model.Hinh2;
            var tenhinh3 = model.Hinh3;
            if (model != null)
            {
                dbc.ChitietXuatNhaps.Remove(model);
                var kq= dbc.SaveChanges();
                if (kq > 0)
                {
                    //Xoa hinh cu
                    bool xoahinhcu1 = XstringAdmin.Xoahinhcu("imgxuatnhap/", tenhinh1);
                    bool xoahinhcu2 = XstringAdmin.Xoahinhcu("imgxuatnhap/", tenhinh2);
                    bool xoahinhcu3 = XstringAdmin.Xoahinhcu("imgxuatnhap/", tenhinh3);
                    
                    KyXuatNhap XN = dbc.KyXuatNhaps.Find(idky);
                    //update tổng tiền
                    var tt = TinhTongtienKy(idky,XN.VAT,XN.Shipper,XN.CKtienmat,XN.CKphantram);
                    XN.TongTienAuto = tt;
                    dbc.Entry(XN).State = EntityState.Modified;
                    var update = dbc.SaveChanges();
                    //nhật Ký
                    var uid = int.Parse(Session["UserId"].ToString());
                    var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, uid, Session["quyen"].ToString()
                            , Session["UserName"].ToString(), "XoaChiTietXNbyID-" + sl + " " + ten, "");
                    Session["ThongBaoXuatNhapUser"]="Xóa thành công "+sl.ToString()+" "+ ten.ToString();
                    Session["ThongBaoXuatNhapTeK"] = "Xóa thành công " + sl.ToString() + " " + ten.ToString();
                    //tro lai trang truoc do 
                    var requestUri = Session["requestUri"] as string;
                    if (requestUri != null)
                    {
                        return Redirect(requestUri);
                    }
                }
                else
                {
                    Session["ThongBaoXuatNhapUser"] = "Có Lỗi Khi Xóa " + sl.ToString() + " " + ten.ToString() + " !!!";
                }
            }
            return RedirectToAction("ListXuatNhapUser");
        }
        public ActionResult DeleteKyXNUser(int id)
        {
            var model = dbc.KyXuatNhaps.Find(id);
            if(model != null)
            {
                //Xoa hinh kỳ củ
                var tenhinh1 = model.HoaDon;
                var tenhinh2 = model.Filesave2;
                var tenhinh3 = model.Filesave3;
                bool xoahinhcu1 = XstringAdmin.Xoahinhcu("imgxuatnhap/", tenhinh1);
                bool xoahinhcu2 = XstringAdmin.Xoahinhcu("imgxuatnhap/", tenhinh2);
                bool xoahinhcu3 = XstringAdmin.Xoahinhcu("imgxuatnhap/", tenhinh3);
                //Xoa hinh chitietXN củ
                var chitietXNbyky = dbc.ChitietXuatNhaps.Where(kh => kh.IdKy == id).ToList();
                for(int i = 0; i < chitietXNbyky.Count(); i++)
                {
                    var tenhct1 = chitietXNbyky[i].Hinh1;
                    var tenhct2 = chitietXNbyky[i].Hinh2;
                    var tenhct3 = chitietXNbyky[i].Hinh3;
                    bool xoa1 = XstringAdmin.Xoahinhcu("imgxuatnhap/", tenhct1);
                    bool xoa2 = XstringAdmin.Xoahinhcu("imgxuatnhap/", tenhct2);
                    bool xoa3 = XstringAdmin.Xoahinhcu("imgxuatnhap/", tenhct3);
                }
                var tenky=model.TenKy;
                var ngayky = model.NgayXuatNhap;
                var nhapxuat = model.XuatNhap == true ? "Xuất" : "Nhập";
                var XoaChitietXN = dbc.Database.ExecuteSqlCommand("DELETE  FROM ["+DBname+"TechZone].[dbo].[ChitietXuatNhap] where IdKy=" + id);
                var kqct = ThuChiData.DeleteThuChibyKy(dbc, id);
                var XoaKyXN = dbc.Database.ExecuteSqlCommand("DELETE  FROM ["+DBname+"TechZone].[dbo].[KyXuatNhap] where Id=" + id);
                Session["ThongBaoXuatNhapUser"] = "Xóa thành công kỳ " + nhapxuat + " " + tenky.ToString()+
                    ". Nếu Thu Chi TeK có Dl cũng bị thu hồi.";
                //Nhật ký
                var uid = int.Parse(Session["UserId"].ToString());
                var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, uid, Session["quyen"].ToString()
                        , Session["UserName"].ToString(), "DeleteKyXNUser-" + tenky + " " + ngayky, "");
            }
            return RedirectToAction("ListXuatNhapUser");
        }
        public ActionResult YeuCauThuHoi(int id)
        {
            var model = dbc.KyXuatNhaps.Find(id);
            if(model != null)
            {
                //Đã ở trên Tek 
                if(model.UPush == true && model.AdminXNPUSH == true)
                {
                    if (model.UYeuCauThuHoi == false)
                    {
                        model.UYeuCauThuHoi = true;
                    }
                    else
                    {
                        model.UYeuCauThuHoi = false;
                    }

                    model.NgayAuto = DateTime.Now;
                    dbc.Entry(model).State = EntityState.Modified;
                    var th = dbc.SaveChanges();
                    if (th > 0)
                    {
                        if (model.UYeuCauThuHoi)
                        {
                            Session["ThongBaoXuatNhapTeK"] = "Bạn Đã Yêu Cầu Thu Hồi Kỳ XN:" + model.TenKy + "-" + model.NgayXuatNhap.ToString("{dd/MM/yyyy}");
                            //Nhật ký
                            var uid = int.Parse(Session["UserId"].ToString());
                            var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, uid, Session["quyen"].ToString()
                                    , Session["UserName"].ToString(), "Yêu Cầu ThuHoi kỳ XN-" + model.TenKy, "");
                            //Thoong bao Msg cho SubAd
                            var sms = model.UserTek.UserName.ToUpper() + " đã Yêu Cầu Thu Hồi kỳ XN-" + model.TenKy + "-" + model.NgayXuatNhap.ToString("{dd/MM/yyyy}")+
                                    "-ADMIN vào Bảng XN TeK để xác nhận.";
                            var Msg = Data.XuatNhapData.InsertMsgAotu(dbc, model.UserId, sms, false, false, false, false, false);

                        }
                        else
                        {
                            Session["ThongBaoXuatNhapTeK"] = "Bạn Đã Hủy Yêu Cầu Thu Hồi Kỳ XN:" + model.TenKy + "-" + model.NgayXuatNhap.ToString("{dd/MM/yyyy}");
                            //Nhật ký
                            var uid = int.Parse(Session["UserId"].ToString());
                            var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, uid, Session["quyen"].ToString()
                                    , Session["UserName"].ToString(), "Hủy Yêu Cầu ThuHoi kỳ XN-" + model.TenKy, "");
                            //Thoong bao Msg cho SubAd
                            var sms = model.UserTek.UserName.ToUpper() + " đã Hủy Yêu Cầu Thu Hồi kỳ XN-" + model.TenKy + "-" + model.NgayXuatNhap.ToString("{dd/MM/yyyy}");
                            var Msg = Data.XuatNhapData.InsertMsgAotu(dbc, model.UserId, sms, false, false, false, false, false);

                        }

                        return RedirectToAction("ListXuatNhapTeK");
                    }
                    Session["ThongBaoXuatNhapTeK"] = "Yêu Cầu/Hủy Yêu cầu Thu Hồi thất bại !!! Có Lỗi Dữ Liệu.";
                    return RedirectToAction("ListXuatNhapTeK");
                }
                else
                {
                    Session["ThongBaoXuatNhapTeK"] = "Yêu Cầu/Hủy Yêu cầu Thu Hồi thất bại !!! Kỳ XN Chưa đưa lên TeK.";
                    return RedirectToAction("ListXuatNhapTeK");
                }
                
            }
            else
            {
                Session["ThongBaoXuatNhapTeK"] = "Yêu Cầu/Hủy Yêu Thu Hồi thất bại !!! Kỳ XN không tồn tại.";
                return RedirectToAction("ListXuatNhapTeK");
            }
        }
        public ActionResult XacNhanThuHoi(int id)
        {
            var model = dbc.KyXuatNhaps.Find(id);
            if(model != null)
            {
                if(model.UPush==true && model.AdminXNPUSH==true && model.UYeuCauThuHoi == true)
                {
                    if (model.XuatNhap == false)
                    {
                        //Kiểm tra số lượng bảng hh >= so luong thu hồi
                        //ky Xuat khi thu hồi sẽ + vào, không cần kiểm tra
                        var kqktHH = Data.XuatNhapData.kiemtrasoluongHH(dbc, id);
                        if (kqktHH == false)
                        {
                            Session["ThongBaoXuatNhapTeK"] = "Thu Hồi thất bại !!! HH trong bảng HH không tồn tại hoặc không đủ số lượng.";
                            return RedirectToAction("ListXuatNhapTeK");
                        }
                    }
                    //delete bảng thu chi trước
                    var kqct = ThuChiData.DeleteThuChibyKy(dbc, id);
                    if (kqct)
                    {
                        model.UPush = false;
                        model.AdminXNPUSH = false;
                        model.UYeuCauThuHoi = false;
                        model.NgayAuto = DateTime.Now;
                        dbc.Entry(model).State = EntityState.Modified;
                        var kq = dbc.SaveChanges();
                        if (kq > 0)
                        {
                            //update bang HH
                            var modelct = dbc.ChitietXuatNhaps.Where(kh => kh.IdKy == id).ToList();
                            if (model.XuatNhap == false)
                            {
                                for (int i = 0; i < modelct.Count(); i++)
                                {
                                    var kqthuhoi = Data.XuatNhapData.XuatHangHoa(dbc, modelct[i].Ten, modelct[i].IDMF,
                                        modelct[i].IDColor, modelct[i].IDSize, modelct[i].SoLuong);
                                }
                            }
                            else //kỳ xuất
                            {
                                for (int i = 0; i < modelct.Count(); i++)
                                {
                                    var kqthuhoi = Data.XuatNhapData.GhibangHangHoa(dbc, modelct[i].Ten, modelct[i].IDMF,
                                        modelct[i].IDColor, modelct[i].IDSize, modelct[i].SoLuong, modelct[i].Gianhap,
                                    modelct[i].Hinh1, modelct[i].Hinh2, modelct[i].Hinh3);
                                }
                            }
                            
                            Session["ThongBaoXuatNhapTeK"] = "Xác nhận Thu Hồi Kỳ XN:" + model.TenKy + "-" + model.NgayXuatNhap.ToString("{dd/MM/yyyy}")+"-Thành Công.";
                            //Nhật ký
                            var uid = int.Parse(Session["UserId"].ToString());
                            var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, uid, Session["quyen"].ToString()
                                    , Session["UserName"].ToString(), "Xác Nhận ThuHoi kỳ XN-" + model.TenKy+"-Thành Công.", "");
                            //Thoong bao Msg cho SubAd
                            var sms =  "Yêu Cầu Thu Hồi kỳ XN-" + model.TenKy + "-" + model.NgayXuatNhap.ToString("{dd/MM/yyyy}")+"-của "+ model.UserTek.UserName.ToUpper() +
                                    " được chấp nhận-" + model.UserTek.UserName.ToUpper() + " vào Bảng XN của mình để kiểm tra.";
                            var Msg = Data.XuatNhapData.InsertMsgAotu(dbc, model.UserId, sms, false, false, false, false, false);

                        }
                    }
                    return RedirectToAction("ListXuatNhapTeK");
                }
                else
                {
                    Session["ThongBaoXuatNhapTeK"] = "Thu Hồi thất bại !!! Kỳ XN không đủ ĐK thu hồi.";
                    return RedirectToAction("ListXuatNhapTeK");
                }
            }
            else
            {
                Session["ThongBaoXuatNhapTeK"] = "Thu Hồi thất bại !!! Kỳ XN không tồn tại.";
                return RedirectToAction("ListXuatNhapTeK");
            }
        }
        public ActionResult CheckHHTEK(string Tenhh, int Hangsx=0, int Mau = 0, int Size = 0)
        {
            var araylistHH = Data.XuatNhapData.CheckHHTEKaotu(dbc,Tenhh, Hangsx, Mau, Size);
            return Json(araylistHH, JsonRequestBehavior.AllowGet);
        }
        public double TinhTongtienKy(int id , int VAT, double ship, double CKtienmat, int CKphantram)
        {
            //VAT và tiền ship được thay đổi 
            double kq=0;
            var ky = dbc.KyXuatNhaps.Find(id);
            var model = dbc.ChitietXuatNhaps.Where(kh => kh.IdKy == id).ToList();
            
                for (int i = 0; i < model.Count(); i++)
                {
                    double tong = model[i].SoLuong * model[i].Gianhap;
                    kq = kq + tong;
                }
            return kq + ship + VAT * kq/100 -CKtienmat - kq*CKphantram/100;
        }
    }
}