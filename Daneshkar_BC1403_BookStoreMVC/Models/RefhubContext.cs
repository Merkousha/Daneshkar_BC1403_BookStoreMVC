using Microsoft.EntityFrameworkCore;

namespace Daneshkar_BC1403_BookStoreMVC.Models;

public partial class RefhubContext : DbContext
{
    public RefhubContext()
    {
    }

    public RefhubContext(DbContextOptions<RefhubContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookAuthor> BookAuthors { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Data Source=wwwroot/db/refhub.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.ToTable("Author");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("integer(64)")
                .HasColumnName("id");
            entity.Property(e => e.Cover)
                .HasColumnType("text(100)")
                .HasColumnName("cover");
            entity.Property(e => e.Name)
                .HasColumnType("text(255)")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.ToTable("Book");

            entity.HasIndex(e => e.SeriId, "RefHubWeb_refrence_seri_id_02ed7806");

            entity.HasIndex(e => e.CategoryId, "Refrence_category_id_25807a06");

            entity.HasIndex(e => e.LanguageId, "Refrence_language_id_7ec81184");

            entity.HasIndex(e => e.SkillLevelId, "Refrence_skill_level_id_7671a865");

            entity.HasIndex(e => e.Slug, "Refrence_slug_50dad31a_like");

            entity.HasIndex(e => e.ViewCount, "Refrence_view_count_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("integer(64)")
                .HasColumnName("id");
            entity.Property(e => e.Author)
                .HasColumnType("text(255)")
                .HasColumnName("author");
            entity.Property(e => e.AvgRating)
                .HasColumnType("integer(53)")
                .HasColumnName("avg_rating");
            entity.Property(e => e.CategoryId)
                .HasColumnType("integer(64)")
                .HasColumnName("category_id");
            entity.Property(e => e.CopyrightLimited).HasColumnName("copyright_limited");
            entity.Property(e => e.Cover)
                .HasColumnType("text(100)")
                .HasColumnName("cover");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("text(6)")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DescriptionEn).HasColumnName("description_en");
            entity.Property(e => e.DescriptionFa).HasColumnName("description_fa");
            entity.Property(e => e.Document)
                .HasColumnType("text(100)")
                .HasColumnName("document");
            entity.Property(e => e.DownloadCount)
                .HasColumnType("integer(32)")
                .HasColumnName("download_count");
            entity.Property(e => e.Featured).HasColumnName("featured");
            entity.Property(e => e.Isbn)
                .HasColumnType("text(255)")
                .HasColumnName("isbn");
            entity.Property(e => e.LanguageId)
                .HasColumnType("integer(64)")
                .HasColumnName("language_id");
            entity.Property(e => e.Mirror1)
                .HasColumnType("text(500)")
                .HasColumnName("mirror1");
            entity.Property(e => e.Mirror2)
                .HasColumnType("text(500)")
                .HasColumnName("mirror2");
            entity.Property(e => e.Name)
                .HasColumnType("text(255)")
                .HasColumnName("name");
            entity.Property(e => e.Pages)
                .HasColumnType("text(255)")
                .HasColumnName("pages");
            entity.Property(e => e.Publisher)
                .HasColumnType("text(255)")
                .HasColumnName("publisher");
            entity.Property(e => e.SeriId)
                .HasColumnType("integer(64)")
                .HasColumnName("seri_id");
            entity.Property(e => e.Size)
                .HasColumnType("text(255)")
                .HasColumnName("size");
            entity.Property(e => e.SkillLevelId)
                .HasColumnType("integer(64)")
                .HasColumnName("skill_level_id");
            entity.Property(e => e.Slug)
                .HasColumnType("text(255)")
                .HasColumnName("slug");
            entity.Property(e => e.Thumb)
                .HasColumnType("text(200)")
                .HasColumnName("thumb");
            entity.Property(e => e.ViewCount)
                .HasColumnType("integer(32)")
                .HasColumnName("view_count");
            entity.Property(e => e.Year)
                .HasColumnType("text(255)")
                .HasColumnName("year");

            entity.HasOne(d => d.Category).WithMany(p => p.Books).HasForeignKey(d => d.CategoryId);
        });

        modelBuilder.Entity<BookAuthor>(entity =>
        {
            entity.ToTable("Book_Authors");

            entity.HasIndex(e => e.AuthorId, "Refrence_authors_author_id_723d106a");

            entity.HasIndex(e => e.RefrenceId, "Refrence_authors_refrence_id_380b3556");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("integer(64)")
                .HasColumnName("id");
            entity.Property(e => e.AuthorId)
                .HasColumnType("integer(64)")
                .HasColumnName("author_id");
            entity.Property(e => e.RefrenceId)
                .HasColumnType("integer(64)")
                .HasColumnName("refrence_id");

            entity.HasOne(d => d.Author).WithMany(p => p.BookAuthors)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Refrence).WithMany(p => p.BookAuthors)
                .HasForeignKey(d => d.RefrenceId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("category");

            entity.HasIndex(e => e.ParentId, "RefHubWeb_category_parent_id_72499e85");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("integer(64)")
                .HasColumnName("id");
            entity.Property(e => e.Cover)
                .HasColumnType("text(100)")
                .HasColumnName("cover");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DescriptionFa).HasColumnName("description_fa");
            entity.Property(e => e.Name)
                .HasColumnType("text(100)")
                .HasColumnName("name");
            entity.Property(e => e.NameFa)
                .HasColumnType("text(100)")
                .HasColumnName("name_fa");
            entity.Property(e => e.ParentId)
                .HasColumnType("integer(64)")
                .HasColumnName("parent_id");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent).HasForeignKey(d => d.ParentId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
