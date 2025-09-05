namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NoteKythuat")]
    public partial class NoteKythuat
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string NoteName { get; set; }

        public string NoiDung { get; set; }

        [StringLength(50)]
        public string Stt { get; set; }

        public int UserId { get; set; }

        public bool UPush { get; set; }

        public int PushtoNoteId { get; set; }

        public bool AdminXacNhan { get; set; }

        public int IdHanhDong { get; set; }

        public int LoaiNoteId { get; set; }

        public virtual HanhDong HanhDong { get; set; }

        public virtual LoaiNoteTeK LoaiNoteTeK { get; set; }

        public virtual UserTek UserTek { get; set; }
    }
}
