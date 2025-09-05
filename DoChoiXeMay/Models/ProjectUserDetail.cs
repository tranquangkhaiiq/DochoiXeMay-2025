namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProjectUserDetail")]
    public partial class ProjectUserDetail
    {
        public Guid Id { get; set; }

        public Guid ProjectDetailId { get; set; }

        [StringLength(500)]
        public string CongViec { get; set; }

        public int TrangthaiId { get; set; }

        public DateTime NgayUpdate { get; set; }

        public virtual ProjectDetail ProjectDetail { get; set; }

        public virtual TrangThaiDuAn TrangThaiDuAn { get; set; }
    }
}
