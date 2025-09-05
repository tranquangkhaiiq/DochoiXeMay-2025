using DoChoiXeMay.Filters;
using DoChoiXeMay.Models;
using DoChoiXeMay.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DoChoiXeMay.Areas.Admin.Controllers
{
    [Protect]
    public class ChiNhanhController : Controller
    {
        // GET: Admin/ChiNhanh
        Model1 dbc = new Model1();
        public ActionResult Index()
        {
            Session["requestUri"] = "/Admin/ChiNhanh/Index";
            return View();
        }
        public ActionResult InsertAutoSNChiNhanh()
        {
            try
            {
                var userid = int.Parse(Session["UserId"].ToString());
                Ser_XuatSN_CN model = new Ser_XuatSN_CN();
                model.UserId = userid;
                model.SoLuong = 0;
                model.DaAdd = 0;
                model.NgayXuat = DateTime.Now;
                model.IdKyxuat = 1;
                model.IdChiNhanh = 1;
                model.Ghichu = "Mới tạo auto";
                dbc.Ser_XuatSN_CN.Add(model);
                dbc.SaveChanges();
                Session["ThongBaoXuatSN_ChiNhanh"] = "Auto xuất S/N cho ChiNhanh thành công, cần update - Số Lượng - Kỳ Xuất và ChiNhanh.";
                var nhatky = Data.XuatNhapData.InsertNhatKy_Admin(dbc, userid, Session["quyen"].ToString()
                            , Session["UserName"].ToString(), "Xuất 0 serial cho chi nhánh - " + model.IdChiNhanh + "-" + DateTime.Now.ToString(), "");
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
                return RedirectToAction("Index");
            }
        }
        public ActionResult GetListSNchinhanh()
        {
            var model = dbc.Ser_XuatSN_CN.OrderByDescending(kh => kh.Id)
                .ThenByDescending(kh => kh.IdChiNhanh)
                .ThenByDescending(kh=>kh.SoLuong).ToList();
            ViewBag.GetListSNchinhanh = model;
            return PartialView(model);
        }
        public ActionResult DeleteSNChiNhanh( int Id) {
            var modelchitiet = dbc.Ser_Chitiet_XuatSN_CN.FirstOrDefault(kh => kh.IdSN_CN==Id);
            if(modelchitiet == null)
            {
                var model = dbc.Ser_XuatSN_CN.Find(Id);
                dbc.Ser_XuatSN_CN.Remove(model);
                dbc.SaveChanges();
                Session["ThongBaoXuatSN_ChiNhanh"] = "Delete đợt S/N cho ChiNhanh thành công.";
            }
            else
            {
                Session["ThongBaoXuatSN_ChiNhanh"] = "";
                Session["ThongBaoLoiSN_ChiNhanh"] = "Đã có S/N add chi nhánh, Delete đợt S/N cho ChiNhanh thất bại !!!";
            }
            //tro lai trang truoc do 
            var requestUri = Session["requestUri"] as string;
            if (requestUri != null)
            {
                return Redirect(requestUri);
            }
            return RedirectToAction("Index");
        }
        public ActionResult UpdateSNchoCN(int Id)
        {
            var model = dbc.Ser_XuatSN_CN.Find(Id);
            ViewBag.IdKyxuat = new SelectList(dbc.KyXuatNhaps.Where(kh => kh.AdminXNPUSH == true && kh.UPush==true), "Id", "TenKy", model.IdKyxuat);
            ViewBag.IdChiNhanh = new SelectList(dbc.Ser_ChiNhanh.Where(kh => kh.Sudung == true), "Id", "TenChiNhanh", model.IdChiNhanh);
            return View(model);
        }
        [HttpPost]
        public ActionResult UpdateSNchoCN(Ser_XuatSN_CN model)
        {
            try
            {
                model.NgayXuat = DateTime.Now;
                if(model.Ghichu=="" || model.Ghichu == "Mới tạo auto")
                {
                    model.Ghichu = "Đã update ngày: "+DateTime.Now.ToShortDateString();
                }
                dbc.Entry(model).State = EntityState.Modified;
                dbc.SaveChanges();
                if(model.SoLuong>0 && model.IdKyxuat>1 && model.IdChiNhanh > 1)
                {
                    Session["ThongBaoLoiSN_ChiNhanh"] = "";
                    Session["ThongBaoXuatSN_ChiNhanh"] = "Update thành công, bạn đã có thể add S/N cho ChiNhanh.";
                }
                else
                {
                    Session["ThongBaoLoiSN_ChiNhanh"] = "";
                    Session["ThongBaoXuatSN_ChiNhanh"] = "Update thành công. Bạn vẫn chưa được add S/N cho ChiNhanh.";
                }
                //tro lai trang truoc do 
                var requestUri = Session["requestUri"] as string;
                if (requestUri != null)
                {
                    return Redirect(requestUri);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex) {
                string message = ex.Message;
                ViewBag.IdKyxuat = new SelectList(dbc.KyXuatNhaps.Where(kh => kh.AdminXNPUSH == true && kh.UYeuCauThuHoi == false), "Id", "TenKy", model.IdKyxuat);
                ViewBag.IdChiNhanh = new SelectList(dbc.Ser_ChiNhanh.Where(kh => kh.Sudung == true), "Id", "TenChiNhanh", model.IdChiNhanh);
                ModelState.AddModelError("", "Update Thất Bại !!!!" + message);
                return View(model);
            }
        }
        public ActionResult AddSN_ChiNhanh(int Id)
        {
            var model = dbc.Ser_Chitiet_XuatSN_CN.Where(kh => kh.IdSN_CN == Id)
                                .OrderBy(kh=>kh.NgayXuat)
                                .ToList();
            ViewBag.GetListChitietSN_CN=model;
            Session["TenChiNhanh"] = new Data.ChiNhanhData().GetChiNhanhByIdXuat(Id).TenChiNhanh;
            Session["KyXuatNhap"] = new Data.ChiNhanhData().GetKyByIdXuat(Id).TenKy;
            Session["IdSN_CN"] = Id;
            Session["DaAdd"] = model.Count();
            Session["SoLuong"] = dbc.Ser_XuatSN_CN.Find(Id).SoLuong;
            for (int i = 0; i < model.Count(); i++)
            {
                model[i].Ghichu = (i + 1).ToString();
            }
            ViewBag.GetListSNtoNPP=model.OrderByDescending(kh=>kh.NgayXuat).ToList();
            ViewBag.GetCountSNtoNPP = model.Count();
            return View(model);
        }
        public ActionResult TimSNLoHang(string Serial)
        {
            var modelct = dbc.Ser_Chitiet_XuatSN_CN.FirstOrDefault(kh => kh.Serial == Serial);
            if (modelct != null)
            {
                Session["ThongBaoLoiSN_ChiNhanh"] = "";
                Session["ThongBaoXuatSN_ChiNhanh"] = "Số serial: " + Serial + " đã nằm trong lô hàng của chi nhánh "
                    + modelct.Ser_XuatSN_CN.Ser_ChiNhanh.TenChiNhanh + " có Id=" + modelct.IdSN_CN + ".";
            }
            else
            {
                Session["ThongBaoLoiSN_ChiNhanh"] = "";
                Session["ThongBaoXuatSN_ChiNhanh"] = "Số serial: " + Serial + " vẫn CHƯA add lô hàng nào.";
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddSNCNtoChiTiet(string Serial)
        {
            
            if (Session["IdSN_CN"] != null && Serial.Trim() !="")
            {
                var Id = int.Parse(Session["IdSN_CN"].ToString());
                var modelctxuat = dbc.Ser_Chitiet_XuatSN_CN.Where(kh => kh.IdSN_CN == Id).ToList();
                var soluong = dbc.Ser_XuatSN_CN.Find(Id).SoLuong;
                var checkSNdain = dbc.Ser_box.FirstOrDefault(kh => kh.Serial == Serial);
                try
                {
                    var modelct = dbc.Ser_Chitiet_XuatSN_CN.FirstOrDefault(kh => kh.Serial == Serial);
                    if (modelct != null)
                    {
                        Session["ThongBaotaolohang"] = "";
                        Session["ThongBaoLoitaolohang"] = "Số serial: " + Serial + " đã nằm trong lô hàng của chi nhánh "
                            + modelct.Ser_XuatSN_CN.Ser_ChiNhanh.TenChiNhanh + " có Id=" + modelct.IdSN_CN + ", không thể add !!!";
                    }else if (modelctxuat.Count()== soluong)
                    {
                        Session["ThongBaotaolohang"] = "";
                        Session["ThongBaoLoitaolohang"] = "Đã add đủ S/N cho lô hàng này, không thể add thêm !!!";
                    }
                    else if (checkSNdain ==null)
                    {
                        Session["ThongBaotaolohang"] = "";
                        Session["ThongBaoLoitaolohang"] = "S/N Không tồn tại, không thể add !!!";
                    }
                    else if (checkSNdain != null && checkSNdain.DaIn==false)
                    {
                        Session["ThongBaotaolohang"] = "";
                        Session["ThongBaoLoitaolohang"] = "S/N Chưa In, không thể add !!!";
                    }
                    else if (checkSNdain != null && checkSNdain.DaIn == true && checkSNdain.Sudung==true)
                    {
                        Session["ThongBaotaolohang"] = "";
                        Session["ThongBaoLoitaolohang"] = "S/N đã Active, không thể add !!!";
                    }
                    else
                    {
                        Ser_Chitiet_XuatSN_CN model = new Ser_Chitiet_XuatSN_CN();
                        model.Id = Guid.NewGuid();
                        model.Serial = Serial;
                        model.IdSN_CN = int.Parse(Session["IdSN_CN"].ToString());
                        model.NgayXuat = DateTime.Now;
                        dbc.Ser_Chitiet_XuatSN_CN.Add(model);
                        dbc.SaveChanges();
                        modelctxuat = dbc.Ser_Chitiet_XuatSN_CN.Where(kh => kh.IdSN_CN == Id).ToList();
                        Session["DaAdd"] = modelctxuat.Count();
                        Session["ThongBaoLoitaolohang"] = "";
                        Session["ThongBaotaolohang"] = "Thành công add S/N: " + Serial + " cho Chi nhánh";
                        //Update DaAdd cho Ser_XuatSN_CN
                        var modelXuat = dbc.Ser_XuatSN_CN.Find(int.Parse(Session["IdSN_CN"].ToString()));
                        modelXuat.DaAdd = modelctxuat.Count();
                        dbc.Entry(modelXuat).State = EntityState.Modified;
                        dbc.SaveChanges();
                    }
                }
                catch (Exception ex) {
                    string message = ex.Message;
                    Id = int.Parse(Session["IdSN_CN"].ToString());
                    //modelctxuat = dbc.Ser_Chitiet_XuatSN_CN.Where(kh => kh.IdSN_CN == Id).ToList();
                    //ViewBag.GetListChitietSN_CN = modelctxuat;
                    Session["TenChiNhanh"] = new Data.ChiNhanhData().GetChiNhanhByIdXuat(Id).TenChiNhanh;
                    Session["KyXuatNhap"] = new Data.ChiNhanhData().GetKyByIdXuat(Id).TenKy;
                    Session["ThongBaotaolohang"] = "";
                    Session["ThongBaoLoitaolohang"] = "S/N: " + Serial + " Có Lỗi, không thể add !!!";
                    return RedirectToAction("AddSN_ChiNhanh", new { Id = Id });
                }
                //ViewBag.GetListChitietSN_CN = modelctxuat;
                Session["TenChiNhanh"] = new Data.ChiNhanhData().GetChiNhanhByIdXuat(Id).TenChiNhanh;
                Session["KyXuatNhap"] = new Data.ChiNhanhData().GetKyByIdXuat(Id).TenKy;
                return RedirectToAction("AddSN_ChiNhanh", new { Id = Id });
            }
            else
            {
                //tro lai trang truoc do 
                var requestUri = Session["requestUri"] as string;
                if (requestUri != null)
                {
                    return Redirect(requestUri);
                }
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public ActionResult RemoveSerialCN(string Serial)
        {
            if (Session["IdSN_CN"] != null)
            {
                var Id = int.Parse(Session["IdSN_CN"].ToString());
                var modelct = dbc.Ser_Chitiet_XuatSN_CN.FirstOrDefault(kh => kh.Serial == Serial && kh.IdSN_CN == Id);
                try
                {
                    if (modelct != null)
                    {
                        var model = dbc.Ser_Chitiet_XuatSN_CN.Find(modelct.Id);
                        dbc.Ser_Chitiet_XuatSN_CN.Remove(model);
                        var kq = dbc.SaveChanges();
                        if (kq > 0)
                        {
                            var modelctxuat = dbc.Ser_Chitiet_XuatSN_CN.Where(kh => kh.IdSN_CN == Id).ToList();
                            Session["DaAdd"] = modelctxuat.Count();
                            Session["ThongBaoLoitaolohang"] = "";
                            Session["ThongBaotaolohang"] = "Remove thành công S/N: " + Serial + " ra khỏi lô hàng, chúc mừng.";
                            //Update DaAdd cho Ser_XuatSN_CN
                            var modelXuat = dbc.Ser_XuatSN_CN.Find(int.Parse(Session["IdSN_CN"].ToString()));
                            modelXuat.DaAdd = modelctxuat.Count();
                            dbc.Entry(modelXuat).State = EntityState.Modified;
                            dbc.SaveChanges();
                        }
                    }
                    else
                    {
                        Session["ThongBaotaolohang"] = "";
                        Session["ThongBaoLoitaolohang"] = "S/N: " + Serial + " Không tồn tại trong lô hàng, không thể remove !!!";
                    }
                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                    Id = int.Parse(Session["IdSN_CN"].ToString());
                    Session["TenChiNhanh"] = new Data.ChiNhanhData().GetChiNhanhByIdXuat(Id).TenChiNhanh;
                    Session["KyXuatNhap"] = new Data.ChiNhanhData().GetKyByIdXuat(Id).TenKy;
                    Session["ThongBaotaolohang"] = "";
                    Session["ThongBaoLoitaolohang"] = "S/N: " + Serial + " Có Lỗi, không thể remove !!!";
                    return View("AddSN_ChiNhanh");
                }
                Session["TenChiNhanh"] = new Data.ChiNhanhData().GetChiNhanhByIdXuat(Id).TenChiNhanh;
                Session["KyXuatNhap"] = new Data.ChiNhanhData().GetKyByIdXuat(Id).TenKy;
                return RedirectToAction("AddSN_ChiNhanh", new { Id = Id });
            }
            else {
                //tro lai trang truoc do 
                var requestUri = Session["requestUri"] as string;
                if (requestUri != null)
                {
                    return Redirect(requestUri);
                }
                return RedirectToAction("Index");
            }
            
        }
        [HttpPost]
        [ValidateInput(false)]
        public EmptyResult Export(string GridHtml)
        {
            var html = XString.EditStringCV(GridHtml);
            string htmlcv = html;

            string _fileCSS = Server.MapPath("~/Areas/Admin/Content/css/Info_Style.css");
            string _strCSS = System.IO.File.ReadAllText(_fileCSS);
            StringBuilder strBody = new StringBuilder();
            strBody.Append("<html " +
             " xmlns:o='urn:schemas-microsoft-com:office:office' " +
             " xmlns:w='urn:schemas-microsoft-com:office:word'" +
              " xmlns='http://www.w3.org/TR/REC-html40'>" +
              "<head><title>Invoice Sample</title>");
            strBody.Append("<xml>" +
            "<w:WordDocument>" +
            " <w:View>Print</w:View>" +
            " <w:Zoom>50</w:Zoom>" +
            " <w:DoNotOptimizeForBrowser/>" +
            " </w:WordDocument>" +
            " </xml>");

            strBody.Append("<style>" + _strCSS + "</style></head>");
            //strBody.Append("<body lang=EN-US style='tab-interval:.5in'>" + "<div class=Section1>");
            strBody.Append("<body><div class='page-settings'>" + htmlcv + "</div></body></html>");
            //strBody.Append("</div></body></html>");
            Response.AppendHeader("Content-Type", "application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml");
            Response.AppendHeader("Content-disposition", "attachment;filename=myword.doc");
            Response.Write(strBody.ToString());

            return new EmptyResult();
        }
    }
}