namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HinhThucTC")]
    public partial class HinhThucTC
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HinhThucTC()
        {
            ChiTietTCs = new HashSet<ChiTietTC>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string TenHT { get; set; }

        public bool SuDung { get; set; }

        public DateTime Ngay { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietTC> ChiTietTCs { get; set; }
    }
}
