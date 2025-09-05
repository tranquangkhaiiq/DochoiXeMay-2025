using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;

namespace DoChoiXeMay.Utils
{
    public class Mailer
    {
        static String VLVNEmail = ConfigurationManager.AppSettings["Email"];
        static String VLVNName = "Tek-lightning";
        public static string VLVNPassword = ConfigurationManager.AppSettings["matkhau"];
        //Tên ứng dụng tạo trên gmail: SMTP Mail
        //mk_smtp:tnsb bvsy xrnr fsbp
        //mk_email:P0WERtekp@ss
        public static bool Send(String Email, String Subject, String Body)
        {
            try
            {
                //Tạo thư
                var message = new MailMessage();
                message.From = new MailAddress(VLVNEmail, VLVNName);
                message.To.Add(Email);
                message.Subject = Subject;
                message.Body = Body;
                message.ReplyToList.Add(VLVNEmail);
                message.IsBodyHtml = true;

                // Bưu điện(chưa được)
                var mail = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential(VLVNEmail, VLVNPassword),

                    EnableSsl = true
                };
                //mail.UseDefaultCredentials = false;
                // Gửi thư
                mail.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                string tt = ex.ToString();
                return false;
            }

            //SmtpClient smtp = new SmtpClient();
            //try
            //{
            //    //ĐỊA CHỈ SMTP Server
            //    smtp.Host = "smtp.gmail.com";
            //    //Cổng SMTP
            //    smtp.Port = 25;
            //    //SMTP yêu cầu mã hóa dữ liệu theo SSL
            //    smtp.EnableSsl = true;
            //    //UserName và Password của mail
            //    smtp.UseDefaultCredentials = false;
            //    smtp.Credentials = new NetworkCredential(VLVNEmail, VLVNPassword);
            //    MailAddress from = new MailAddress(VLVNEmail, VLVNName);
            //    MailMessage message = new MailMessage(from.ToString(), Email, Subject, Body);
            //    message.IsBodyHtml = true;
            //    smtp.Send(message);
            //    return true;
            //    //1. Bật Xác minh bước 2 trên gmail
            //    //vào https://myaccount.google.com/apppasswords => đặt tên ứng dụng và Get app password
            //    //Sử dụng app password thay cho pass
            //}
            //catch (Exception ex)
            //{
            //    string tt = ex.ToString();
            //    return false;
            //}
        }
    }
}