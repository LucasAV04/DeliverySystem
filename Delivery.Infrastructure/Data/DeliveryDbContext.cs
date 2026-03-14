using Delivery.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Delivery.Infrastructure.Data
{
    public class DeliveryDbContext : DbContext
    {
        public DeliveryDbContext(DbContextOptions<DeliveryDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Motorista> Motoristas { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Entrega> Entregas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ── Cliente ──────────────────────────────────────────
            modelBuilder.Entity<Cliente>(e =>
            {
                e.ToTable("clientes");
                e.HasKey(c => c.Id);
                e.Property(c => c.Nome).IsRequired().HasMaxLength(100);
                e.Property(c => c.Cpf).IsRequired().HasMaxLength(14);
                e.HasIndex(c => c.Cpf).IsUnique();
                e.Property(c => c.Email).IsRequired().HasMaxLength(150);
                e.Property(c => c.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20);
            });

            // ── Motorista ─────────────────────────────────────────
            modelBuilder.Entity<Motorista>(e =>
            {
                e.ToTable("motoristas");
                e.HasKey(m => m.Id);
                e.Property(m => m.Nome).IsRequired().HasMaxLength(100);
                e.Property(m => m.Telefone).IsRequired().HasMaxLength(20);
                e.Property(m => m.Cnh).IsRequired().HasMaxLength(20);
                e.HasIndex(m => m.Cnh).IsUnique();
                e.Property(m => m.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20);
            });

            // ── Veiculo ───────────────────────────────────────────
            modelBuilder.Entity<Veiculo>(e =>
            {
                e.ToTable("veiculos");
                e.HasKey(v => v.Id);
                e.Property(v => v.Placa).IsRequired().HasMaxLength(10);
                e.HasIndex(v => v.Placa).IsUnique();
                e.Property(v => v.Modelo).IsRequired().HasMaxLength(80);
                e.Property(v => v.Ano).IsRequired().HasMaxLength(4);
                e.Property(v => v.CapacidadeCarga).HasColumnType("numeric(10,2)");
                e.Property(v => v.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20);
            });

            // ── Pedido ────────────────────────────────────────────
            modelBuilder.Entity<Pedido>(e =>
            {
                e.ToTable("pedidos");
                e.HasKey(p => p.Id);
                e.Property(p => p.EnderecoEntrega).IsRequired().HasMaxLength(200);
                e.Property(p => p.DataSolicitacao).IsRequired();
                e.Property(p => p.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20);

                // Relacionamento: Pedido pertence a um Cliente
                e.HasOne<Cliente>()
                    .WithMany()
                    .HasForeignKey(p => p.ClienteId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Entrega ───────────────────────────────────────────
            modelBuilder.Entity<Entrega>(e =>
            {
                e.ToTable("entregas");
                e.HasKey(en => en.Id);
                e.Property(en => en.DataSaida).IsRequired();
                e.Property(en => en.DataEntrega);
                e.Property(en => en.Observacoes).HasMaxLength(500);
                e.Property(en => en.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20);

                // Relacionamentos
                e.HasOne<Pedido>()
                    .WithMany()
                    .HasForeignKey(en => en.PedidoId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne<Motorista>()
                    .WithMany()
                    .HasForeignKey(en => en.MotoristaId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne<Veiculo>()
                    .WithMany()
                    .HasForeignKey(en => en.VeiculoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}