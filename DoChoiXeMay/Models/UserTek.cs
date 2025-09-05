namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserTek")]
    public partial class UserTek
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserTek()
        {
            ChiTietTCs = new HashSet<ChiTietTC>();
            KyXuatNhaps = new HashSet<KyXuatNhap>();
            MailTeKs = new HashSet<MailTeK>();
            MailTeKDetails = new HashSet<MailTeKDetail>();
            NoteKythuats = new HashSet<NoteKythuat>();
            ProjectDetails = new HashSet<ProjectDetail>();
            ProjectTeKs = new HashSet<ProjectTeK>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [StringLength(128)]
        public string Password { get; set; }

        [Required]
        [StringLength(128)]
        public string PasswordSalt { get; set; }

        public int IdLoai { get; set; }

        [StringLength(100)]
        public string LoaiConnection { get; set; }

        [Required]
        [StringLength(200)]
        public string EmailConnection { get; set; }

        public bool Islocked { get; set; }

        public DateTime LastLokedChangedate { get; set; }

        public DateTime lastPasswordChangedate { get; set; }

        public DateTime Createdate { get; set; }

        public int CountFailedPassword { get; set; }

        [StringLength(500)]
        public string GhiChu { get; set; }

        [StringLength(500)]
        public string Avatar { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietTC> ChiTietTCs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KyXuatNhap> KyXuatNhaps { get; set; }

        public virtual LoaiUserTek LoaiUserTek { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MailTeK> MailTeKs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MailTeKDetail> MailTeKDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NoteKythuat> NoteKythuats { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectDetail> ProjectDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectTeK> ProjectTeKs { get; set; }
    }
}
