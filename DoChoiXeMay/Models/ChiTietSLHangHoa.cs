namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietSLHangHoa")]
    public partial class ChiTietSLHangHoa
    {
        public Guid Id { get; set; }

        public int IdHangHoa { get; set; }

        public int IdKho { get; set; }

        public int SoLuong { get; set; }

        public DateTime NgayAuto { get; set; }

        [StringLength(100)]
        public string GhiChu { get; set; }
    }
}
