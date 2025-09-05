using DoChoiXeMay.Areas.Admin.Data;
using DoChoiXeMay.Filters;
using DoChoiXeMay.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoChoiXeMay.Areas.Admin.Controllers
{
    [Protect]
    public class ProjectTeKController : Controller
    {
        // GET: Admin/ProjectTeK
        Model1 dbc = new Model1();
        string DBname = ConfigurationManager.AppSettings["DBname"];
        public ActionResult Index()
        {
            Session["requestUri"] = "/Admin/ProjectTeK/Index";
            ViewBag.ListProJect = new Data.ProjectTeKData().GetListProjectTeK();
            return View();
        }
        public ActionResult InsertProJectTeK()
        {
            var userid = int.Parse(Session["UserId"].ToString());
            ProjectTeK model = new ProjectTeK();
            model.NameProject = "New ProjectTek " + DateTime.Now;
            model.Giaithich = "Giải thích là kiểu bài tập làm văn có nhiệm vụ trình bày, giảng giải, cắt nghĩa về nội dung, sự vật và lời nói của con người.";
            model.DateBegin = DateTime.Now;
            model.DateEnd = DateTime.Now.AddMonths(3);
            model.Uutien = true;
            model.TrangthaiId = 1;
            model.GroupId = 1;
            model.PhantramHoanThanh = 0;
            model.NgayCapNhat = DateTime.Now;
            model.GhiChu = "";
            model.UserId = userid;
            dbc.ProjectTeKs.Add(model);
            int kt = dbc.SaveChanges();
            if (kt > 0)
            {
                Session["ThongBaoProject"] = "Insert thành công Dự Án mới, bạn phải update để sử dụng.";
            }
            else
            {
                Session["ThongBaoProject"] = "Insert Dự Án mới thất bại !!!.";
            }
            var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, userid, Session["quyen"].ToString()
                        , Session["UserName"].ToString(), "Insert Dự Án -" + model.NameProject + "-" + DateTime.Now.ToString(), "");
            //tro lai trang truoc do 
            var requestUri = Session["requestUri"] as string;
            if (requestUri != null)
            {
                return Redirect(requestUri);
            }
            return RedirectToAction("Index");
        }
        public ActionResult DeleteProject(int id)
        {
            var userid = int.Parse(Session["UserId"].ToString());
            var model = dbc.ProjectTeKs.Find(id);
            try
            {
                dbc.ProjectTeKs.Remove(model);
                dbc.SaveChanges();
                Session["ThongBaoProject"] = "Delete Dự Án " + model.NameProject + " thành công.";
                var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, userid, Session["quyen"].ToString()
                            , Session["UserName"].ToString(), "Delete Dự Án -" + model.NameProject + "-" + DateTime.Now.ToString(), "");
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
                var mess = ex.Message;
                Session["ThongBaoProject"] = "(đã có thành viên tham gia)Delete Dự Án " + model.NameProject + " không thành công !!!.";
                return RedirectToAction("Index");
            }
        }
        public ActionResult AdminDeleteProjectDetail(string id)
        {
            var userid = int.Parse(Session["UserId"].ToString());
            //
            var Deid = new Guid(id);
            var model = dbc.ProjectDetails.Find(Deid);
            var modeltek = dbc.ProjectTeKs.Find(model.ProjectId);
            var user = dbc.UserTeks.Find(model.UserId);
            try
            {
                dbc.ProjectDetails.Remove(model);
                dbc.SaveChanges();
                Session["ThongBaoProject"] = "Xóa (cưỡng bức) User cho project: '" + modeltek.NameProject +"' thành công.";
                var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, userid, Session["quyen"].ToString()
                            , Session["UserName"].ToString(), "Xóa (cưỡng bức) User cho project: '" + modeltek.NameProject + "-" + DateTime.Now.ToString(), "");
                return RedirectToAction("Index");
            }
            catch (Exception ex) {
                var mess = ex.Message;
                Session["ThongBaoProject"] = "(Có lỗi)Xóa User tham gia của Project: " + modeltek.NameProject + " không thành công !!!.";
                return RedirectToAction("Index");
            }
        }
            public ActionResult AdminDeleteProjectUserDetail(string id)
        {
            var userid = int.Parse(Session["UserId"].ToString());
            //
            var Deid = new Guid(id);
            var model = dbc.ProjectUserDetails.Find(Deid);
            var prodetail = dbc.ProjectDetails.Find(model.ProjectDetailId);
            var user = dbc.UserTeks.Find(prodetail.UserId);
            try
            {
                dbc.ProjectUserDetails.Remove(model);
                dbc.SaveChanges();
                Session["ThongBaoProject"] = "Xóa (cưỡng bức) công việc: '" + model.CongViec + "- của " + user.UserName + "' thành công.";
                var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, userid, Session["quyen"].ToString()
                            , Session["UserName"].ToString(), "Delete (cưỡng bức) Công Việc -" + model.CongViec + " cho User: " + user.UserName + "-" + DateTime.Now.ToString(), "");
                var kq = new Data.ProjectTeKData().updatephantramht(prodetail.ProjectId);
                if (kq == 100)
                {
                    var kqp = new Data.ProjectTeKData().UpdatetrangthaiProDetail(prodetail.ProjectId);
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
                var mess = ex.Message;
                Session["ThongBaoProject"] = "(Có lỗi)Xóa Công Việc cho: " + user.UserName + " không thành công !!!.";
                return RedirectToAction("Index");
            }
        }
        public ActionResult DeleteProjectUserDetail(string id)
        {
            var userid = int.Parse(Session["UserId"].ToString());
            //
            var Deid = new Guid(id);
            var model = dbc.ProjectUserDetails.Find(Deid);
            var prodetail = dbc.ProjectDetails.Find(model.ProjectDetailId);
            var user = dbc.UserTeks.Find(prodetail.UserId);
            try
            {
                if (model.TrangthaiId != 5)
                {
                    dbc.ProjectUserDetails.Remove(model);
                    dbc.SaveChanges();
                    Session["ThongBaoProject"] = "Xóa công việc: '" + model.CongViec + "- của " + user.UserName + "' thành công.";
                    var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, userid, Session["quyen"].ToString()
                                , Session["UserName"].ToString(), "Delete Công Việc -" + model.CongViec + " cho User: " + user.UserName + "-" + DateTime.Now.ToString(), "");
                    var kq = new Data.ProjectTeKData().updatephantramht(prodetail.ProjectId);
                    if (kq == 100)
                    {
                        var kqp = new Data.ProjectTeKData().UpdatetrangthaiProDetail(prodetail.ProjectId);
                    }
                    //tro lai trang truoc do 
                    var requestUri = Session["requestUri"] as string;
                    if (requestUri != null)
                    {
                        return Redirect(requestUri);
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    Session["ThongBaoProject"] = "(Công Việc đã hoàn thành) Không thể xóa!!!.";
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                var mess = ex.Message;
                Session["ThongBaoProject"] = "(Có lỗi)Xóa Công Việc cho: " + user.UserName + " không thành công !!!.";
                return RedirectToAction("Index");
            }
        }
        public ActionResult UpdateProjectTeK(int id)
        {
            var model = dbc.ProjectTeKs.Find(id);
            ViewBag.TrangthaiId = new SelectList(dbc.TrangThaiDuAns.ToList(), "Id", "Name", model.TrangthaiId);
            ViewBag.User = dbc.UserTeks.Where(kh=>kh.IdLoai<3).OrderBy(kh => kh.Id).ToList();
            ViewBag.UserC = dbc.UserTeks.Where(kh => kh.IdLoai < 3).OrderBy(kh => kh.Id).Count();
            ViewBag.ProjectDetail = dbc.ProjectDetails.Where(kh => kh.ProjectId == id).OrderBy(kh => kh.Id).ToList();
            ViewBag.CountProjectDetail = dbc.ProjectDetails.Where(kh => kh.ProjectId == id).OrderBy(kh => kh.Id).Count();
            return View(model);
        }
        [HttpPost]
        public ActionResult UpdateProjectTeK(ProjectTeK teK)
        {
            try
            {
                //Nếu User đã có thì giữ lại, user mới thì thêm vào, User không được chọn thì xóa
                var leadid = "";
                var userid = int.Parse(Session["UserId"].ToString());
                string[] ProjectDetail = (string[])Session["ProjectTeK"];
                string[] lead = (string[])Session["Projectlead"];
                bool project = new Data.ProjectTeKData().UPdateProjectTeK(teK);
                if (project)
                {
                    if (Session["ProjectTeK"] != null)
                    {
                        //Xóa ProjectTeK detail không được chọn
                        var listpd = new Data.ProjectTeKData().getlistProjectDetail(teK.Id);
                        for (int i = 0; i < listpd.Count; i++)//duyệt ds củ
                        {
                            int dem = 0;
                            for (int j = 0; j < ProjectDetail.Count(); j++)
                            {
                                if (listpd[i].UserId == int.Parse(ProjectDetail[j]))
                                {
                                    dem = dem + 1;//nếu trùng ds mới thì ++
                                    break;
                                }
                            }
                            if (dem == 0)//nếu không trùng ds mới thì xóa
                            {
                                var XoaProdetail = dbc.Database.ExecuteSqlCommand
                                     ("DELETE  FROM [" + DBname + "TechZone].[dbo].[ProjectDetail] where Id='" + listpd[i].Id.ToString() + "'");
                            }
                        }
                        //duyệt ds mới, //Thêm mới ProjectTeK detail
                        if (lead != null && lead[0].Length > 4)
                        {
                            leadid = Data.XstringAdmin.Cutstring_getID(lead[0]);
                        }
                        for (int j = 0; j < ProjectDetail.Count(); j++)
                        {
                            var pd = new ProjectTeKData().getProjectDetail(teK.Id, int.Parse(ProjectDetail[j]));
                            if (pd != null)//đã tồn tại
                            {
                                var p = dbc.ProjectDetails.Find(pd.Id);
                                if (leadid == ProjectDetail[j] && p.Leader == false)
                                {
                                    p.Leader = true;
                                    p.NgayUpdate = DateTime.Now;
                                    var updatep = new ProjectTeKData().UPdateProjectDetail(p);
                                }
                                if (p.Leader == true && leadid != ProjectDetail[j])
                                {
                                    p.Leader = false;
                                    p.NgayUpdate = DateTime.Now;
                                    var updatep = new ProjectTeKData().UPdateProjectDetail(p);
                                }
                            }
                            else
                            {
                                //Chọn lại Leader
                                if (leadid == ProjectDetail[j])//Leader
                                {
                                    new Data.ProjectTeKData().InsertProjecDetail(int.Parse(ProjectDetail[j]), teK.Id, true);
                                }
                                else
                                {
                                    new Data.ProjectTeKData().InsertProjecDetail(int.Parse(ProjectDetail[j]), teK.Id, false);
                                }
                            }
                        }
                        @Session["ThongBaoProject"] = "Update thành công Project: " + teK.NameProject;
                        var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, userid, Session["quyen"].ToString()
                            , Session["UserName"].ToString(), "Update thành công Project: " + teK.NameProject, "");

                        //tro lai trang truoc do 
                        var requestUri = Session["requestUri"] as string;
                        if (requestUri != null)
                        {
                            return Redirect(requestUri);
                        }
                    }
                    //update projectDetail trạng thái
                    var kqp = new Data.ProjectTeKData().UpdatetrangthaiProDetail(teK.Id);

                }
                else
                {
                    ModelState.AddModelError("", "Update Project Thất Bại !!!!");
                    ViewBag.TrangthaiId = new SelectList(dbc.TrangThaiDuAns.ToList(), "Id", "Name", teK.TrangthaiId);
                    ViewBag.User = dbc.UserTeks.OrderBy(kh => kh.Id).ToList();
                    ViewBag.UserC = dbc.UserTeks.OrderBy(kh => kh.Id).Count();
                    return View(teK);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                ModelState.AddModelError("", "(User đã có công việc, không thể thay đổi)-Update Thất Bại !!!! ");
                ViewBag.TrangthaiId = new SelectList(dbc.TrangThaiDuAns.ToList(), "Id", "Name", teK.TrangthaiId);
                ViewBag.User = dbc.UserTeks.OrderBy(kh => kh.Id).ToList();
                ViewBag.UserC = dbc.UserTeks.OrderBy(kh => kh.Id).Count();
                ViewBag.ProjectDetail = dbc.ProjectDetails.Where(kh => kh.ProjectId == teK.Id).OrderBy(kh => kh.Id).ToList();
                ViewBag.CountProjectDetail = dbc.ProjectDetails.Where(kh => kh.ProjectId == teK.Id).OrderBy(kh => kh.Id).Count();
                return View(teK);
            }

        }
        public ActionResult InsertProjectUserDetail(string Id)
        {
            Session["PDid"] = Id;
            var detailId = new Guid(Session["PDid"].ToString());
            var uid = dbc.ProjectDetails.Find(detailId).UserId;
            ViewBag.UserName = dbc.UserTeks.Find(uid).UserName;
            //thiết kế giao diện
            return View();
        }
        [HttpPost]
        public ActionResult InsertProjectUserDetail(string CV1, string CV2, string CV3)
        {
            var detailId = new Guid(Session["PDid"].ToString());
            var ProjectDetail = dbc.ProjectDetails.Find(detailId);
            var project = dbc.ProjectTeKs.Find(ProjectDetail.ProjectId);
            try
            {
                if (CV1.Trim() != "")
                {
                    var kq1 = new Data.ProjectTeKData().InsertProjecUserDetail(CV1, Session["PDid"].ToString());
                }
                if (CV2.Trim() != "")
                {
                    var kq2 = new Data.ProjectTeKData().InsertProjecUserDetail(CV2, Session["PDid"].ToString());
                }
                if (CV3.Trim() != "")
                {
                    var kq3 = new Data.ProjectTeKData().InsertProjecUserDetail(CV3, Session["PDid"].ToString());
                }
                var usertek = dbc.UserTeks.Find(ProjectDetail.UserId);
                var userid = int.Parse(Session["UserId"].ToString());
                Session["ThongBaoProject"] = "Insert công việc cho " + usertek.UserName + "-Dự Án :"+project.NameProject+"- thành công.";
                var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, userid, Session["quyen"].ToString()
                            , Session["UserName"].ToString(), "Insert Cong Viec cho : " + ProjectDetail.UserTek.UserName + "-Dự Án :" + project.NameProject, "");
                var kq = new Data.ProjectTeKData().updatephantramht(project.Id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                ModelState.AddModelError("", "Thêm CV Thất Bại !!!! " + message);
                var usertek = dbc.UserTeks.Find(ProjectDetail.UserId);
                var uid = dbc.ProjectDetails.Find(detailId).UserId;
                ViewBag.UserName = dbc.UserTeks.Find(uid).UserName;
                return RedirectToAction("InsertProjectUserDetail");
            }
        }
        public ActionResult UpdateProjectUserDetail(string Id, int TrangthaiId, int hoanthanh)
        {
            try
            {
                var model = dbc.ProjectUserDetails.Find(new Guid(Id));
                var ProjectDetail = dbc.ProjectDetails.Find(model.ProjectDetailId);
                var usertek = dbc.UserTeks.Find(ProjectDetail.UserId);
                if (ProjectDetail.TrangthaiId == 2)
                {
                    if (hoanthanh == 5)
                    {
                        model.TrangthaiId = 5;
                    }
                    else
                    {
                        model.TrangthaiId = TrangthaiId;
                    }
                    model.NgayUpdate = DateTime.Now;
                    dbc.Entry(model).State = EntityState.Modified;
                    dbc.SaveChanges();
                    //if hoàn thành công việc, update % hoàn thành cho projectTeK
                    if (hoanthanh == 5)
                    {
                        var kq= new Data.ProjectTeKData().updatephantramht(ProjectDetail.ProjectId);
                        var teK = dbc.ProjectTeKs.Find(ProjectDetail.ProjectId);
                        var kqp = new Data.ProjectTeKData().UpdatetrangthaiProDetail(teK.Id);
                        //int getCVTeK = new Data.ProjectTeKData().getCountCVProject(teK.Id);
                        //int getCVTeKOK = new Data.ProjectTeKData().getCountCVProjectHoanThanh(teK.Id);
                        //teK.PhantramHoanThanh = int.Parse(Math.Ceiling((1.0 * getCVTeKOK / getCVTeK) * 100).ToString());
                        //if (teK.PhantramHoanThanh == 100)
                        //{
                        //    teK.TrangthaiId = 5;
                        //}
                        //dbc.Entry(teK).State = EntityState.Modified;
                        //dbc.SaveChanges();
                        
                    }
                    @Session["ThongBaoProject"] = "Update trạng thái công việc cho " + usertek.UserName + " thành công.";
                    var userid = int.Parse(Session["UserId"].ToString());
                    var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, userid, Session["quyen"].ToString()
                                , Session["UserName"].ToString(), "Update thành công trang thai Cong Viec cho : " + ProjectDetail.UserTek.UserName, "");
                    //tro lai trang truoc do 
                    var requestUri = Session["requestUri"] as string;
                    if (requestUri != null)
                    {
                        return Redirect(requestUri);
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    @Session["ThongBaoProject"] = "(Thành viên " + usertek.UserName + " không ở trạng thái 'Đang diễn ra')Update trạng thái công việc cho " + usertek.UserName + " thất bại !!!.";
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                var message = ex.Message;
                var model = dbc.ProjectUserDetails.Find(new Guid(Id));
                var ProjectDetail = dbc.ProjectDetails.Find(model.ProjectDetailId);
                var usertek = dbc.UserTeks.Find(ProjectDetail.UserId);
                @Session["ThongBaoProject"] = "(Có Lỗi)Update trạng thái công việc cho " + usertek.UserName + " thất bại !!!.";
                return RedirectToAction("Index");
            }
        }
        public ActionResult UpdateProjectDetail(string Id)
        {

            var ProjectDetail = dbc.ProjectDetails.Find(new Guid(Id));
            var usertek = dbc.UserTeks.Find(ProjectDetail.UserId);

            if (ProjectDetail.TrangthaiId != 4)
            {
                ProjectDetail.TrangthaiId = 4;
            }
            else ProjectDetail.TrangthaiId = 2;
            ProjectDetail.NgayUpdate = DateTime.Now;
            dbc.Entry(ProjectDetail).State = EntityState.Modified;
            var kq = dbc.SaveChanges();
            if (kq > 0)
            {
                @Session["ThongBaoProject"] = "Update trạng thái công việc cho " + usertek.UserName + " thành công.";
            }
            else
            {
                @Session["ThongBaoProject"] = "(Có Lỗi)Update trạng thái công việc cho " + usertek.UserName + " thất bại !!!.";
            }


            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult setSession(string[] Id, string[] lead)
        {
            Session["ProjectTeK"] = Id;
            Session["Projectlead"] = lead;
            return Json("ok", JsonRequestBehavior.AllowGet);
        }
    }
}