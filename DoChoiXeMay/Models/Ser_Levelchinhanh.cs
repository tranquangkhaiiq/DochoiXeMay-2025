namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ser_Levelchinhanh
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ser_Levelchinhanh()
        {
            Ser_ChiNhanh = new HashSet<Ser_ChiNhanh>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Level_Name { get; set; }

        [Required]
        [StringLength(10)]
        public string Viettat { get; set; }

        public int ChietKhau_bandau { get; set; }

        public int chietkhau_khbh { get; set; }

        public int chietkhau_KPIQUI { get; set; }

        public int ChietKhau_KPIYEAR { get; set; }

        public int ChietKhau_khac { get; set; }

        [Required]
        [StringLength(200)]
        public string Thuongcuoinam { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ser_ChiNhanh> Ser_ChiNhanh { get; set; }
    }
}
