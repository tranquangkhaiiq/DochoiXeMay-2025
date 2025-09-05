namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProjectTeK")]
    public partial class ProjectTeK
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProjectTeK()
        {
            ProjectDetails = new HashSet<ProjectDetail>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string NameProject { get; set; }

        [StringLength(500)]
        public string Giaithich { get; set; }

        public DateTime DateBegin { get; set; }

        public DateTime DateEnd { get; set; }

        public bool Uutien { get; set; }

        public int TrangthaiId { get; set; }

        public int GroupId { get; set; }

        public int PhantramHoanThanh { get; set; }

        public DateTime NgayCapNhat { get; set; }

        [StringLength(100)]
        public string GhiChu { get; set; }

        public int UserId { get; set; }

        public virtual GroupDuAn GroupDuAn { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectDetail> ProjectDetails { get; set; }

        public virtual TrangThaiDuAn TrangThaiDuAn { get; set; }

        public virtual UserTek UserTek { get; set; }
    }
}
