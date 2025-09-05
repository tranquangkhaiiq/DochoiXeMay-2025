namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KhuVuc")]
    public partial class KhuVuc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KhuVuc()
        {
            Ser_ChiNhanh = new HashSet<Ser_ChiNhanh>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string TenKhuvuc { get; set; }

        [Required]
        [StringLength(10)]
        public string Viettat { get; set; }

        public bool Sudung { get; set; }

        [StringLength(50)]
        public string MaVungDT { get; set; }

        [StringLength(50)]
        public string BienSoXe { get; set; }

        [StringLength(50)]
        public string MaBuuChinh { get; set; }

        [StringLength(200)]
        public string SatNhap { get; set; }

        [StringLength(200)]
        public string Ghichu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ser_ChiNhanh> Ser_ChiNhanh { get; set; }
    }
}
