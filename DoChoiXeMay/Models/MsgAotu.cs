namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MsgAotu")]
    public partial class MsgAotu
    {
        public Guid Id { get; set; }

        public int UserIdmsgAotu { get; set; }

        [Required]
        [StringLength(500)]
        public string MsgHeThong { get; set; }

        public DateTime NgayTao { get; set; }

        public bool AdminDaxem { get; set; }

        public bool Sub2Daxem { get; set; }

        public bool Sub4Daxem { get; set; }

        public bool Sub5Daxem { get; set; }

        public bool Sub6Daxem { get; set; }
    }
}
