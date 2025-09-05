namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ser_box
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ser_box()
        {
            Ser_kichhoat = new HashSet<Ser_kichhoat>();
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(200)]
        public string LoSanXuat { get; set; }

        [Required]
        [StringLength(100)]
        public string Serial { get; set; }

        public bool DaIn { get; set; }

        public bool Sudung { get; set; }

        public int IdLoai { get; set; }

        public string QRcode { get; set; }

        [StringLength(50)]
        public string Stt { get; set; }

        public DateTime NgayTao { get; set; }

        public DateTime NgayUpdate { get; set; }

        [StringLength(200)]
        public string Ghichu { get; set; }

        public virtual Ser_LoaiHang Ser_LoaiHang { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ser_kichhoat> Ser_kichhoat { get; set; }
    }
}
