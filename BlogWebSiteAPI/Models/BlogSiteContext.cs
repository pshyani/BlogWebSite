using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BlogWebSiteAPI.Models
{
    public partial class BlogSiteContext : DbContext
    {
        public BlogSiteContext()
        {
        }

        public BlogSiteContext(DbContextOptions<BlogSiteContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BlogComments> BlogComments { get; set; }
        public virtual DbSet<Blogs> Blogs { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<BlogComments>(entity =>
            {
                entity.Property(e => e.Comment)
                    .HasColumnName("comment")
                    .HasMaxLength(500);

                entity.Property(e => e.Datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.BlogComments)
                    .HasForeignKey(d => d.BlogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BlogComments_Blogs");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BlogComments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BlogComments_Users");
            });

            modelBuilder.Entity<Blogs>(entity =>
            {
                entity.HasKey(e => e.BlogId);

                entity.Property(e => e.Datecreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(100);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Blogs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Blogs_Users");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });
        }
    }
}
