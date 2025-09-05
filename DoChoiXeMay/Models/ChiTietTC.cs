namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietTC")]
    public partial class ChiTietTC
    {
        public Guid Id { get; set; }

        public int IdMa { get; set; }

        public int IdHT { get; set; }

        public int IdKyxuatnhap { get; set; }

        [Required]
        [StringLength(1000)]
        public string Noidung { get; set; }

        public double SoTien { get; set; }

        public bool ThuChi { get; set; }

        public bool Indebted { get; set; }

        [StringLength(50)]
        public string STT { get; set; }

        [StringLength(100)]
        public string Filesave1 { get; set; }

        [StringLength(100)]
        public string Filesave2 { get; set; }

        [StringLength(100)]
        public string HoaDon { get; set; }

        public DateTime NgayTC { get; set; }

        public DateTime NgayAuto { get; set; }

        public int UserId { get; set; }

        public bool YeuCauDay { get; set; }

        public bool AdminXacNhan { get; set; }

        public virtual HinhThucTC HinhThucTC { get; set; }

        public virtual KyXuatNhap KyXuatNhap { get; set; }

        public virtual MaTC MaTC { get; set; }

        public virtual UserTek UserTek { get; set; }
    }
}
