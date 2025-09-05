namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class aspnet_getVisitors
    {
        public int Id { get; set; }

        public int TongLuotTruyCap { get; set; }

        public int Online { get; set; }

        [StringLength(100)]
        public string Ghichu { get; set; }

        public DateTime Ngay { get; set; }
    }
}
