using DoChoiXeMay.Areas.Admin.Data;
using DoChoiXeMay.Filters;
using DoChoiXeMay.Models;
using DoChoiXeMay.Utils;
using MaHoa_GiaiMa_TaiKhoan;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Windows.Documents;
using static QRCoder.PayloadGenerator;

namespace DoChoiXeMay.Areas.Admin.Controllers
{
    [Protect]
    public class DanhMucController : Controller
    {
        // GET: Admin/DanhMuc
        Model1 dbc = new Model1();
        string DBname = ConfigurationManager.AppSettings["DBname"];
        TaiKhoanInfo tk = new TaiKhoanInfo();
        public ActionResult NhatKy()
        {
            Session["requestUri"] = "/Admin/DanhMuc/NhatKy";
            return View();
        }
        public ActionResult GetNhatKy()
        {
            var model = dbc.NhatKyUTeks.OrderByDescending(kh => kh.CreateDate)
                .Skip(0).Take(200).ToList();
            for (int i = 0; i < model.Count(); i++)
            {
                model[i].GhiChu = (i + 1).ToString();
            }
            ViewBag.GetNhatKy = model;
            return PartialView();
        }
        public ActionResult Listchinhanh()
        {
            Session["requestUri"] = "/Admin/DanhMuc/Listchinhanh";
            return View();
        }
        public ActionResult GetListchinhanh()
        {
            var model = dbc.Ser_ChiNhanh.Where(kh=>kh.Id>0)
                .OrderByDescending(kh => kh.Id)
                .ThenByDescending(kh=>kh.IdLevel).ToList();
            ViewBag.GetListchinhanh = model;
            return PartialView(model);
        }
        public ActionResult Levelchinhanh()
        {
            Session["requestUri"] = "/Admin/DanhMuc/Levelchinhanh";
            return View();
        }
        public ActionResult GetLevelchinhanh()
        {
            var model = dbc.Ser_Levelchinhanh.OrderByDescending(kh => kh.Id).ToList();
            ViewBag.GetLevelchinhanh = model;
            return PartialView();
        }
        public ActionResult UpdateChiNhanhvaUser(int id)
        {
            var model = dbc.Ser_ChiNhanh.Find(id);
            ViewBag.IdLevel = new SelectList(dbc.Ser_Levelchinhanh.OrderBy(kh=>kh.Id).ToList(), "Id", "Level_Name", model.IdLevel);
            ViewBag.IdKhuVuc = new SelectList(dbc.KhuVucs.Where(kh=>kh.Sudung==true).OrderBy(kh => kh.Id).ToList()
                                                                          , "Id", "TenKhuvuc", model.IdKhuVuc);
            var UserCN = dbc.UserTeks.FirstOrDefault(kh=>kh.Id== model.IdUser);
            if(UserCN != null)
            {
                string check_pass = tk.DeCryptDotNetNukePassword(UserCN.Password, "A872EDF100E1BC806C0E37F1B3FF9EA279F2F8FD378103CB", UserCN.PasswordSalt);//pass ma hoa
                Session["UserNamechinhanh"] = UserCN.UserName;
                Session["PWchinhanh"] = check_pass;
            }
            else
            {
                Session["UserNamechinhanh"] = "Da Xoa.";
                Session["PWchinhanh"] = "";
            }
            
            return View(model);
        }
        [HttpPost]
        public ActionResult UpdateChiNhanhvaUser(Ser_ChiNhanh CN, string UserName, string Password)
        {
            try
            {
                int updateUser = 0;
                if(Session["UserNamechinhanh"].ToString()!="Da Xoa.")//User chưa bị xóa thì mới làm
                {
                    var UserChinhanh = dbc.UserTeks.Find(CN.IdUser);
                    string check_pass = tk.DeCryptDotNetNukePassword(UserChinhanh.Password, "A872EDF100E1BC806C0E37F1B3FF9EA279F2F8FD378103CB", UserChinhanh.PasswordSalt);//pass ma hoa
                    if (UserChinhanh.UserName == UserName.Trim() && check_pass == Password.Trim() && UserChinhanh.EmailConnection == CN.Gmail)
                    {
                        //Không update UserChinhanh
                    }
                    else
                    {
                        //update UserChinhanh
                        if (UserName.Trim() == UserChinhanh.UserName)
                        {
                            UserChinhanh.UserName = UserName.Trim();
                        }
                        else
                        {
                            var CheckUser = dbc.UserTeks.FirstOrDefault(kh => kh.UserName == UserName.Trim());
                            if (CheckUser == null)
                            {
                                UserChinhanh.UserName = UserName.Trim();
                            }
                            else
                            {
                                ModelState.AddModelError("", "Update Thất Bại !!!! UserName bị trùng lặp.");
                                return View(CN);
                            }
                        }
                        UserChinhanh.EmailConnection = CN.Gmail;
                        string PasswordSalt = Convert.ToBase64String(tk.GenerateSalt()); //tạo chuổi salt ngẫu nhiên
                        string cipherPass = tk.EnCryptDotNetNukePassword(Password, "", PasswordSalt);

                        UserChinhanh.Password = cipherPass;
                        UserChinhanh.PasswordSalt = PasswordSalt;
                        UserChinhanh.lastPasswordChangedate = DateTime.Now;
                        dbc.Entry(UserChinhanh).State = EntityState.Modified;
                        dbc.SaveChanges();
                        updateUser = 1;
                    }
                }
                var model = dbc.Ser_ChiNhanh.Find(CN.Id);
                if(CN.IdLevel != model.IdLevel || model.IdKhuVuc != CN.IdKhuVuc && model.Id > 1)
                {
                    CN.TenChiNhanh = new ChiNhanhData().MakeNameChiNhanh(model.IdKhuVuc, CN.IdKhuVuc, CN.IdLevel,CN.STTCNOFTinh);
                    string[] ssst = CN.TenChiNhanh.Split('_');
                    CN.STTCNOFTinh = ssst[2].ToString();
                }

                var kqupdate = new ChiNhanhData().Update_ChiNhanh(CN, DBname);
                string sms = "";
                if (updateUser == 1)
                {
                    Session["ThongBaoListChiNhanh"] = "Update chi nhánh Id=" + CN.Id + " và User Web, thành công.";
                    sms = "Update chi nhánh Id=" + CN.Id + " và User Web, thành công.";
                }
                else
                {
                    Session["ThongBaoListChiNhanh"] = "Update chi nhánh Id=" + CN.Id + ", thành công.";
                    sms = "Update chi nhánh Id=" + CN.Id + ", thành công.";
                }
                //SMS hệ thống
                new Data.UserData().SMSvaNhatKy(dbc, Session["UserId"].ToString(), Session["UserName"].ToString()
                    , Session["quyen"].ToString(), sms);
                //tro lai trang truoc do 
                var requestUri = Session["requestUri"] as string;
                if (requestUri != null)
                {
                    return Redirect(requestUri);
                }
                return RedirectToAction("Listchinhanh");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                ModelState.AddModelError("", "Update Thất Bại !!!!" + message);
                return RedirectToAction("UpdateChiNhanhvaUser");
            }
        }
        public ActionResult InsertChiNhanh()
        {
            try
            {
                var UN = new Data.ActiveData().InsertUserAotu();
                
                var counHCM = dbc.Ser_ChiNhanh.Where(kh=>kh.IdKhuVuc==27).Count();
                var IDU = dbc.UserTeks.FirstOrDefault(kh => kh.UserName == UN).Id;
                Ser_ChiNhanh model = new Ser_ChiNhanh();
                model.TenChiNhanh = new ChiNhanhData().MakeNameChiNhanh(26,27, 3, XString.makeSTT(counHCM));
                model.DaiDien = "Trần Auto";
                model.SDT = "0987654321";
                model.IdKhuVuc = 27;
                string[] ssst = model.TenChiNhanh.Split('_');
                model.STTCNOFTinh=ssst[2].ToString();
                model.DiaChi = "139 Trần Văn ơn, Khu 6, Phú Hòa, Thủ Dầu Một, Bình Dương, Việt Nam.";
                model.TaiKhoanNH = "VietComBank 0987654321 Trần Auto";
                model.Gmail = "email@gmail.com";
                model.Sudung = false;
                model.IdLevel = 3;
                model.IdUser = IDU;
                model.GhiChu = "";
                dbc.Ser_ChiNhanh.Add(model);
                dbc.SaveChanges();
                Session["ThongBaoListChiNhanh"] = "Insert chi nhánh thành công. Cần update để sử dụng.";
                var userid = int.Parse(Session["UserId"].ToString());
                var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, userid, Session["quyen"].ToString()
                            , Session["UserName"].ToString(), "Insert chi nhánh - " + model.TenChiNhanh + "-" + DateTime.Now.ToString(), "");
                //tro lai trang truoc do 
                var requestUri = Session["requestUri"] as string;
                if (requestUri != null)
                {
                    return Redirect(requestUri);
                }
                return RedirectToAction("Listchinhanh");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return RedirectToAction("Listchinhanh");
            }
            
        }
        public ActionResult DeleteChiNhanh(int id)
        {
            var userid = int.Parse(Session["UserId"].ToString());
            if(id > 1)
            {
                try
                {
                    var model = dbc.Ser_ChiNhanh.Find(id);
                    var user = dbc.UserTeks.FirstOrDefault(kh => kh.Id == model.IdUser);
                    dbc.Ser_ChiNhanh.Remove(model);
                    dbc.SaveChanges();
                    var sms = "";
                    if (User != null)
                    {
                        var UserD = dbc.UserTeks.Find(model.IdUser);
                        dbc.UserTeks.Remove(UserD);
                        dbc.SaveChanges();
                        Session["ThongBaoListChiNhanh"] = "Delete chi nhánh và User liên kết :" + model.TenChiNhanh + " thành công.";
                        sms = "Delete chi nhánh và User liên kết :" + model.TenChiNhanh + " thành công.";
                    }
                    else
                    {
                        Session["ThongBaoListChiNhanh"] = "Delete chi nhánh :" + model.TenChiNhanh + " thành công.";
                        sms = "Delete chi nhánh :" + model.TenChiNhanh + " thành công.";
                    }
                    //SMS hệ thống
                    new Data.UserData().SMSvaNhatKy(dbc, Session["UserId"].ToString(), Session["UserName"].ToString()
                        , Session["quyen"].ToString(), sms);
                }
                catch (Exception ex) {
                    string message = ex.Message;
                    Session["ThongBaoListChiNhanh"] = "Chi nhánh đã mua hàng, delete chi nhánh Không thành công!!!."+message;

                    return RedirectToAction("Listchinhanh");
                }
                
            }
            //tro lai trang truoc do 
            var requestUri = Session["requestUri"] as string;
            if (requestUri != null)
            {
                return Redirect(requestUri);
            }
            return RedirectToAction("Listchinhanh");
        }
        public ActionResult UpdateLVChiNhanh(int id)
        {
            var model = dbc.Ser_Levelchinhanh.Find(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult UpdateLVChiNhanh(Ser_Levelchinhanh LV)
        {
            try
            {
                dbc.Entry(LV).State = EntityState.Modified;
                dbc.SaveChanges();
                Session["ThongBaoLVChiNhanh"] = "Update Level chi nhánh Id=" + LV.Id + ", thành công.";
                //SMS hệ thống
                var sms = "Update Level chi nhánh Id=" + LV.Id + ", thành công.";
                new Data.UserData().SMSvaNhatKy(dbc, Session["UserId"].ToString(), Session["UserName"].ToString()
                    , Session["quyen"].ToString(), sms);
                //tro lai trang truoc do 
                var requestUri = Session["requestUri"] as string;
                if (requestUri != null)
                {
                    return Redirect(requestUri);
                }
                return RedirectToAction("Levelchinhanh");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                ModelState.AddModelError("", "Update Thất Bại !!!!" + message);

                return View(LV);
            }
        }
        public ActionResult ListMaThuChi()
        {
            Session["requestUri"] = "/Admin/DanhMuc/ListMaThuChi";
            return View();
        }

        public ActionResult GetListMaThuChi()
        {
            var model = dbc.MaTCs.OrderByDescending(kh => kh.Id).ToList();
            ViewBag.GetListMaThuChi = model;
            return PartialView();
        }
        public ActionResult InsertLVChiNhanh()
        {
            Ser_Levelchinhanh model = new Ser_Levelchinhanh();
            model.Level_Name = "Auto Level";
            model.ChietKhau_bandau = 0;
            model.chietkhau_khbh = 0;
            model.chietkhau_KPIQUI = 0;
            model.ChietKhau_KPIYEAR = 0;
            model.ChietKhau_khac = 0;
            model.Thuongcuoinam = "Thưởng cuối năm.";
            dbc.Ser_Levelchinhanh.Add(model);
            dbc.SaveChanges();
            Session["ThongBaoLVChiNhanh"] = "Insert LV chi nhánh thành công. Cần update để sử dụng.";
            var userid = int.Parse(Session["UserId"].ToString());
            var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, userid, Session["quyen"].ToString()
                        , Session["UserName"].ToString(), "Insert LV chi nhánh-" + model.Level_Name + "-" + DateTime.Now.ToString(), "");
            //tro lai trang truoc do 
            var requestUri = Session["requestUri"] as string;
            if (requestUri != null)
            {
                return Redirect(requestUri);
            }
            return RedirectToAction("Levelchinhanh");
        }
        public ActionResult DeleteLVChiNhanh(int id)
        {
            var userid = int.Parse(Session["UserId"].ToString());
            var model = dbc.Ser_Levelchinhanh.Find(id);
            dbc.Ser_Levelchinhanh.Remove(model);
            dbc.SaveChanges();
            Session["ThongBaoLVChiNhanh"] = "Delete LV chi nhánh :"+model.Level_Name+" thành công.";
            var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, userid, Session["quyen"].ToString()
                        , Session["UserName"].ToString(), "Delete LV chi nhánh -" + model.Level_Name + "-" + DateTime.Now.ToString(), "");
            //tro lai trang truoc do 
            var requestUri = Session["requestUri"] as string;
            if (requestUri != null)
            {
                return Redirect(requestUri);
            }
            return RedirectToAction("Levelchinhanh");
        }
        public ActionResult InsertMaThuChi()
        {
            MaTC model = new MaTC();
            model.TenMa = "Ma moi TeK";
            model.GhiChu = "Cần update trước khi sử dụng.";
            model.NgayAuto = DateTime.Now;
            model.SuDung = false;
            model.XuatNhap = false;
            dbc.MaTCs.Add(model);
            dbc.SaveChanges();
            Session["ThongBaoMaThuChi"] = "Insert mã thu-chi thành công. Cần Update để sử dụng.";
            var userid = int.Parse(Session["UserId"].ToString());
            var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, userid, Session["quyen"].ToString()
                        , Session["UserName"].ToString(), "Insert Mã Thu Chi-" + model.TenMa + "-" + DateTime.Now.ToString(), "");
            //tro lai trang truoc do 
            var requestUri = Session["requestUri"] as string;
            if (requestUri != null)
            {
                return Redirect(requestUri);
            }
            return RedirectToAction("ListThuChiTeK");
        }
        public ActionResult UpdateMaThuChi(int id)
        {
            var model = dbc.MaTCs.Find(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult UpdateMaThuChi(MaTC Ma)
        {
            try
            {
                Ma.NgayAuto = DateTime.Now;
                Ma.XuatNhap = false ;
                dbc.Entry(Ma).State = EntityState.Modified;
                dbc.SaveChanges();
                Session["ThongBaoMaThuChi"] = "Update mã thu-chi "+Ma.TenMa+" thành công.";
                var userid = int.Parse(Session["UserId"].ToString());
                var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, userid, Session["quyen"].ToString()
                        , Session["UserName"].ToString(), "Update mã thu chi -" + Ma.TenMa + "-" + DateTime.Now.ToString(), "");
                //tro lai trang truoc do 
                var requestUri = Session["requestUri"] as string;
                if (requestUri != null)
                {
                    return Redirect(requestUri);
                }
                return RedirectToAction("ListThuChiTeK");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                ModelState.AddModelError("", "Update Thất Bại !!!!" + message);

                return View(Ma);
            }
        }
        public ActionResult ListMauSanPham()
        {
            Session["requestUri"] = "/Admin/DanhMuc/ListMauSanPham";
            return View();
        }
        public ActionResult GetListMauSanPham()
        {
            var model = dbc.Colors.OrderByDescending(kh => kh.Id).ToList();
            ViewBag.GetListMauSP = model;
            return PartialView();
        }
        public ActionResult InsertMauSP()
        {
            Color model = new Color();
            model.TenColor = "Mau moi TeK";
            model.Ngay=DateTime.Now;
            model.Ghichu = "Cần update trước khi sử dụng.";
            dbc.Colors.Add(model);
            dbc.SaveChanges();
            Session["ThongBaoMauSP"] = "Insert màu sp thành công. Cần Update để sử dụng.";
            var userid = int.Parse(Session["UserId"].ToString());
            var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, userid, Session["quyen"].ToString()
                        , Session["UserName"].ToString(), "Insert màu sp-" + model.TenColor + "-" + DateTime.Now.ToString(), "");
            //tro lai trang truoc do 
            var requestUri = Session["requestUri"] as string;
            if (requestUri != null)
            {
                return Redirect(requestUri);
            }
            return RedirectToAction("ListThuChiTeK");
        }
        public ActionResult UpdateMauSP(int id)
        {
            var model = dbc.Colors.Find(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult UpdateMauSP(Color Ma)
        {
            try
            {
                Ma.Ngay = DateTime.Now;
                dbc.Entry(Ma).State = EntityState.Modified;
                dbc.SaveChanges();
                Session["ThongBaoMauSP"] = "Update màu sp thành công.";
                var userid = int.Parse(Session["UserId"].ToString());
                var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, userid, Session["quyen"].ToString()
                        , Session["UserName"].ToString(), "Update màu sp-"+ Ma.TenColor+"-" + DateTime.Now.ToString(), "");
                //tro lai trang truoc do 
                var requestUri = Session["requestUri"] as string;
                if (requestUri != null)
                {
                    return Redirect(requestUri);
                }
                return RedirectToAction("ListThuChiTeK");
            }
            catch (Exception ex)
            {

                string message = ex.Message;
                ModelState.AddModelError("", "Update Thất Bại !!!!" + message);

                return View(Ma);
            }
        }
        public ActionResult ListHangSanXuat()
        {
            Session["requestUri"] = "/Admin/DanhMuc/ListHangSanXuat";
            return View();
        }
        public ActionResult GetListHangSanXuat()
        {
            var model = dbc.Manufacturers.OrderByDescending(kh => kh.Id).ToList();
            ViewBag.GetListHangsx = model;
            return PartialView();
        }
        public ActionResult InsertHangSX()
        {
            Manufacturer model = new Manufacturer();
            model.Name = "TeK" + DateTime.Now.ToString();
            model.Sudung = false;
            model.NgayAuto = DateTime.Now;
            model.Logo = "";
            dbc.Manufacturers.Add(model);
            dbc.SaveChanges();
            //tro lai trang truoc do 
            var requestUri = Session["requestUri"] as string;
            if (requestUri != null)
            {
                return Redirect(requestUri);
            }
            return RedirectToAction("ListThuChiTeK");
        }
        public ActionResult UpdatetHangSX(int id)
        {
            var model = dbc.Manufacturers.Find(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult UpdatetHangSX(Manufacturer Ma)
        {
            try
            {
                var file1 = Request.Files["Dinhkem1"];
                if (file1.ContentLength > 0)
                {
                    //Xoa hinh cu
                    bool xoahinhcu = XstringAdmin.Xoahinhcu("imgTeK/", Ma.Logo);
                    Ma.Logo = XstringAdmin.saveFile(file1, "imgTeK/");
                }
                Ma.NgayAuto = DateTime.Now;
                dbc.Entry(Ma).State = EntityState.Modified;
                dbc.SaveChanges();
                Session["ThongBaoHangSX"] = "Update hãng SX thành công.";
                var userid = int.Parse(Session["UserId"].ToString());
                var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, userid, Session["quyen"].ToString()
                        , Session["UserName"].ToString(), "Update Hãng SX-" + DateTime.Now.ToString(), "");
                //tro lai trang truoc do 
                var requestUri = Session["requestUri"] as string;
                if (requestUri != null)
                {
                    return Redirect(requestUri);
                }
                return RedirectToAction("ListThuChiTeK");
            }
            catch (Exception ex) { 

                string message = ex.Message;
                ModelState.AddModelError("", "Update Thất Bại !!!!"+message);

                return View(Ma);
            }
        }
    }
}