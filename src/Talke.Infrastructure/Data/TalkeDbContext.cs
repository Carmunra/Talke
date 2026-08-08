using Microsoft.EntityFrameworkCore;
using Talke.Domain.Entities;

namespace Talke.Infrastructure.Data;

public class TalkeDbContext : DbContext
{
    public TalkeDbContext(DbContextOptions<TalkeDbContext> options) : base(options) {}

    // Mapeamento das entidades já criadas no seu domínio
    public DbSet<User> Users { get; set;}
    public DbSet<Student> Students { get; set;}
    public DbSet<Teacher> Teachers { get; set;}
    public DbSet<Lesson> Lessons { get; set;}
    public DbSet<CreditWallet>  CreditWallets { get; set;}
    public DbSet<CreditPackage> CreditPackages { get; set;}
    public DbSet<TeacherRecommendation> TeacherRecommendations { get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(user =>
        {
            user.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
            user.Property(u => u.LastName).IsRequired().HasMaxLength(50);
            user.Property(u => u.Email).IsRequired().HasMaxLength(256);
            user.Property(u => u.PasswordHash).IsRequired();
            user.HasIndex(u => u.Email).IsUnique();
        });
    }
}