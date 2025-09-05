namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NhatKyUTek")]
    public partial class NhatKyUTek
    {
        public Guid Id { get; set; }

        public int UserID { get; set; }

        [Required]
        [StringLength(50)]
        public string LoaiUser { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [StringLength(500)]
        public string CongViec { get; set; }

        [StringLength(500)]
        public string GhiChu { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
