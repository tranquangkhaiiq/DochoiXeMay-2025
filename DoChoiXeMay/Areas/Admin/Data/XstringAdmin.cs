using DoChoiXeMay.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace DoChoiXeMay.Areas.Admin.Data
{
    public static class XstringAdmin
    {
        public static String Cutstring_getLastString(this String nn)
        {
            if (nn != null && nn != "")
            {
                string[] str1 = nn.Split('/');
                if (str1.Count() > 1)
                {
                    string tt = str1[str1.Count() - 1];
                    return str1[str1.Count() - 1];
                }
                else { return str1[0]; }

            }
            else return nn;
        }
        public static String Cutstring_getID(this String nn)
        {
            if (nn != null && nn != "")
            {
                string[] str1 = nn.Split('d');
                if (str1.Count() > 1)
                {
                    string tt = str1[str1.Count() - 1];
                    return str1[str1.Count() - 1];
                }
                else { return str1[0]; }

            }
            else return nn;
        }
        public static bool Xoahinhcu(string foder, string img)
        {
            try
            {
                if(img != null && img != "")
                {
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/Content/" + foder + img)))
                    {
                        File.Delete(HttpContext.Current.Server.MapPath("~/Content/" + foder + img));
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return false;
            }

        }
        public static string saveFile(HttpPostedFileBase File, string foder)
        {
            if (File.ContentLength > 0)
            {
                var ten = File.FileName;
                string[] str = ten.Split('.');

                var ext = str[str.Count() - 1].ToLower();
                if (ext == "jpg" || ext == "png" || ext == "jpeg" || ext == "xls" || ext == "pdf" || ext == "xlsx"
                    || ext == "doc" || ext == "docx" || ext == "txt" || ext == "gif")
                {
                    var sub = XString.MakeAotuName();
                    ten = str[str.Count() - 2] + sub + "." + ext;
                    //Không thu nhỏ hình
                    //File.SaveAs(mapweb + foder+ten);
                    File.SaveAs(HttpContext.Current.Server.MapPath("~/Content/"+ foder+ten));
                }
                else ten = "";
                return ten;
            }
            return "";
        }
        public static string Truncate(string input, int length)
        {
            string tr = "";
            if (input != null)
            {
                if (input.Length <= length)
                {
                    tr = input;
                }
                else
                {
                    tr = input.Substring(0, length) + "...";
                }
            }
            return tr;
        }
        public static bool CopyFile(string source, string destination)
        {
            try
            {
                System.IO.File.Copy(source, destination);
                return true;
            }
            catch (Exception ex)
            {
                var ms=ex.ToString();
                return false;
            }
            
            
        }
    }
}