namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("QCbaiviet")]
    public partial class QCbaiviet
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public QCbaiviet()
        {
            QCtrangchus = new HashSet<QCtrangchu>();
        }

        public int Id { get; set; }

        [StringLength(200)]
        public string Imgmau1 { get; set; }

        [StringLength(200)]
        public string Imgmau2 { get; set; }

        [StringLength(200)]
        public string Imgmau3 { get; set; }

        [StringLength(100)]
        public string TieuDe { get; set; }

        [StringLength(500)]
        public string Tomtat { get; set; }

        [StringLength(100)]
        public string BaiVietFile { get; set; }

        public string BaiViet { get; set; }

        public bool Sudung { get; set; }

        public DateTime Ngay { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QCtrangchu> QCtrangchus { get; set; }
    }
}
