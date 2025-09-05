namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProjectDetail")]
    public partial class ProjectDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProjectDetail()
        {
            ProjectUserDetails = new HashSet<ProjectUserDetail>();
        }

        public Guid Id { get; set; }

        public int UserId { get; set; }

        public DateTime NgayBatDau { get; set; }

        public DateTime NgayUpdate { get; set; }

        public int ProjectId { get; set; }

        public int TrangthaiId { get; set; }

        [StringLength(200)]
        public string Ghichu { get; set; }

        public bool Leader { get; set; }

        public virtual ProjectTeK ProjectTeK { get; set; }

        public virtual TrangThaiDuAn TrangThaiDuAn { get; set; }

        public virtual UserTek UserTek { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectUserDetail> ProjectUserDetails { get; set; }
    }
}
