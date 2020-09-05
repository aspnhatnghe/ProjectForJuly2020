using Microsoft.EntityFrameworkCore;

namespace MyProjectForJuly2020.Data
{
    public class MyDbContext : DbContext
    {
        #region DbSet
        public DbSet<Loai> Loais { get; set; }
        public DbSet<HangHoa> HangHoas { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<HangHoaTag> HangHoaTags { get; set; }
        public DbSet<HinhPhu> HinhPhus { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ReviewHangHoa> ReviewHangHoas { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HinhPhu>(e => {
                e.HasOne(h => h.HangHoa)
                .WithMany(hh => hh.HinhPhus)
                .HasForeignKey(h => h.MaHangHoa);
            });
            modelBuilder.Entity<HangHoa>(e =>
            {
                e.ToTable("HangHoa");
                e.HasKey(hh => hh.MaHangHoa);
                e.Property(hh => hh.MaHangHoa)
                    //.HasDefaultValue("0");
                    .HasDefaultValueSql("newid()");
                e.Property(hh => hh.TenHh)
                .IsRequired().HasMaxLength(100);
                e.Property(hh => hh.ChiTiet).HasMaxLength(200);
                e.HasIndex(hh => hh.TenHh).IsUnique();

                e.HasOne(hh => hh.Loai)
                .WithMany(lo => lo.HangHoas)
                .HasForeignKey(hh => hh.MaLoai)
                .OnDelete(DeleteBehavior.SetNull);
            });
            modelBuilder.Entity<Tag>(e => {
                e.ToTable("Tag");
                e.HasKey(hh => hh.TagKey);
                e.Property(hh => hh.TagKey).HasMaxLength(50);
            });
            modelBuilder.Entity<HangHoaTag>(e => {
                e.ToTable("HangHoaTag");
                e.HasKey(hh => new { hh.TagKey, hh.MaHangHoa});
                e.HasOne(hh => hh.HangHoa)
                 .WithMany(hht => hht.HangHoaTags)
                 .HasForeignKey(t => t.MaHangHoa);

                e.HasOne(hh => hh.Tag)
                 .WithMany(hht => hht.HangHoaTags)
                 .HasForeignKey(t => t.TagKey);
            });

            modelBuilder.Entity<Loai>(entity =>
            {
                entity.ToTable("Loai");
                entity.Property(e => e.TenLoai)
                    .IsRequired()
                    .IsUnicode().HasMaxLength(100);
                entity.HasKey(e => e.MaLoai);
            });
        }

        public MyDbContext(DbContextOptions opt) : base(opt)
        {
        }
    }
}
