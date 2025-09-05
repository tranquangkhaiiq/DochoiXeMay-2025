namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MailTeKDetail")]
    public partial class MailTeKDetail
    {
        public Guid Id { get; set; }

        public Guid FromIdmail { get; set; }

        public int toUserId { get; set; }

        public bool ChuaXem { get; set; }

        public bool Xoa { get; set; }

        public DateTime NgayAotu { get; set; }

        [StringLength(100)]
        public string GhiChu { get; set; }

        public virtual MailTeK MailTeK { get; set; }

        public virtual UserTek UserTek { get; set; }
    }
}
