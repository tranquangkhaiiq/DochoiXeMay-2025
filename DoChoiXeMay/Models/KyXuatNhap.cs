namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KyXuatNhap")]
    public partial class KyXuatNhap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KyXuatNhap()
        {
            ChiTietTCs = new HashSet<ChiTietTC>();
            ChitietXuatNhaps = new HashSet<ChitietXuatNhap>();
            Ser_XuatSN_CN = new HashSet<Ser_XuatSN_CN>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string TenKy { get; set; }

        public bool XuatNhap { get; set; }

        public int CKphantram { get; set; }

        public double CKtienmat { get; set; }

        public double Shipper { get; set; }

        public int VAT { get; set; }

        public int IdMaTC { get; set; }

        public int UserId { get; set; }

        public bool UPush { get; set; }

        public bool AdminXNPUSH { get; set; }

        public bool UYeuCauThuHoi { get; set; }

        public double TongTienAuto { get; set; }

        public DateTime NgayXuatNhap { get; set; }

        public DateTime NgayAuto { get; set; }

        [StringLength(100)]
        public string HoaDon { get; set; }

        [StringLength(100)]
        public string Filesave2 { get; set; }

        [StringLength(100)]
        public string Filesave3 { get; set; }

        [StringLength(500)]
        public string LuuKho { get; set; }

        [StringLength(50)]
        public string STT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietTC> ChiTietTCs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChitietXuatNhap> ChitietXuatNhaps { get; set; }

        public virtual UserTek UserTek { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ser_XuatSN_CN> Ser_XuatSN_CN { get; set; }
    }
}
