using DoChoiXeMay.Areas.Admin.Data;
using DoChoiXeMay.Filters;
using DoChoiXeMay.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace DoChoiXeMay.Areas.Admin.Controllers
{
    [Protect]
    public class NoteKyThuatController : Controller
    {
        // GET: Admin/NoteKyThuat
        Model1 dbc = new Model1();
        public ActionResult Index()
        {// video huong dan Tek
            Session["requestUri"] = "/Admin/NoteKyThuat/Index";
            ViewBag.ViDeo = new Data.NoteKyThuatData().GetListNotebyHD(1);
            ViewBag.ViDeo1 = new Data.NoteKyThuatData().Get1ListNotebyHD(1);
            return View();
        }
        public ActionResult VDTEK()
        {//video quang cao tek
            Session["requestUri"] = "/Admin/NoteKyThuat/VDTEK";
            ViewBag.ViDeo = new Data.NoteKyThuatData().GetListNotebyHD(2);
            ViewBag.ViDeo1 = new Data.NoteKyThuatData().Get1ListNotebyHD(2);
            return View();
        }
        public ActionResult InsertVD(int loai)
        {
            var Max = 1;
            if (dbc.NoteKythuats.Count() > 0)
            {
                Max = dbc.NoteKythuats.OrderByDescending(kh => kh.Id).Take(1).Single().Id;
            }
            var userid = int.Parse(Session["UserId"].ToString());
            NoteKythuat model = new NoteKythuat();
            model.Id = Max + 1;
            model.NoteName = "Video New TeK.";
            model.NoiDung = "Cần update để sử dụng";
            model.Stt = "";
            model.UserId = userid;
            model.UPush = true;
            model.PushtoNoteId = 1;
            model.AdminXacNhan = true;
            model.IdHanhDong = 0; // video=0
            model.LoaiNoteId = loai;
            dbc.NoteKythuats.Add(model);
            dbc.SaveChanges();
            Session["ThongBaoVDTEK"] = "Insert Video thành công. Cần Update để sử dụng.";

            var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, userid, Session["quyen"].ToString()
                        , Session["UserName"].ToString(), "Insert Video -" + model.NoteName + "-" + DateTime.Now.ToString(), "");
            //tro lai trang truoc do 
            var requestUri = Session["requestUri"] as string;
            if (requestUri != null)
            {
                return Redirect(requestUri);
            }
            return RedirectToAction("ListThuChiTeK");
        }
        public ActionResult DeleteVideo(int id)
        {
            var model = dbc.NoteKythuats.Find(id);
            dbc.NoteKythuats.Remove(model);
            dbc.SaveChanges();
            Session["ThongBaoVDTEK"] = "Delete Video thành công.";
            //SMS hệ thống
            var sms = "Delete Video -" + model.NoteName+", thành công.";
            new Data.UserData().SMSvaNhatKy(dbc, Session["UserId"].ToString(), Session["UserName"].ToString()
                , Session["quyen"].ToString(), sms);

            //tro lai trang truoc do 
            var requestUri = Session["requestUri"] as string;
            if (requestUri != null)
            {
                return Redirect(requestUri);
            }
            return RedirectToAction("Index");
        }
        public ActionResult EditVideo(int id)
        {
            var model = dbc.NoteKythuats.Find(id);
            ViewBag.IdHanhDong = new SelectList(dbc.HanhDongs.ToList(), "Id", "TenHD", model.IdHanhDong);
            return View(model);
        }
        [HttpPost]
        public ActionResult EditVideo(NoteKythuat model)
        {
            try
            {
                var userid = int.Parse(Session["UserId"].ToString());
                model.UserId = userid;
                dbc.Entry(model).State = EntityState.Modified;
                dbc.SaveChanges();
                Session["ThongBaoVDTEK"] = "Update Video thành công.";
                var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, userid, Session["quyen"].ToString()
                            , Session["UserName"].ToString(), "Delete Video -" + model.NoteName + "-" + DateTime.Now.ToString(), "");
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
                string message = ex.Message;
                ModelState.AddModelError("", "Update Thất Bại !!!!" + message);
                return View(model);
            }

        }
        public ActionResult HinhTrangChu()
        {
            Session["requestUri"] = "/Admin/NoteKyThuat/HinhTrangChu";

            return View();
        }
        public ActionResult GetHinhTrangChu()
        {
            var model = dbc.QCtrangchus.OrderByDescending(kh => kh.Id)
                .ThenByDescending(kh => kh.Idvitri)
                .ThenByDescending(kh => kh.Img).ToList();
            ViewBag.GetHinhTrangChu = model;
            return PartialView(model);
        }
        [HttpGet]
        public ActionResult InsertAotuHinhTrangChu()
        {
            try
            {
                QCtrangchu model = new QCtrangchu();
                model.Name = "Auto";
                model.Ngay = DateTime.Now;
                model.Idvitri = 1;
                model.Sudung = false;
                model.Img = true;
                model.Idloai_socials = 1;
                model.Urlsocials = "";
                model.Ghichu = "Nội dung Auto";
                dbc.QCtrangchus.Add(model);
                dbc.SaveChanges();
                Session["ThongBaoHinhtrangchu"] = "Thêm mới hình Aotu thành công, cần update để sử dụng.";
                return RedirectToAction("HinhTrangChu");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Session["ThongBaoHinhtrangchu"] = "Thêm mới hình Aotu thất bại !!!." + message;
                return RedirectToAction("HinhTrangChu");
            }

        }
        public ActionResult DeleteHTC(int Id)
        {
            try
            {
                var model = dbc.QCtrangchus.Find(Id);
                dbc.QCtrangchus.Remove(model);
                dbc.SaveChanges();
                Session["ThongBaoHinhtrangchu"] = "Delete hình Thành Công.";
                return RedirectToAction("HinhTrangChu");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Session["ThongBaoHinhtrangchu"] = "Delete hình thất bại !!!." + message;
                return RedirectToAction("HinhTrangChu");
            }

        }
        public ActionResult UpdateHTC(int Id)
        {
            var model = dbc.QCtrangchus.Find(Id);
            ViewBag.Idvitri = new SelectList(dbc.QCVitris.ToList(), "Id", "Vitri", model.Idvitri);
            ViewBag.Idloai_socials = new SelectList(dbc.Loai_Socials.ToList(), "Id", "Loai", model.Idloai_socials);
            return View(model);
        }
        [HttpPost]
        public ActionResult UpdateHTC(QCtrangchu model)
        {
            try
            {
                var file1 = Request.Files["Dinhkem1"];
                if (file1.ContentLength > 0)
                {
                    //Xoa hinh cu
                    if (model.Idvitri == 1)
                    {
                        bool xoahinhcu = XstringAdmin.Xoahinhcu("images/", model.Name);
                        model.Name = XstringAdmin.saveFile(file1, "images/");
                    }
                    if (model.Idvitri == 2)
                    {
                        bool xoahinhcu = XstringAdmin.Xoahinhcu("images/clients/", model.Name);
                        model.Name = XstringAdmin.saveFile(file1, "images/clients/");
                    }
                }
                model.Ngay = DateTime.Now;
                dbc.Entry(model).State = EntityState.Modified;
                dbc.SaveChanges();
                Session["ThongBaoHinhtrangchu"] = "Update hình trang chủ: Id= " + model.Id + " Thành Công.";
                //SMS hệ thống
                string sms = "Update hình trang chủ: Id= " + model.Id + " Thành Công.";
                new Data.UserData().SMSvaNhatKy(dbc, Session["UserId"].ToString(), Session["UserName"].ToString()
                    , Session["quyen"].ToString(), sms);
                //tro lai trang truoc do 
                var requestUri = Session["requestUri"] as string;
                if (requestUri != null)
                {
                    return Redirect(requestUri);
                }
                return RedirectToAction("HinhTrangChu");
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                ModelState.AddModelError("", "Update hình trang chủ Thất Bại !!!!");
                var model1 = dbc.QCtrangchus.Find(model.Id);
                return View(model1);
            }
        }
    }
}