using DoChoiXeMay.Models;
using MaHoa_GiaiMa_TaiKhoan;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using static System.Net.Mime.MediaTypeNames;

namespace DoChoiXeMay.Controllers
{
    public class HomeController : Controller
    {
        Model1 dbc = new Model1();
        public ActionResult Index()
        {
            ViewBag.GetPanelNgang   = dbc.QCtrangchus.Where(kh=>kh.Idvitri== 1 && kh.Sudung == true && kh.Img == true).ToList();
            ViewBag.GetPanelDoc     = dbc.QCtrangchus.Where(kh=>kh.Idvitri== 2 && kh.Sudung==true && kh.Img==true).ToList();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult LoginWebGuest()
        {
            ViewBag.UserName = "";
            ViewBag.Password = "";
            ViewBag.reme = "";
            return View();
        }
        [HttpPost]
        public ActionResult LoginWebGuest(String UserName, String Password)
        {
            TaiKhoanInfo tk_check = new TaiKhoanInfo();
            //tài khoản không phân biệt hoa thường.
            var user = dbc.UserTeks.Where(p => p.UserName.ToLower() == UserName.ToLower() && p.IdLoai == 5).SingleOrDefault();
            if (user != null)
            {
                var time_locked = user.LastLokedChangedate;
                var check_time = DateTime.Now - time_locked;

                // Kiểm tra xem thời giạn bị khóa trên 5 chưa ? Nếu trên 5p reset lại thành false
                if (check_time.Minutes > 4 && user.Islocked == true)
                {
                    user.Islocked = false;
                    user.CountFailedPassword = 0;
                    dbc.SaveChanges();
                }
                // Sau khi kiểm tra tình trạng khóa tài khoản thì kiểm tra đăng nhập
                if (user.Islocked == true)
                {
                    ModelState.AddModelError("", "Tài khoản của bạn đã bị khóa.Vui lòng đăng nhập sau " + (4 - check_time.Minutes) + " phút " + (60 - check_time.Seconds) + " giây");
                }
                else
                {
                    string check_pass = tk_check.EnCryptDotNetNukePassword(Password, "", user.PasswordSalt);//pass ma hoa
                    if (user.Password == check_pass)
                    {
                        int UserFirt = user.Id;
                        // đăng nhập thành công set lại số lần nhập sai = 0
                        if (user.CountFailedPassword > 0)
                        {
                            user.CountFailedPassword = 0;
                            dbc.Entry(user).State = EntityState.Modified;
                            dbc.SaveChanges();
                        }
                        string[] user_log = new string[2];
                        user_log[0] = UserName;
                        user_log[1] = Password;
                        Session["UserName"] = UserName;
                        Session["UserId"] = UserFirt;
                        //
                        Session["quyen"] = user.LoaiUserTek.Id;
                        Session["avatar"] = user.Avatar;
                        var uID = UserFirt;
                        //var model_uid = dbc.UserTeks.Find(UserFirt);
                        bool nhatky = Areas.Admin.Data.XuatNhapData.InsertNhatKy_Admin(dbc, UserFirt, Session["quyen"].ToString()
                , Session["UserName"].ToString(), "LoginWeb", "");
                        Session["idchinhanhAt"] = dbc.Ser_ChiNhanh.FirstOrDefault(kh => kh.IdUser == uID).Id;
                        ////
                        return RedirectToAction("IndexDL", "Active");
                    }
                    else
                    {
                        //đếm số lần nhập sai
                        user.CountFailedPassword = user.CountFailedPassword + 1;
                        if (user.CountFailedPassword == 5)
                        {
                            //Sai 5 lần tài khoản bị khóa
                            user.Islocked = true;
                            user.lastPasswordChangedate = DateTime.Now;
                            user.LastLokedChangedate = DateTime.Now;
                            dbc.Entry(user).State = EntityState.Modified;
                            var kqlo= dbc.SaveChanges();
                            ModelState.AddModelError("", "Nhập sai password liên tiếp 5 lần !!! Tài khoản của bạn bị khóa trong 5p !!!");
                        }
                        else
                        {
                            dbc.Entry(user).State = EntityState.Modified;
                            dbc.SaveChanges();
                            ModelState.AddModelError("", "Nhập sai password lần: " + user.CountFailedPassword + ".Tài khoản của bạn sẽ bị khóa nếu nhập sai 5 lần liên tiếp !!!");
                        }
                        //// địa chỉ đến
                        //return RedirectToAction("LoginWebGuest", "Home");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Sai tên đăng nhập");
            }
            return View();
        }
        public ActionResult Login()
        {
            ViewBag.UserName = "";
            ViewBag.Password = "";
            ViewBag.reme = "";
            return View("Login");
        }
        [HttpPost]
        public ActionResult Login(String UserName, String Password)
        {
            var tk = new UserTek();
            var requestUri = Session["requestUri"] as string;
            TaiKhoanInfo tk_check = new TaiKhoanInfo();
            //tài khoản không phân biệt hoa thường.
            var user = dbc.UserTeks.Where(p => p.UserName.ToLower() == UserName.ToLower()).SingleOrDefault();
            if (user != null)
            {
                var time_locked = user.LastLokedChangedate;
                var check_time = DateTime.Now - time_locked;

                // Kiểm tra xem thời giạn bị khóa trên 5 chưa ? Nếu trên 5p reset lại thành false
                if (check_time.Minutes > 4 && user.Islocked == true)
                {
                    user.Islocked = false;
                    user.CountFailedPassword = 0;
                    dbc.SaveChanges();
                }
                // Sau khi kiểm tra tình trạng khóa tài khoản thì kiểm tra đăng nhập
                if (user.Islocked == true)
                {
                    ModelState.AddModelError("", "Tài khoản của bạn đã bị khóa.Vui lòng đăng nhập sau " + (4 - check_time.Minutes) + " phút " + (60 - check_time.Seconds) + " giây");
                }
                else
                {
                    string check_pass = tk_check.EnCryptDotNetNukePassword(Password, "", user.PasswordSalt);//pass ma hoa
                    if (user.Password == check_pass)
                    {
                        int UserFirt = user.Id;
                        var quyenAd = dbc.UserTeks.Where(p => p.UserName == UserName && p.IdLoai == 1).ToList();
                        var quyensub = dbc.UserTeks.Where(p => p.UserName == UserName && p.IdLoai == 2).ToList();
                        var quyenStaff = dbc.UserTeks.Where(p => p.UserName == UserName && (p.IdLoai == 3 || p.IdLoai == 4)).ToList();
                        if (UserFirt == -1 || UserFirt == 0 || quyenAd.Count == 0 && quyensub.Count == 0 && quyenStaff.Count==0)
                        {
                            ModelState.AddModelError("", "Chào bạn, tài khoản của bạn không sử dụng được trên web của chúng tôi. Liên Hệ trung tâm để được hổ trợ, hoặc tạo tài khoản mới để đăng nhập web !!");
                        }
                        else
                        {
                            // đăng nhập thành công set lại số lần nhập sai = 0
                            if (user.CountFailedPassword > 0)
                            {
                                user.CountFailedPassword = 0;
                                dbc.Entry(user).State = EntityState.Modified;
                                dbc.SaveChanges();
                            }
                            string[] user_log = new string[2];
                            user_log[0] = UserName;
                            user_log[1] = Password;
                            Session["UserName"] = UserName;
                            Session["UserId"] = UserFirt;
                            //
                            
                            if (user.IdLoai == 1 || user.IdLoai == 2)
                            {
                                Session["quyen"] = user.LoaiUserTek.Id;
                                Session["avatar"] = user.Avatar;
                                var uID = UserFirt;
                                var model_uid = dbc.UserTeks.Find(UserFirt);
                                bool nhatky = Areas.Admin.Data.XuatNhapData.InsertNhatKy_Admin(dbc, UserFirt, Session["quyen"].ToString()
                        , Session["UserName"].ToString(), "LoginWeb_Admin", "");
                                //tro lai trang truoc do 
                                if (requestUri != null)
                                {
                                    return Redirect(requestUri);
                                }
                                return RedirectToAction("ListThuChiTeK", "ThuChi", new { area = "Admin" });
                            }
                            else
                            {
                                Session["quyen"] = user.LoaiUserTek.Id;
                                var uID = UserFirt;
                                var model_uid = dbc.UserTeks.Find(uID);
                                bool nhatky = Areas.Admin.Data.XuatNhapData.InsertNhatKy_Admin(dbc, UserFirt, Session["quyen"].ToString()
                                    , Session["UserName"].ToString(), "LoginWeb_Admin", "");
                            }
                            Session.Remove("ThongbaoLogin");
                            //tro lai trang truoc do 
                            if (requestUri != null)
                            {
                                return Redirect(requestUri);
                            }
                            else { return RedirectToAction("Index", "Home"); }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Mật khẩu sai vui lòng nhập lại");
                        //đếm số lần nhập sai
                        user.CountFailedPassword = user.CountFailedPassword + 1;
                        if (user.CountFailedPassword == 5)
                        {
                            //Sai 5 lần tài khoản bị khóa
                            user.Islocked = true;
                            user.lastPasswordChangedate = DateTime.Now;
                            user.LastLokedChangedate = DateTime.Now;
                            dbc.Entry(user).State = EntityState.Modified;
                            dbc.SaveChanges();
                            ModelState.AddModelError("", "Nhập sai password liên tiếp 5 lần !!! Tài khoản của bạn bị khóa trong 5p !!!");
                        }
                        else
                        {
                            dbc.Entry(user).State = EntityState.Modified;
                            dbc.SaveChanges();
                            ModelState.AddModelError("", "Nhập sai password lần: " + user.CountFailedPassword + ".Tài khoản của bạn sẽ bị khóa nếu nhập sai 5 lần liên tiếp !!!");
                        }
                    }
                }

            }
            else
            {
                ModelState.AddModelError("", "Sai tên đăng nhập");
            }
            return View();
        }
        public ActionResult LogOut()
        {
            Session.Clear();
            return Redirect("/warranty");
        }
        public ActionResult LoadVideo()
        {
            //1=IdHanhDong:Admin & trangchu // 2=LoaiNoteId:Video global TeK
            //0 = IdHanhDong:Page Admin // 1=LoaiNoteId:Hướng dẫn TeK
            var model = dbc.NoteKythuats.Where(kh => kh.IdHanhDong == 1 && kh.LoaiNoteId == 2)
                        .OrderByDescending(kh => kh.Id)
                        .ToList();
            ViewBag.Video1_kh = model.Take(1).ToList();
            for (int i = 0; i < model.Count(); i++)
            {
                model[i].Id = i + 1;
            }
            ViewBag.Video_kh = model;
            ViewBag.totalvd = model.Count();
            return PartialView();
        }
    }
}