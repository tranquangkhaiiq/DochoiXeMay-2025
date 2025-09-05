namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HangHoa")]
    public partial class HangHoa
    {
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Ten { get; set; }

        public int IDKy { get; set; }

        public int SoLuong { get; set; }

        public double GiaNhap { get; set; }

        public DateTime NgayAuto { get; set; }

        public int IdLoai { get; set; }

        [StringLength(100)]
        public string Hinh1 { get; set; }

        [StringLength(100)]
        public string Hinh2 { get; set; }

        [StringLength(100)]
        public string Hinh3 { get; set; }

        public int IDMF { get; set; }

        public int IDColor { get; set; }

        public int IDSize { get; set; }

        [StringLength(200)]
        public string GhiChu { get; set; }

        public virtual Color Color { get; set; }

        public virtual Manufacturer Manufacturer { get; set; }

        public virtual Ser_LoaiHang Ser_LoaiHang { get; set; }

        public virtual Size Size { get; set; }
    }
}
