using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OsDsII.api.Models;

namespace OsDsII.api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }
        public DbSet<ServiceOrder> ServiceOrders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Criacao tabela Customer
            modelBuilder.Entity<Customer>()
                .ToTable("customer");
            
            modelBuilder.Entity<Customer>()
                .HasKey(c => c.Id); //Tem chave primaria
            
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .IsUnique();
            
            modelBuilder.Entity<Customer>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd(); //auto incremento de chave primária
            
            modelBuilder.Entity<Customer>()
                .Property(c => c.Name)
                .HasMaxLength(60)
                .IsRequired();
            
            modelBuilder.Entity<Customer>()
                .Property(c => c.Email)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<Customer>()
                .Property(c => c.Phone)
                .HasMaxLength(20);

            //Criacao tabela ServiceOrder
            modelBuilder.Entity<ServiceOrder>()
                .ToTable("service_order");

            modelBuilder.Entity<ServiceOrder>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<ServiceOrder>()
                .Property(s => s.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<ServiceOrder>()
                .Property(s => s.Description)
                .IsRequired(); //NOT NULL

            modelBuilder.Entity<ServiceOrder>()
                .Property(s => s.Price)
                .IsRequired();

            modelBuilder.Entity<ServiceOrder>()
                .Property(s => s.Status)
                .HasConversion(
                new EnumToStringConverter<StatusServiceOrder>() //Converte o enum para string
                );

            modelBuilder.Entity<ServiceOrder>()
                .Property(s => s.OpeningDate)
                .HasDefaultValue(DateTimeOffset.Now);
                // .HasDefaultValueSql("getdate()"); ou .HasDefaultValue(DateTimeOffset.Now);

            modelBuilder.Entity<ServiceOrder>()
                .Property(s => s.FinishDate)
                .HasDefaultValue(null);

            //Para chave estrangeira 1/N
            modelBuilder.Entity<ServiceOrder>()
                .HasOne(s => s.Customer)
                .WithMany(e => e.ServiceOrders)
                .IsRequired();

            modelBuilder.Entity<ServiceOrder>()
                .HasMany(s => s.Comments)
                .WithOne(e => e.ServiceOrder)
                .HasForeignKey(e => e.ServiceOrderId)
                .IsRequired();

            //Tabela Comments
            modelBuilder.Entity<Comment>()
                .ToTable("comentario");

            modelBuilder.Entity<Comment>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Comment>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Comment>()
                .Property(c => c.Description)
                .HasColumnType("text")
                .IsRequired();

            modelBuilder.Entity<Comment>()
                .Property(c => c.SendDate)
                .HasDefaultValue(DateTimeOffset.Now);

            //Chave estrangeira
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.ServiceOrder)
                .WithMany(e => e.Comments)
                .HasForeignKey(e => e.ServiceOrderId)
                .IsRequired();
        }
    }
}