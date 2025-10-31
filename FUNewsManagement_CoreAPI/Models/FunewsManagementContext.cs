using FUNewsManagement_CoreAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagement_CoreAPI.Models;

public partial class FunewsManagementContext : DbContext
{
    public FunewsManagementContext()
    {
    }

    public FunewsManagementContext(DbContextOptions<FunewsManagementContext> options)
        : base(options)
    {
    }
    public virtual DbSet<AuditLog> AuditLogs { get; set; }
    public virtual DbSet<NewsImage> NewsImages { get; set; }
    public virtual DbSet<NewsView> NewsViews { get; set; }
    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<NewsArticle> NewsArticles { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<SystemAccount> SystemAccounts { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AuditLog__3214EC07F315A029");

            entity.Property(e => e.Action).HasMaxLength(50);
            entity.Property(e => e.EntityName).HasMaxLength(100);
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_AuditLogs_SystemAccount");
        });
        modelBuilder.Entity<NewsImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__NewsImag__7516F70C6264A358");

            entity.ToTable("NewsImage");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.NewsArticleId).HasMaxLength(20);

            entity.HasOne(d => d.NewsArticle).WithMany(p => p.NewsImages)
                .HasForeignKey(d => d.NewsArticleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NewsImage__NewsA__3F466844");
        });

        modelBuilder.Entity<NewsView>(entity =>
        {
            entity.HasKey(e => e.ViewId).HasName("PK__NewsView__1E371CF65398B8BD");

            entity.ToTable("NewsView");

            entity.Property(e => e.NewsArticleId).HasMaxLength(20);
            entity.Property(e => e.ViewedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.NewsArticle).WithMany(p => p.NewsViews)
                .HasForeignKey(d => d.NewsArticleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NewsView_Article");

            entity.HasOne(d => d.ViewedBy).WithMany(p => p.NewsViews)
                .HasForeignKey(d => d.ViewedById)
                .HasConstraintName("FK_NewsView_User");
        });
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryDesciption).HasMaxLength(250);
            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.ParentCategoryId).HasColumnName("ParentCategoryID");

            entity.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory)
                .HasForeignKey(d => d.ParentCategoryId)
                .HasConstraintName("FK_Category_Category");
        });

        modelBuilder.Entity<NewsArticle>(entity =>
        {
            entity.ToTable("NewsArticle");

            entity.Property(e => e.NewsArticleId)
                .HasMaxLength(20)
                .HasColumnName("NewsArticleID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Headline).HasMaxLength(150);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.NewsContent).HasMaxLength(4000);
            entity.Property(e => e.NewsSource).HasMaxLength(400);
            entity.Property(e => e.NewsTitle).HasMaxLength(400);
            entity.Property(e => e.UpdatedById).HasColumnName("UpdatedByID");

            entity.HasOne(d => d.Category).WithMany(p => p.NewsArticles)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_NewsArticle_Category");

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.NewsArticles)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_NewsArticle_SystemAccount");

            entity.HasMany(d => d.Tags).WithMany(p => p.NewsArticles)
                .UsingEntity<Dictionary<string, object>>(
                    "NewsTag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_NewsTag_Tag"),
                    l => l.HasOne<NewsArticle>().WithMany()
                        .HasForeignKey("NewsArticleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_NewsTag_NewsArticle"),
                    j =>
                    {
                        j.HasKey("NewsArticleId", "TagId");
                        j.ToTable("NewsTag");
                        j.IndexerProperty<string>("NewsArticleId")
                            .HasMaxLength(20)
                            .HasColumnName("NewsArticleID");
                        j.IndexerProperty<int>("TagId").HasColumnName("TagID");
                    });
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefreshT__3214EC2736704628");

            entity.ToTable("RefreshToken");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
            entity.Property(e => e.Token).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RefreshTo__UserI__3B75D760");
        });

        modelBuilder.Entity<SystemAccount>(entity =>
        {
            entity.HasKey(e => e.AccountId);

            entity.ToTable("SystemAccount");

            entity.Property(e => e.AccountId)
                .ValueGeneratedNever()
                .HasColumnName("AccountID");
            entity.Property(e => e.AccountEmail).HasMaxLength(70);
            entity.Property(e => e.AccountName).HasMaxLength(100);
            entity.Property(e => e.AccountPassword).HasMaxLength(70);
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagId).HasName("PK_HashTag");

            entity.ToTable("Tag");

            entity.Property(e => e.TagId)
                .ValueGeneratedNever()
                .HasColumnName("TagID");
            entity.Property(e => e.Note).HasMaxLength(400);
            entity.Property(e => e.TagName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
