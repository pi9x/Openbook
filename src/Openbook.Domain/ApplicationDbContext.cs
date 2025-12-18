using Microsoft.EntityFrameworkCore;
using Openbook.Domain.Aggregates.Authors;
using Openbook.Domain.Aggregates.Books;
using Openbook.Domain.Aggregates.Reviews;

namespace Openbook.Domain;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Author> Authors { get; set; } = null!;
    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<Review> Reviews { get; set; } = null!;
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<AuthorId>().HaveConversion<StronglyTypedIdConverter<AuthorId>>();
        configurationBuilder.Properties<BookId>().HaveConversion<StronglyTypedIdConverter<BookId>>();
        configurationBuilder.Properties<ReviewId>().HaveConversion<StronglyTypedIdConverter<ReviewId>>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Author>(a =>
        {
            a.HasKey(x => x.AuthorId);
            a.Property(x => x.Name).IsRequired().HasMaxLength(255);
            a.Property(x => x.Bio).HasMaxLength(1000);
        });

        modelBuilder.Entity<Book>(b =>
        {
            b.HasKey(x => x.BookId);
            b.Property(x => x.Title).IsRequired().HasMaxLength(255);
            b.HasOne(x => x.Author).WithMany().HasForeignKey(nameof(Author.AuthorId)).IsRequired();
            b.HasMany(x => x.Reviews).WithOne().HasForeignKey(nameof(Book.BookId)).IsRequired();
        });
        
        modelBuilder.Entity<Review>(r =>
        {
            r.HasKey(x => x.ReviewId);
            r.Property(x => x.Rating).IsRequired();
            r.Property(x => x.Content).HasMaxLength(2000);
        });
    }
}