namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ser_Chitiet_XuatSN_CN
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Serial { get; set; }

        public int IdSN_CN { get; set; }

        public DateTime NgayXuat { get; set; }

        [StringLength(100)]
        public string Ghichu { get; set; }

        public virtual Ser_XuatSN_CN Ser_XuatSN_CN { get; set; }
    }
}
