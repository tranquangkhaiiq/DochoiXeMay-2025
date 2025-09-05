namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MailTeK")]
    public partial class MailTeK
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MailTeK()
        {
            MailTeKDetails = new HashSet<MailTeKDetail>();
        }

        public Guid Id { get; set; }

        public int UserId { get; set; }

        public int LoaiId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Containt { get; set; }

        [StringLength(200)]
        public string Foot { get; set; }

        public DateTime NgayAotu { get; set; }

        public Guid IdReplay { get; set; }

        [StringLength(100)]
        public string GhiChu { get; set; }

        public virtual UserTek UserTek { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MailTeKDetail> MailTeKDetails { get; set; }
    }
}
