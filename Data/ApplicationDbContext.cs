using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ervilhinha.Models;

namespace Ervilhinha.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<BabyItem> BabyItems { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        // Baby Wishlist System
        public DbSet<BabyWishlist> BabyWishlists { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<WishlistManager> WishlistManagers { get; set; }
        public DbSet<WishlistShare> WishlistShares { get; set; }
        public DbSet<WishlistReservation> WishlistReservations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configurar precisão dos decimais (18,2) - formato monetário
            builder.Entity<BabyItem>()
                .Property(b => b.EstimatedCost)
                .HasPrecision(18, 2);

            builder.Entity<BabyItem>()
                .Property(b => b.ActualCost)
                .HasPrecision(18, 2);

            builder.Entity<Expense>()
                .Property(e => e.Amount)
                .HasPrecision(18, 2);

            builder.Entity<Invoice>()
                .Property(i => i.TotalAmount)
                .HasPrecision(18, 2);

            builder.Entity<WishlistItem>()
                .Property(w => w.EstimatedPrice)
                .HasPrecision(18, 2);

            // Configurar relacionamentos da Wishlist
            builder.Entity<BabyWishlist>()
                .HasMany(w => w.Managers)
                .WithOne(m => m.Wishlist)
                .HasForeignKey(m => m.WishlistId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BabyWishlist>()
                .HasMany(w => w.Items)
                .WithOne(i => i.Wishlist)
                .HasForeignKey(i => i.WishlistId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BabyWishlist>()
                .HasMany(w => w.Shares)
                .WithOne(s => s.Wishlist)
                .HasForeignKey(s => s.WishlistId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<WishlistItem>()
                .HasMany(i => i.Reservations)
                .WithOne(r => r.WishlistItem)
                .HasForeignKey(r => r.WishlistItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índices para performance
            builder.Entity<BabyWishlist>()
                .HasIndex(w => w.ShareCode)
                .IsUnique();

            builder.Entity<WishlistShare>()
                .HasIndex(s => s.SharedWithEmail);

            builder.Entity<WishlistManager>()
                .HasIndex(m => m.ManagerEmail);
        }
    }
}
