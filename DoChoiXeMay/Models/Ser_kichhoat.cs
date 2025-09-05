namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ser_kichhoat
    {
        public Guid Id { get; set; }

        public Guid IDSer_box { get; set; }

        public Guid IDSer_sp { get; set; }

        public DateTime NgayKichHoat { get; set; }

        public DateTime NgayUpdate { get; set; }

        [StringLength(100)]
        public string EmailKichHoat { get; set; }

        [StringLength(200)]
        public string TenKhachHang { get; set; }

        [StringLength(50)]
        public string SDT { get; set; }

        [StringLength(500)]
        public string DiaChiKhach { get; set; }

        public int IdChiNhanh { get; set; }

        public int TrangThaiId { get; set; }

        [StringLength(200)]
        public string Ghichu { get; set; }

        public virtual Ser_box Ser_box { get; set; }

        public virtual Ser_sp Ser_sp { get; set; }

        public virtual Ser_trangthai Ser_trangthai { get; set; }
    }
}
