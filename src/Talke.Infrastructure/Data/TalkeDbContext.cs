using Microsoft.EntityFrameworkCore;
using Talke.Domain.Entities;

namespace Talke.Infrastructure.Data;

public class TalkeDbContext : DbContext
{
    public TalkeDbContext(DbContextOptions<TalkeDbContext> options) : base(options) {}

    // Mapeamento das entidades já criadas no seu domínio
    public DbSet<Student> Students { get; set;}
    public DbSet<Teacher> Teachers { get; set;}
    public DbSet<Lesson> Lessons { get; set;}
    public DbSet<CreditWallet>  CreditWallets { get; set;}
    public DbSet<CreditPackage> CreditPackages { get; set;}
    public DbSet<TeacherRecommendation> TeacherRecommendations { get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurações adicionais de mapeamento podem ser feitas aqui, se necessário
    }
}