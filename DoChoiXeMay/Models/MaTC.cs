namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MaTC")]
    public partial class MaTC
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MaTC()
        {
            ChiTietTCs = new HashSet<ChiTietTC>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string TenMa { get; set; }

        public bool SuDung { get; set; }

        public DateTime NgayAuto { get; set; }

        [StringLength(500)]
        public string GhiChu { get; set; }

        public bool XuatNhap { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietTC> ChiTietTCs { get; set; }
    }
}
