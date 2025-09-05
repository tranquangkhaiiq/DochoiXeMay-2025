namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ser_XuatSN_CN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ser_XuatSN_CN()
        {
            Ser_Chitiet_XuatSN_CN = new HashSet<Ser_Chitiet_XuatSN_CN>();
        }

        public int Id { get; set; }

        public int UserId { get; set; }

        public int SoLuong { get; set; }

        public int DaAdd { get; set; }

        public DateTime NgayXuat { get; set; }

        public int IdKyxuat { get; set; }

        public int IdChiNhanh { get; set; }

        [StringLength(200)]
        public string Ghichu { get; set; }

        public virtual KyXuatNhap KyXuatNhap { get; set; }

        public virtual Ser_ChiNhanh Ser_ChiNhanh { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ser_Chitiet_XuatSN_CN> Ser_Chitiet_XuatSN_CN { get; set; }
    }
}
