using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoChoiXeMay.Filters
{
    public class ProtectNVAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var quyen = HttpContext.Current.Session["quyen"];
            if (quyen == null || int.Parse(quyen.ToString())>2 && int.Parse(quyen.ToString()) <5)
            {
                //HttpContext.Current.Session["Message"] = "Vui lòng đăng nhập";
                filterContext.HttpContext.Session["ThongbaoLogin"] = "Phiên làm việc đã kết thúc, vui lòng đăng nhập lại.";
                HttpContext.Current.Response.Redirect("/Login");
                return;
            }
        }
    }
}