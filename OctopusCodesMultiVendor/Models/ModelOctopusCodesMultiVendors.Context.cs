using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace OctopusCodesMultiVendor.Models
{
    public partial class OctopusCodesMultiVendorsEntities : DbContext
    {
        public virtual DbSet<AccountCustomer> AccountCustomer { get; set; }
        public virtual DbSet<AccountVendor> AccountVendors { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<MemberShip> MemberShips { get; set; }
        public virtual DbSet<MemberShipVendor> MemberShipVendors { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Orders> Orderss { get; set; }
        public virtual DbSet<OrdersDetail> OrdersDetails { get; set; }
        public virtual DbSet<OrderStatus> OrderStatuss { get; set; }
        public virtual DbSet<Page> Pages { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }

        public virtual DbSet<Customer> Customer { get; set; }

        public virtual DbSet<Users> Users { get; set; }

        public virtual DbSet<CustomerVendor> CustomerVendor { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json");
                var configuration = builder.Build();
                optionsBuilder
                    .UseLazyLoadingProxies()
                    .UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
              

                entity.Property(e => e.Name)
                   .HasMaxLength(250)
                   .IsUnicode(false);

                entity.Property(e => e.Address)
                  .HasMaxLength(250)
                  .IsUnicode(false);

                entity.Property(e => e.Code)
                .HasMaxLength(250)
                .IsUnicode(false);

                entity.Property(e => e.Phone)
               .HasMaxLength(250)
               .IsUnicode(false);

                entity.Property(e => e.Rut)
             .HasMaxLength(50)
             .IsUnicode(false);

                entity.Property(e => e.RutDv)
           .HasMaxLength(50)
           .IsUnicode(false);
            });

                modelBuilder.Entity<Users>(entity =>
            {
                entity.HasIndex(e => e.Username)
                    .HasName("IX_Users")
                    .IsUnique();

                //entity.Property(e => e.Address)
                //   .HasMaxLength(250)
                //   .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                  .HasMaxLength(250)
                  .IsUnicode(false);
            });

            modelBuilder.Entity<AccountVendor>(entity =>
            {
                entity.HasIndex(e => e.Email)
                    .HasName("IX_AccountVendor_1")
                    .IsUnique();

             

                entity.Property(e => e.Email)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                //entity.Property(e => e.Username)
                //    .HasMaxLength(250)
                //    .IsUnicode(false);

          
            });

            modelBuilder.Entity<AccountCustomer>(entity =>
            {
                //entity.HasIndex(e => e.Username)
                //    .HasName("IX_Table_1")
                //    .IsUnique();
                entity.HasIndex(e => e.Email)
                .HasName("IX_AccountCustomer")
                .IsUnique();

                entity.Property(e => e.Address)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                //entity.Property(e => e.Username)
                //    .HasMaxLength(250)
                //    .IsUnicode(false);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParents)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Category_Category");

                entity.HasOne(d => d.Vendor)
                    .WithMany(p => p.Categories)
                    .HasForeignKey(d => d.VendorId)
                    .HasConstraintName("FK_Category_Vendor");
            });

            modelBuilder.Entity<CustomerVendor>(entity =>
            {

                entity.HasOne(d => d.Vendor)
                   .WithMany(p => p.CustomerVendors)
                   .HasForeignKey(d => d.VendorId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_CustomerVendors_Vendor");

                entity.HasOne(d => d.Customer)
                  .WithMany(p => p.CustomerVendors)
                  .HasForeignKey(d => d.CustomerId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_CustomerVendors_Customer");

                //entity.HasOne(d => d.Vendor)                    
                //    .HasForeignKey(d => d.)
                //    .HasConstraintName("FK_Category_Category");

                //entity.HasOne(d => d.Vendor)
                //    .WithMany(p => p.Categories)
                //    .HasForeignKey(d => d.VendorId)
                //    .HasConstraintName("FK_Category_Vendor");
            });


            modelBuilder.Entity<MemberShip>(entity =>
            {
                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MemberShipVendor>(entity =>
            {
                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.HasOne(d => d.MemberShip)
                    .WithMany(p => p.MemberShipVendors)
                    .HasForeignKey(d => d.MemerShipId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MemberShipVendor_MemberShip");

                entity.HasOne(d => d.Vendor)
                    .WithMany(p => p.MemberShipVendors)
                    .HasForeignKey(d => d.VendorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MemberShipVendor_Vendor");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.Property(e => e.Body).HasColumnType("ntext");

                entity.Property(e => e.DateCreation).HasColumnType("date");

                entity.Property(e => e.Title)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                //entity.HasOne(d => d.Customer)
                //    .WithMany(p => p.Messages)
                //    .HasForeignKey(d => d.CustomerId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_Message_Account");

                entity.HasOne(d => d.Vendor)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.VendorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Message_Vendor");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.Property(e => e.DateCreation).HasColumnType("date");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                //entity.HasOne(d => d.Customer)
                //    .WithMany(p => p.Orderss)
                //    .HasForeignKey(d => d.CustomerId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_Orders_Account");

                entity.HasOne(d => d.OrderStatus)
                    .WithMany(p => p.Orderss)
                    .HasForeignKey(d => d.OrderStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_OrderStatus");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.Orderss)
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("FK_Orders_Payment");

                entity.HasOne(d => d.Vendor)
                    .WithMany(p => p.Orderss)
                    .HasForeignKey(d => d.VendorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_Vendor");
            });

            modelBuilder.Entity<OrdersDetail>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId });

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrdersDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrdersDetail_Orders");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrdersDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrdersDetail_Product");
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Page>(entity =>
            {
                entity.HasIndex(e => e.Plug)
                    .HasName("IX_Page")
                    .IsUnique();

                entity.Property(e => e.Detail).HasColumnType("ntext");

                entity.Property(e => e.Plug)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Photos)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Photo_Product");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Category");

                entity.HasOne(d => d.Vendor)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.VendorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Vendor");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.Property(e => e.DatePost).HasColumnType("date");

                entity.Property(e => e.Detail).HasColumnType("ntext");

                //entity.HasOne(d => d.Customer)
                //    .WithMany(p => p.Reviews)
                //    .HasForeignKey(d => d.CustomerId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_Review_Account");

                entity.HasOne(d => d.Vendor)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.VendorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Review_Vendor");
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.Property(e => e.Group)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Key)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.TypeOfControl)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Value).HasColumnType("ntext");
            });

            modelBuilder.Entity<Vendor>(entity =>
            {
                //entity.HasIndex(e => e.Username)
                //    .HasName("IX_Vendor")
                //    .IsUnique();

                entity.Property(e => e.Address)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Logo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                //entity.Property(e => e.Password)
                //    .HasMaxLength(250)
                //    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                //entity.Property(e => e.Username)
                //    .HasMaxLength(250)
                //    .IsUnicode(false);
            });
        }
    }
}
