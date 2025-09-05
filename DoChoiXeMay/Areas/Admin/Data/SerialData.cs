using DoChoiXeMay.Models;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Web.Helpers;
using System.Net;
using System.Web.Services.Description;

namespace DoChoiXeMay.Areas.Admin.Data
{
    public class SerialData
    {
        Model1 _context;
        public SerialData()
        {
            _context = new Model1();
        }
        public bool InsertSer_sp(Ser_sp sp)
        {
            try
            {
                Ser_sp p = new Ser_sp();
                p = sp;
                p.Id = Guid.NewGuid();
                p.NgayTao = DateTime.Now;
                p.NgayUpdate = sp.NgayUpdate;
                p.DaIn = false;
                _context.Ser_sp.Add(p);
                int kt = _context.SaveChanges();
                if (kt > 0)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return false;
            }
        }
        public bool InsertSer_Box(int IdLoai,string Ngaysx,string Lo, string Seri, bool sudung, string stt, string ghichu, string qr)
        {
            try
            {
                Ser_box b = new Ser_box();
                b.Id = Guid.NewGuid();
                b.LoSanXuat = Lo;
                b.Serial = Seri;
                b.Sudung = sudung;
                b.Stt = stt;
                b.NgayTao = DateTime.Now;
                b.NgayUpdate = DateTime.Parse(Ngaysx);
                b.IdLoai = IdLoai;
                b.DaIn = false;
                b.Ghichu = ghichu;
                b.QRcode = qr;
                _context.Ser_box.Add(b);
                int kt = _context.SaveChanges();
                if (kt > 0)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return false;
            }
        }
        public bool UpdateSer_SP(string Id, bool sudung, bool dain, DateTime ngayupdate, bool tang)
        {
            try
            {
                var sp = _context.Ser_sp.Find(new Guid(Id));
                if (sp != null)
                {
                    sp.Sudung = sudung;
                    sp.DaIn = dain;
                    sp.NgayUpdate = ngayupdate;
                    sp.HangTangKhongBan = tang;
                    _context.Entry(sp).State = EntityState.Modified;
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return false;
            }
        }
        public bool UpdateSer_box(string Id, bool sudung, bool dain, DateTime ngayupdate, string ghichu)
        {
            try
            {
                var box = _context.Ser_box.Find(new Guid(Id));
                if (box != null)
                {
                    box.Sudung = sudung;
                    box.DaIn = dain;
                    box.NgayUpdate = ngayupdate;
                    box.Ghichu = ghichu;
                    _context.Entry(box).State = EntityState.Modified;
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return false;
            }
        }
        //Khoong xai
        public bool InsertSer_kichhoat(string IdSer_box, string IdSer_sp, string email)
        {
            try
            {
                Ser_kichhoat kichhoat = new Ser_kichhoat();
                kichhoat.Id = Guid.NewGuid();
                kichhoat.IDSer_box = new Guid(IdSer_box);
                kichhoat.IDSer_sp = new Guid(IdSer_sp);
                kichhoat.NgayKichHoat = DateTime.Now;
                kichhoat.NgayUpdate = DateTime.Now;
                kichhoat.EmailKichHoat = email;
                kichhoat.TrangThaiId = 1;
                kichhoat.Ghichu = "";
                _context.Ser_kichhoat.Add(kichhoat);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return false;
            }
        }
        public string getQRcode(string qrcode)
        {
            string QR = "";
            try
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qRCodeData = qrGenerator.CreateQrCode(qrcode, QRCodeGenerator.ECCLevel.Q);
                QRCode qRCode = new QRCode(qRCodeData);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (Bitmap bitMap = qRCode.GetGraphic(8))
                    {
                        bitMap.Save(ms, ImageFormat.Png);
                        QR = Convert.ToBase64String(ms.ToArray());
                        //QR = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                    }
                }
                return QR;
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return "";
            }
        }
        public string getImgtextBOX(string value,bool box)
        {
            string kq = "";
            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                Bitmap bitmap = new Bitmap(600, 230, PixelFormat.Format64bppArgb);
                Graphics graphics = Graphics.FromImage(bitmap);
                graphics.Clear(System.Drawing.Color.White);
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                if (box)
                {
                    graphics.DrawString(value, new Font("Roboto", 32, FontStyle.Regular), new SolidBrush(System.Drawing.Color.FromArgb(0, 0, 0)), new PointF(20F, 33F));
                }
                else
                {
                    graphics.DrawString(value, new Font("Roboto", 35, FontStyle.Regular), new SolidBrush(System.Drawing.Color.FromArgb(0, 0, 0)), new PointF(100F, 30F));
                }
                
                graphics.Flush();
                graphics.Dispose();
                bitmap.Save(stream, ImageFormat.Jpeg);
                bitmap.Dispose();
                bytes = stream.ToArray();
            }
            kq =Convert.ToBase64String(bytes, 0, bytes.Length);
            //kq = "data:image/png;base64," + Convert.ToBase64String(bytes, 0, bytes.Length);
            return kq;
        }
        public string getMergeImg(string base641, string base642)
        {
            string kq = "";
            base641 = Cleanbase64(base641);
            base642 = Cleanbase64(base642);
            Image img1 = Base64toImg(base641);
            Image img2 = Base64toImg(base642);
            //Chiều cao hình mới = Tổng 2 chiều cao
            //chiều rộng hình mới=max(2 chiều rộng)
            int mergewith = Math.Max(img1.Width, img2.Width);
            int mergeheight = img1.Height + img2.Height - img2.Height/2;

            Bitmap mergeImg = new Bitmap(mergewith, mergeheight);
            using (Graphics g = Graphics.FromImage(mergeImg))
            {
                g.Clear(System.Drawing.Color.White);//nen trang
                g.DrawImage(img1, 0, img1.Height);
                g.DrawImage(img2, img1.Width/4, 0);
            }
            //xuat anh ra stream
            using (MemoryStream ms = new MemoryStream())
            {
                mergeImg.Save(ms, ImageFormat.Png);
                byte[] bytes = ms.ToArray();
                kq = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
            }
            return kq;
        }
        //không xài
        //public string getMergeImgNgang(string base641, string base642)
        //{
        //    string kq = "";
        //    base641 = Cleanbase64(base641);
        //    base642 = Cleanbase64(base642);
        //    Image img1 = Base64toImg(base641);
        //    Image img2 = Base64toImg(base642);
        //    //Chiều cao hình mới = max(2 chiều cao) Tổng 2 chiều cao
        //    //chiều rộng hình mới=Tổng 2 chiều rong
        //    int mergewith = img1.Width + img2.Width;
        //    int mergeheight = Math.Max(img1.Height, img2.Height);

