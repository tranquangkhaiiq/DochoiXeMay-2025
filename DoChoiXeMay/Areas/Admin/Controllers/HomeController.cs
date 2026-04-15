using DoChoiXeMay.Areas.Admin.Data;
using DoChoiXeMay.Filters;
using DoChoiXeMay.Models;
using DoChoiXeMay.Utils;
using MaHoa_GiaiMa_TaiKhoan;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace DoChoiXeMay.Areas.Admin.Controllers
{
    [Protect]
    public class HomeController : Controller
    {
        // GET: Admin/Home
        Model1 dbc = new Model1();
        TaiKhoanInfo tk = new TaiKhoanInfo();
        public ActionResult Index()
        {
            ViewBag.ChiTietTC = dbc.ChiTietTCs.Where(kh => kh.AdminXacNhan == true);
            if (Session["ActiveS"] != null) Session["ActiveS"] = "li-Home";
            return View();
        }
        public ActionResult LogOut() {
            Session.Clear();
            return Redirect("/Login");
        }
        public ActionResult SetSession(string Sec)
        {
            Session["ActiveS"]=Sec;
            return Json("ok", JsonRequestBehavior.AllowGet);
        }
        public ActionResult SetSessionMB(string Sec)
        {
            Session["ActiveMB"] = Sec;
            return Json("ok", JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult EditUser(int id)
        {
            var model = dbc.UserTeks.Find(id);
            string check_pass = tk.DeCryptDotNetNukePassword(model.Password, "A872EDF100E1BC806C0E37F1B3FF9EA279F2F8FD378103CB", model.PasswordSalt);//pass ma hoa
            if (Session["quyen"].ToString() == "1")
            {
                ViewBag.passcu = check_pass;
            }
            else
            {
                ViewBag.passcu = "";
            }
            ViewBag.IdLoai = new SelectList(dbc.LoaiUserTeks.Where(kh => kh.IsLocked == false), "Id", "LoaiUser", model.IdLoai);
            return View(model);
        }
        [HttpPost]
        public ActionResult EditUser(UserTek model, string PW, string PWM, string PWMM)
        {
            string check_pass = tk.DeCryptDotNetNukePassword(model.Password, "A872EDF100E1BC806C0E37F1B3FF9EA279F2F8FD378103CB", model.PasswordSalt);//pass ma hoa
            if (PW == check_pass)
            {
                if(PWM == PWMM)
                {
                    var checkname = dbc.UserTeks.Where(kh => kh.UserName == model.UserName && kh.Id!=model.Id);
                    if (checkname.Count() == 0)
                    {
                        var file1 = Request.Files["Dinhkem1"];
                        if (file1.ContentLength > 0)
                        {
                            if(model.Avatar != "Namn.png")
                            {
                                //Xoa hinh cu
                                bool xoahinhcu = XstringAdmin.Xoahinhcu("imgTeK/", model.Avatar);
                            }
                            model.Avatar = XstringAdmin.saveFile(file1, "imgTeK/");
                        }
                        string PasswordSalt = Convert.ToBase64String(tk.GenerateSalt()); //tạo chuổi salt ngẫu nhiên
                        string cipherPass = tk.EnCryptDotNetNukePassword(PWM, "", PasswordSalt);

                        model.Password = cipherPass;
                        model.PasswordSalt = PasswordSalt;
                        model.lastPasswordChangedate = DateTime.Now;
                        dbc.Entry(model).State = EntityState.Modified;
                        dbc.SaveChanges();
                        var uid = int.Parse(Session["UserId"].ToString());
                        //SMS hệ thống
                        string sms = " đã update user: " + model.UserName + " .Thành Công.";
                        new Data.UserData().SMSvaNhatKy(dbc, Session["UserId"].ToString(), Session["UserName"].ToString()
                            , Session["quyen"].ToString(), sms);
                        @Session["ThongBaoUserTEK"] = sms;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Update Thất Bại !!!!!!!!!!. Tên đăng nhập bị trùng lặp");
                        return View();
                    }
                    
                }
                else
                {
                    ModelState.AddModelError("", "Update Thất Bại !!!!!!!!!!. PassWord Xác nhận không giống");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "Update Thất Bại !!!!!!!!!!. PassWord củ không đúng");
                return View();
            }
            
            //tro lai trang truoc do 
            var requestUri = Session["requestUri"] as string;
            if (requestUri != null)
            {
                return Redirect(requestUri);
            }
            return RedirectToAction("ListThuChiTeK","ThuChi");
        }
        public ActionResult ListUserTeK()
        {
            Session["requestUri"] = "/Admin/Home/ListUserTeK";
            return View();
        }
        public ActionResult GetListUserTeK() {
            //model.GhiChu dùng lưu hình đai dien
            var model = dbc.UserTeks.OrderByDescending(kh=>kh.Createdate).ToList();
            for (int i = 0; i < model.Count(); i++)
            {
                string check_pass = tk.DeCryptDotNetNukePassword(model[i].Password, "A872EDF100E1BC806C0E37F1B3FF9EA279F2F8FD378103CB", model[i].PasswordSalt);//pass ma hoa
                model[i].Password = check_pass;
            }
            ViewBag.UserTeK = model;
            return PartialView();
        }
        public ActionResult HangSanXuat()
        {
            var IDMF = dbc.Manufacturers.Where(kh => kh.Sudung == true).
                            Select(kh => new { Id = kh.Id, ten = kh.Name });

            return Json(IDMF, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListHangHoaTek()
        {
            Session["requestUri"] = "/Admin/Home/ListHangHoaTek";
            ViewBag.IDMF = new SelectList(dbc.Manufacturers.Where(kh => kh.Sudung == true), "Id", "Name");
            return View();
        }
        public ActionResult GetListHHTEK(int id=0, string Key="")
        {
            double tong = 0;
            List<HangHoa> model = new List<HangHoa>();
            if (id == 0 && Key=="")
            {
                model = dbc.HangHoas.OrderBy(h => h.Id).ToList();
            }
            else if(id==0 && Key !="")
            {
                model = dbc.HangHoas.Where(h => h.Ten.ToLower().Trim()
                            .Contains(Key.ToLower().Trim())).OrderBy(h => h.Id).ToList();
            }
            else if (id >0 && Key == "")
            {
                model = dbc.HangHoas.Where(h => h.IDMF == id).OrderBy(h => h.Id).ToList();
            }else if(id >0 && Key != "")
            {
                model = dbc.HangHoas.Where(h => h.IDMF == id && h.Ten.ToLower().Trim()
                            .Contains(Key.ToLower().Trim())).OrderBy(h => h.Id).ToList();
            }

            ViewBag.Tongsp = model.Sum(kh => kh.SoLuong);
            var model1= model.OrderByDescending(h => h.IdKho)
                                .ThenBy(kh=>kh.IDKy)//SP or NVL
                                .ThenByDescending(h=>h.IDMF)
                                .ThenByDescending(h => h.SoLuong)
                                .ThenByDescending(h=>h.NgayAuto)
                                .ToList();
            ViewBag.HHTEK = model1;
            for (int i = 0; i < model1.Count(); i++)
            {
                model1[i].GhiChu = (i + 1).ToString();
                tong = tong + model1[i].SoLuong * model1[i].GiaNhap;
            }
            ViewBag.TongLoai = model.Count();
            ViewBag.TongTien = tong;
            return PartialView();
        }
        public ActionResult UpdateHinhSP(int id)
        {
            var model = dbc.HangHoas.Find(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult UpdateHinhSP(HangHoa model)
        {
            try
            {
                var file1 = Request.Files["Dinhkem1"];
                var file2 = Request.Files["Dinhkem2"];
                var file3 = Request.Files["Dinhkem3"];
                if (file1.ContentLength > 0)
                {
                    //Xoa hinh cu
                    bool xoahinhcu = XstringAdmin.Xoahinhcu("imgxuatnhap/", model.Hinh1);
                    model.Hinh1 = XstringAdmin.saveFile(file1, "imgxuatnhap/");
                }
                if (file2.ContentLength > 0)
                {
                    //Xoa hinh cu
                    bool xoahinhcu = XstringAdmin.Xoahinhcu("imgxuatnhap/", model.Hinh2);
                    model.Hinh2 = XstringAdmin.saveFile(file2, "imgxuatnhap/");
                }
                if (file3.ContentLength > 0)
                {
                    //Xoa hinh cu
                    bool xoahinhcu = XstringAdmin.Xoahinhcu("imgxuatnhap/", model.Hinh3);
                    model.Hinh3 = XstringAdmin.saveFile(file3, "imgxuatnhap/");
                }
                model.NgayAuto = DateTime.Now;
                dbc.Entry(model).State = EntityState.Modified;
                var ktkq= dbc.SaveChanges();
                //new ChiTietSLHangHoas 13thang2
                //Chua co thi Insert, co roi thi update
                string DBname = ConfigurationManager.AppSettings["DBname"];
                if (ktkq > 0)
                {
                    new TonKhoData().AutoChiTietSLHangHoa(model.Id, model.SoLuong, DBname);
                }
                //SMS hệ thống
                string sms = " đã Update hình sp: " + model.Ten + " bảng HH, Thành Công.";
                new Data.UserData().SMSvaNhatKy(dbc, Session["UserId"].ToString(), Session["UserName"].ToString()
                    , Session["quyen"].ToString(), sms);
                Session["ThongBaoHangHoaTEK"] = "Update hình sp: "+model.Ten+" Thành Công.";
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

                string loi = ex.ToString();
                ModelState.AddModelError("", "Update hình sp Thất Bại !!!!");
                return View(model);
            }
            
        }
        public ActionResult InsertUser()
        {
            UserTek model = new UserTek();
            model.UserName = "Newusertek"+DateTime.Now.ToString();
            model.Password = "4+szJJPdHNwGTpohvWoq5W0FS0TGKrNhny2zvF6cf64fgvm9EvAuew==";
            model.PasswordSalt = "cwYRNpQl/Jissz6PZo/oUjHBEsYJw8w=";
            model.IdLoai = 2;
            model.LoaiConnection = "";
            model.EmailConnection = "email@gmail.com";
            model.Islocked = false;
            model.lastPasswordChangedate = DateTime.Now;
            model.LastLokedChangedate = DateTime.Now;
            model.Createdate = DateTime.Now;
            model.CountFailedPassword = 0;
            model.GhiChu = "";
            dbc.UserTeks.Add(model);
            dbc.SaveChanges();
            //tro lai trang truoc do 
            var requestUri = Session["requestUri"] as string;
            if (requestUri != null)
            {
                return Redirect(requestUri);
            }
            return RedirectToAction("ListThuChiTeK");
        }
        public ActionResult DeleteUserChiNhanh(int Id)
        {
            var UserCN = dbc.UserTeks.Find(Id);
            if (UserCN != null && UserCN.IdLoai > 4)
            {
                dbc.UserTeks.Remove(UserCN);
                dbc.SaveChanges();
                Session["ThongBaoUserTEK"] = "Delete User " + UserCN.UserName + " thành công.";
                
                //SMS hệ thống
                string sms = "Delete User: " + UserCN.UserName + " Thành Công. ";
                new Data.UserData().SMSvaNhatKy(dbc, Session["UserId"].ToString(), Session["UserName"].ToString()
                    , Session["quyen"].ToString(), sms);
            }
            else {
                Session["ThongBaoUserTEK"] = "User không xóa được !!!. Delete User " + UserCN.UserName + " Không thành công !!!.";
            }
            //tro lai trang truoc do 
            var requestUri = Session["requestUri"] as string;
            if (requestUri != null)
            {
                return Redirect(requestUri);
            }
            return RedirectToAction("ListUserTeK", "ThuChi");
        }
        public ActionResult ListKhoTek()
        {
            Session["requestUri"] = "/Admin/Home/ListKhoTek";
            return View();
        }
        public ActionResult GetListKhoTek()
        {
            var model = dbc.Khoes.ToList();
            ViewBag.GetListKhoTek = model;
            return PartialView();
        }
        public ActionResult InsertKhoAuto()
        {
            try
            {
                Kho model = new Kho();
                model.TenKho = "AutoKho";
                model.GhiChu = "Make Auto";
                model.SuDung = false;
                model.Ngay = DateTime.Now;
                dbc.Khoes.Add(model);
                dbc.SaveChanges();
                Session["ThongBaoListKhoTek"] = "Tạo mới kho Auto, cần update để sử dụng.";
                return View("ListKhoTek");
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                Session["ThongBaoListKhoTek"] = loi;
                return View("ListKhoTek");
            }
        }
        public ActionResult DeleteKho(int id)
        {
            try
            {
                var kho = dbc.Khoes.Find(id);
                dbc.Khoes.Remove(kho);
                dbc.SaveChanges();
                Session["ThongBaoListKhoTek"] = "Xóa Kho " + kho.TenKho + " thành công.";
                return View("ListKhoTek");
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                Session["ThongBaoListKhoTek"] = loi;
                return View("ListKhoTek");
            }
            
        }
        public ActionResult UpdateKho(int id)
        {
            var model = dbc.Khoes.Find(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult UpdateKho(Kho model)
        {
            try
            {
                dbc.Entry(model).State = EntityState.Modified;
                dbc.SaveChanges();
                Session["ThongBaoListKhoTek"] = "Update Kho Id=" + model.Id + ", thành công.";
                //SMS hệ thống
                var sms = "Update kho Id=" + model.Id + ", thành công.";
                new Data.UserData().SMSvaNhatKy(dbc, Session["UserId"].ToString(), Session["UserName"].ToString()
                    , Session["quyen"].ToString(), sms);
                //tro lai trang truoc do 
                var requestUri = Session["requestUri"] as string;
                if (requestUri != null)
                {
                    return Redirect(requestUri);
                }
                return View("ListKhoTek");
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                ModelState.AddModelError("", "Update Thất Bại !!!!" + loi);
                return View(model);
            }
        }
    }
}