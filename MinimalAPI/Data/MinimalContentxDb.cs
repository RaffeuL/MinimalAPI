using MinimalAPI.Models;
    using Microsoft.EntityFrameworkCore;

namespace MinimalAPI.Data
{
    public class MinimalContentxDb: DbContext
    {
        public MinimalContentxDb(DbContextOptions<MinimalContentxDb> options) : base(options) { }

        public DbSet<Fornecedor> Forncedores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fornecedor>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Fornecedor>()
                .Property(p => p.Nome)
                .IsRequired()
                .HasColumnType("varchar(200)");

            modelBuilder.Entity<Fornecedor>()
                .Property(p => p.Documento)
                .IsRequired()
                .HasColumnType("varchar(14)");

            modelBuilder.Entity<Fornecedor>()
                .ToTable("Fornecedores");

            base.OnModelCreating(modelBuilder);

        }
    }
}
