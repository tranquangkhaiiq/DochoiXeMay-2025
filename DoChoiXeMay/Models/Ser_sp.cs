namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ser_sp
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ser_sp()
        {
            Ser_kichhoat = new HashSet<Ser_kichhoat>();
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(200)]
        public string LoSanXuat { get; set; }

        [StringLength(500)]
        public string Name_sp { get; set; }

        [Required]
        [StringLength(100)]
        public string SerialSP { get; set; }

        public bool DaIn { get; set; }

        public bool Sudung { get; set; }

        public string QRcode { get; set; }

        [StringLength(50)]
        public string Stt { get; set; }

        public DateTime NgayTao { get; set; }

        public DateTime NgayUpdate { get; set; }

        public int IDMF { get; set; }

        public int IdColor { get; set; }

        public int IdSize { get; set; }

        public int Idver { get; set; }

        public int IdLoai { get; set; }

        public bool HangTangKhongBan { get; set; }

        [StringLength(200)]
        public string Ghichu { get; set; }

        public int BaoHanh { get; set; }

        public virtual Color Color { get; set; }

        public virtual Manufacturer Manufacturer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ser_kichhoat> Ser_kichhoat { get; set; }

        public virtual Ser_LoaiHang Ser_LoaiHang { get; set; }

        public virtual Size Size { get; set; }

        public virtual Version Version { get; set; }
    }
}
