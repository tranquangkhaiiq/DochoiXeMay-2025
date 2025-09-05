namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("QCtrangchu")]
    public partial class QCtrangchu
    {
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Name { get; set; }

        public DateTime Ngay { get; set; }

        public int Idvitri { get; set; }

        public bool Sudung { get; set; }

        public bool Img { get; set; }

        public int Idloai_socials { get; set; }

        [StringLength(50)]
        public string Urlsocials { get; set; }

        [StringLength(500)]
        public string Ghichu { get; set; }

        public virtual Loai_Socials Loai_Socials { get; set; }

        public virtual QCVitri QCVitri { get; set; }
    }
}