        //    Bitmap mergeImg = new Bitmap(mergewith, mergeheight);
        //    using (Graphics g = Graphics.FromImage(mergeImg))
        //    {
        //        g.Clear(System.Drawing.Color.White);//nen trang
        //        g.DrawImage(img1, 0, 0);
        //        g.DrawImage(img2, img1.Width, img1.Height/3);
        //    }
        //    //xuat anh ra stream
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        mergeImg.Save(ms, ImageFormat.Png);
        //        byte[] bytes = ms.ToArray();
        //        kq = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
        //    }
        //    return kq;
        //}
        //ghep 2 qr code
        public string getMerge2Img(string str1, string str2)
        {
            string kq = "";
            Image img2; Image img1;
            str1 = Cleanbase64(str1);
            img1 = Base64toImg(str1);

            if (str2 == "NOSERIALNUMBER")
            {
                img2 = gettextImgbystring("NOSERIALNUMBER", "------------");
            }
            else
            {
                str2 = Cleanbase64(str2);
                img2 = Base64toImg(str2);
            }
            
            //Chiều cao hình mới = max(2 chiều cao)
            //chiều rộng hình mới= tổng 2 chiều rộng
            int mergewith = img1.Width + img2.Width;
            int mergeheight = Math.Max(img1.Height, img2.Height);

            Bitmap mergeImg = new Bitmap(mergewith, mergeheight);
            using (Graphics g = Graphics.FromImage(mergeImg))
            {
                g.Clear(System.Drawing.Color.White);//nen trang
                g.DrawImage(img1, 0, 0);
                g.DrawImage(img2, img1.Width, 0);
            }
            //xuat anh ra stream
            using (MemoryStream ms = new MemoryStream())
            {
                mergeImg.Save(ms, ImageFormat.Png);
                byte[] bytes = ms.ToArray();
                kq = Convert.ToBase64String(ms.ToArray());
            }
            return kq;
        }
        private string Cleanbase64(string base64)
        {
            if (base64.Contains(",")){
                return base64.Split(',')[1];
            }
            return base64;
        }
        private Image Base64toImg(string base64string)
        {
            byte[] imageBytes = Convert.FromBase64String(base64string);
            
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                return Image.FromStream(ms);
            }
        }
        public Image getScaleImg2(string base641)
        {
            base641 = Cleanbase64(base641);
            Image img2 = Base64toImg(base641);
            return img2;
        }
        public Image gettextImgbystring(string QRbase641, string Strbase642)
        {
            string qrS = getQRcode(QRbase641);
            string ImgS = getImgtextBOX(Strbase642,false);
            //string merS = getMergeImgNgang(qrS,ImgS);
            string merS = getMergeImg(ImgS,qrS);
            Image img = getScaleImg2(merS);
            return img;
        }
        //Khoong dung nua
        //public Image getScaleImg(string base641)
        //{
        //    base641 = Cleanbase64(base641);
        //    byte[] imageBytes = Convert.FromBase64String(base641);
        //    WebImage webi = new WebImage(imageBytes);
            
        //        webi.Resize(1000, 300);
        //    byte[] imageBytes2 = webi.GetBytes();
        //    //
        //    string base64String = Convert.ToBase64String(imageBytes2);
        //    //
        //    Image img2 = Base64toImg(base64String);

        //    return img2;
        //}
    }
}