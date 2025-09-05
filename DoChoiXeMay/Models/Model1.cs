using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DoChoiXeMay.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<aspnet_getVisitors> aspnet_getVisitors { get; set; }
        public virtual DbSet<ChiTietTC> ChiTietTCs { get; set; }
        public virtual DbSet<ChitietXuatNhap> ChitietXuatNhaps { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<GroupDuAn> GroupDuAns { get; set; }
        public virtual DbSet<HangHoa> HangHoas { get; set; }
        public virtual DbSet<HanhDong> HanhDongs { get; set; }
        public virtual DbSet<HinhThucTC> HinhThucTCs { get; set; }
        public virtual DbSet<KhuVuc> KhuVucs { get; set; }
        public virtual DbSet<KyXuatNhap> KyXuatNhaps { get; set; }
        public virtual DbSet<Loai_Socials> Loai_Socials { get; set; }
        public virtual DbSet<LoaiNoteTeK> LoaiNoteTeKs { get; set; }
        public virtual DbSet<LoaiUserTek> LoaiUserTeks { get; set; }
        public virtual DbSet<MailTeK> MailTeKs { get; set; }
        public virtual DbSet<MailTeKDetail> MailTeKDetails { get; set; }
        public virtual DbSet<Manufacturer> Manufacturers { get; set; }
        public virtual DbSet<MaTC> MaTCs { get; set; }
        public virtual DbSet<MsgAotu> MsgAotus { get; set; }
        public virtual DbSet<NhatKyUTek> NhatKyUTeks { get; set; }
        public virtual DbSet<NoteKythuat> NoteKythuats { get; set; }
        public virtual DbSet<ProjectDetail> ProjectDetails { get; set; }
        public virtual DbSet<ProjectTeK> ProjectTeKs { get; set; }
        public virtual DbSet<ProjectUserDetail> ProjectUserDetails { get; set; }
        public virtual DbSet<QCtrangchu> QCtrangchus { get; set; }
        public virtual DbSet<QCVitri> QCVitris { get; set; }
        public virtual DbSet<Ser_box> Ser_box { get; set; }
        public virtual DbSet<Ser_ChiNhanh> Ser_ChiNhanh { get; set; }
        public virtual DbSet<Ser_Chitiet_XuatSN_CN> Ser_Chitiet_XuatSN_CN { get; set; }
        public virtual DbSet<Ser_kichhoat> Ser_kichhoat { get; set; }
        public virtual DbSet<Ser_Levelchinhanh> Ser_Levelchinhanh { get; set; }
        public virtual DbSet<Ser_LoaiHang> Ser_LoaiHang { get; set; }
        public virtual DbSet<Ser_sp> Ser_sp { get; set; }
        public virtual DbSet<Ser_trangthai> Ser_trangthai { get; set; }
        public virtual DbSet<Ser_XuatSN_CN> Ser_XuatSN_CN { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TrangThaiDuAn> TrangThaiDuAns { get; set; }
        public virtual DbSet<UserTek> UserTeks { get; set; }
        public virtual DbSet<Version> Versions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Color>()
                .HasMany(e => e.ChitietXuatNhaps)
                .WithRequired(e => e.Color)
                .HasForeignKey(e => e.IDColor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Color>()
                .HasMany(e => e.HangHoas)
                .WithRequired(e => e.Color)
                .HasForeignKey(e => e.IDColor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Color>()
                .HasMany(e => e.Ser_sp)
                .WithRequired(e => e.Color)
                .HasForeignKey(e => e.IdColor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GroupDuAn>()
                .HasMany(e => e.ProjectTeKs)
                .WithRequired(e => e.GroupDuAn)
                .HasForeignKey(e => e.GroupId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HanhDong>()
                .HasMany(e => e.NoteKythuats)
                .WithRequired(e => e.HanhDong)
                .HasForeignKey(e => e.IdHanhDong)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HinhThucTC>()
                .HasMany(e => e.ChiTietTCs)
                .WithRequired(e => e.HinhThucTC)
                .HasForeignKey(e => e.IdHT)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KhuVuc>()
                .HasMany(e => e.Ser_ChiNhanh)
                .WithRequired(e => e.KhuVuc)
                .HasForeignKey(e => e.IdKhuVuc)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KyXuatNhap>()
                .Property(e => e.LuuKho)
                .IsFixedLength();

            modelBuilder.Entity<KyXuatNhap>()
                .HasMany(e => e.ChiTietTCs)
                .WithRequired(e => e.KyXuatNhap)
                .HasForeignKey(e => e.IdKyxuatnhap)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KyXuatNhap>()
                .HasMany(e => e.ChitietXuatNhaps)
                .WithRequired(e => e.KyXuatNhap)
                .HasForeignKey(e => e.IdKy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KyXuatNhap>()
                .HasMany(e => e.Ser_XuatSN_CN)
                .WithRequired(e => e.KyXuatNhap)
                .HasForeignKey(e => e.IdKyxuat)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Loai_Socials>()
                .HasMany(e => e.QCtrangchus)
                .WithRequired(e => e.Loai_Socials)
                .HasForeignKey(e => e.Idloai_socials)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LoaiNoteTeK>()
                .HasMany(e => e.NoteKythuats)
                .WithRequired(e => e.LoaiNoteTeK)
                .HasForeignKey(e => e.LoaiNoteId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LoaiUserTek>()
                .HasMany(e => e.UserTeks)
                .WithRequired(e => e.LoaiUserTek)
                .HasForeignKey(e => e.IdLoai)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MailTeK>()
                .HasMany(e => e.MailTeKDetails)
                .WithRequired(e => e.MailTeK)
                .HasForeignKey(e => e.FromIdmail)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Manufacturer>()
                .HasMany(e => e.ChitietXuatNhaps)
                .WithRequired(e => e.Manufacturer)
                .HasForeignKey(e => e.IDMF)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Manufacturer>()
                .HasMany(e => e.HangHoas)
                .WithRequired(e => e.Manufacturer)
                .HasForeignKey(e => e.IDMF)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Manufacturer>()
                .HasMany(e => e.Ser_sp)
                .WithRequired(e => e.Manufacturer)
                .HasForeignKey(e => e.IDMF)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MaTC>()
                .HasMany(e => e.ChiTietTCs)
                .WithRequired(e => e.MaTC)
                .HasForeignKey(e => e.IdMa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProjectDetail>()
                .HasMany(e => e.ProjectUserDetails)
                .WithRequired(e => e.ProjectDetail)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProjectTeK>()
                .HasMany(e => e.ProjectDetails)
                .WithRequired(e => e.ProjectTeK)
                .HasForeignKey(e => e.ProjectId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<QCVitri>()
                .HasMany(e => e.QCtrangchus)
                .WithRequired(e => e.QCVitri)
                .HasForeignKey(e => e.Idvitri)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ser_box>()
                .HasMany(e => e.Ser_kichhoat)
                .WithRequired(e => e.Ser_box)
                .HasForeignKey(e => e.IDSer_box)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ser_ChiNhanh>()
                .HasMany(e => e.Ser_XuatSN_CN)
                .WithRequired(e => e.Ser_ChiNhanh)
                .HasForeignKey(e => e.IdChiNhanh)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ser_Levelchinhanh>()
                .HasMany(e => e.Ser_ChiNhanh)
                .WithRequired(e => e.Ser_Levelchinhanh)
                .HasForeignKey(e => e.IdLevel)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ser_LoaiHang>()
                .HasMany(e => e.HangHoas)
                .WithRequired(e => e.Ser_LoaiHang)
                .HasForeignKey(e => e.IdLoai)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ser_LoaiHang>()
                .HasMany(e => e.Ser_box)
                .WithRequired(e => e.Ser_LoaiHang)
                .HasForeignKey(e => e.IdLoai)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ser_LoaiHang>()
                .HasMany(e => e.Ser_sp)
                .WithRequired(e => e.Ser_LoaiHang)
                .HasForeignKey(e => e.IdLoai)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ser_sp>()
                .HasMany(e => e.Ser_kichhoat)
                .WithRequired(e => e.Ser_sp)
                .HasForeignKey(e => e.IDSer_sp)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ser_trangthai>()
                .HasMany(e => e.Ser_kichhoat)
                .WithRequired(e => e.Ser_trangthai)
                .HasForeignKey(e => e.TrangThaiId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ser_XuatSN_CN>()
                .HasMany(e => e.Ser_Chitiet_XuatSN_CN)
                .WithRequired(e => e.Ser_XuatSN_CN)
                .HasForeignKey(e => e.IdSN_CN)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Size>()
                .HasMany(e => e.ChitietXuatNhaps)
                .WithRequired(e => e.Size)
                .HasForeignKey(e => e.IDSize)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Size>()
                .HasMany(e => e.HangHoas)
                .WithRequired(e => e.Size)
                .HasForeignKey(e => e.IDSize)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Size>()
                .HasMany(e => e.Ser_sp)
                .WithRequired(e => e.Size)
                .HasForeignKey(e => e.IdSize)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TrangThaiDuAn>()
                .HasMany(e => e.ProjectDetails)
                .WithRequired(e => e.TrangThaiDuAn)
                .HasForeignKey(e => e.TrangthaiId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TrangThaiDuAn>()
                .HasMany(e => e.ProjectTeKs)
                .WithRequired(e => e.TrangThaiDuAn)
                .HasForeignKey(e => e.TrangthaiId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TrangThaiDuAn>()
                .HasMany(e => e.ProjectUserDetails)
                .WithRequired(e => e.TrangThaiDuAn)
                .HasForeignKey(e => e.TrangthaiId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserTek>()
                .HasMany(e => e.ChiTietTCs)
                .WithRequired(e => e.UserTek)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserTek>()
                .HasMany(e => e.KyXuatNhaps)
                .WithRequired(e => e.UserTek)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserTek>()
                .HasMany(e => e.MailTeKs)
                .WithRequired(e => e.UserTek)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserTek>()
                .HasMany(e => e.MailTeKDetails)
                .WithRequired(e => e.UserTek)
                .HasForeignKey(e => e.toUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserTek>()
                .HasMany(e => e.NoteKythuats)
                .WithRequired(e => e.UserTek)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserTek>()
                .HasMany(e => e.ProjectDetails)
                .WithRequired(e => e.UserTek)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserTek>()
                .HasMany(e => e.ProjectTeKs)
                .WithRequired(e => e.UserTek)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Version>()
                .HasMany(e => e.Ser_sp)
                .WithRequired(e => e.Version)
                .HasForeignKey(e => e.Idver)
                .WillCascadeOnDelete(false);
        }
    }
}
