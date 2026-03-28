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

        // Baby Wishlist System (LEGACY - será migrado para BabyList)
        public DbSet<BabyWishlist> BabyWishlists { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<WishlistManager> WishlistManagers { get; set; }
        public DbSet<WishlistShare> WishlistShares { get; set; }
        public DbSet<WishlistReservation> WishlistReservations { get; set; }

        // Smart Planning System - NEW
        public DbSet<BabyCostSimulator> BabyCostSimulators { get; set; }
        public DbSet<FamilyBudget> FamilyBudgets { get; set; }
        public DbSet<BabyShoppingItem> BabyShoppingItems { get; set; } // LEGACY - será migrado para BabyList
        public DbSet<BabyTimeline> BabyTimelines { get; set; }
        public DbSet<SmartAlert> SmartAlerts { get; set; }

        // Unified Baby Lists System (NOVO - substitui BabyWishlist + BabyShoppingItem)
        public DbSet<BabyList> BabyLists { get; set; }
        public DbSet<BabyListItem> BabyListItems { get; set; }
        public DbSet<BabyListManager> BabyListManagers { get; set; }
        public DbSet<BabyListShare> BabyListShares { get; set; }
        public DbSet<BabyListReservation> BabyListReservations { get; set; }

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

            // Configurar precisão dos decimais para novos modelos
            builder.Entity<BabyCostSimulator>()
                .Property(b => b.MonthlyIncome)
                .HasPrecision(18, 2);
            builder.Entity<BabyCostSimulator>()
                .Property(b => b.EstimatedPregnancyCost)
                .HasPrecision(18, 2);
            builder.Entity<BabyCostSimulator>()
                .Property(b => b.EstimatedBirthCost)
                .HasPrecision(18, 2);
            builder.Entity<BabyCostSimulator>()
                .Property(b => b.Estimated0to6MonthsCost)
                .HasPrecision(18, 2);
            builder.Entity<BabyCostSimulator>()
                .Property(b => b.Estimated6to12MonthsCost)
                .HasPrecision(18, 2);
            builder.Entity<BabyCostSimulator>()
                .Property(b => b.EstimatedTotalFirstYear)
                .HasPrecision(18, 2);
            builder.Entity<BabyCostSimulator>()
                .Property(b => b.EstimatedMonthlyImpact)
                .HasPrecision(18, 2);

            builder.Entity<FamilyBudget>()
                .Property(f => f.TotalIncome)
                .HasPrecision(18, 2);
            builder.Entity<FamilyBudget>()
                .Property(f => f.BudgetBaby)
                .HasPrecision(18, 2);
            builder.Entity<FamilyBudget>()
                .Property(f => f.BudgetHouse)
                .HasPrecision(18, 2);
            builder.Entity<FamilyBudget>()
                .Property(f => f.BudgetFood)
                .HasPrecision(18, 2);
            builder.Entity<FamilyBudget>()
                .Property(f => f.BudgetTransport)
                .HasPrecision(18, 2);
            builder.Entity<FamilyBudget>()
                .Property(f => f.BudgetHealth)
                .HasPrecision(18, 2);
            builder.Entity<FamilyBudget>()
                .Property(f => f.BudgetLeisure)
                .HasPrecision(18, 2);
            builder.Entity<FamilyBudget>()
                .Property(f => f.BudgetSavings)
                .HasPrecision(18, 2);
            builder.Entity<FamilyBudget>()
                .Property(f => f.BudgetOther)
                .HasPrecision(18, 2);
            builder.Entity<FamilyBudget>()
                .Property(f => f.ActualBaby)
                .HasPrecision(18, 2);
            builder.Entity<FamilyBudget>()
                .Property(f => f.ActualHouse)
                .HasPrecision(18, 2);
            builder.Entity<FamilyBudget>()
                .Property(f => f.ActualFood)
                .HasPrecision(18, 2);
            builder.Entity<FamilyBudget>()
                .Property(f => f.ActualTransport)
                .HasPrecision(18, 2);
            builder.Entity<FamilyBudget>()
                .Property(f => f.ActualHealth)
                .HasPrecision(18, 2);
            builder.Entity<FamilyBudget>()
                .Property(f => f.ActualLeisure)
                .HasPrecision(18, 2);
            builder.Entity<FamilyBudget>()
                .Property(f => f.ActualOther)
                .HasPrecision(18, 2);
            builder.Entity<FamilyBudget>()
                .Property(f => f.ForecastBaby)
                .HasPrecision(18, 2);
            builder.Entity<FamilyBudget>()
                .Property(f => f.ForecastTotal)
                .HasPrecision(18, 2);

            builder.Entity<BabyShoppingItem>()
                .Property(b => b.EstimatedCost)
                .HasPrecision(18, 2);
            builder.Entity<BabyShoppingItem>()
                .Property(b => b.ActualCost)
                .HasPrecision(18, 2);

            builder.Entity<BabyTimeline>()
                .Property(b => b.EstimatedCost)
                .HasPrecision(18, 2);
            builder.Entity<BabyTimeline>()
                .Property(b => b.ActualCost)
                .HasPrecision(18, 2);

            builder.Entity<SmartAlert>()
                .Property(s => s.AssociatedAmount)
                .HasPrecision(18, 2);

            // Índices para performance dos novos modelos
            builder.Entity<BabyCostSimulator>()
                .HasIndex(b => b.UserId);

            builder.Entity<FamilyBudget>()
                .HasIndex(f => new { f.UserId, f.Year, f.Month })
                .IsUnique();

            builder.Entity<BabyShoppingItem>()
                .HasIndex(b => b.UserId);

            builder.Entity<BabyTimeline>()
                .HasIndex(b => new { b.UserId, b.EventDate });

            builder.Entity<SmartAlert>()
                .HasIndex(s => new { s.UserId, s.IsRead, s.CreatedDate });

            // ========== UNIFIED BABY LIST SYSTEM CONFIGURATION ==========

            // Configurar precisão dos decimais
            builder.Entity<BabyListItem>()
                .Property(i => i.EstimatedCost)
                .HasPrecision(18, 2);
            builder.Entity<BabyListItem>()
                .Property(i => i.ActualCost)
                .HasPrecision(18, 2);

            // Configurar relacionamentos
            builder.Entity<BabyList>()
                .HasMany(l => l.Items)
                .WithOne(i => i.BabyList)
                .HasForeignKey(i => i.BabyListId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BabyList>()
                .HasMany(l => l.Managers)
                .WithOne(m => m.BabyList)
                .HasForeignKey(m => m.BabyListId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BabyList>()
                .HasMany(l => l.Shares)
                .WithOne(s => s.BabyList)
                .HasForeignKey(s => s.BabyListId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BabyListItem>()
                .HasMany(i => i.Reservations)
                .WithOne(r => r.BabyListItem)
                .HasForeignKey(r => r.BabyListItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índices para performance
            builder.Entity<BabyList>()
                .HasIndex(l => l.UserId);

            builder.Entity<BabyList>()
                .HasIndex(l => l.ShareCode)
                .IsUnique()
                .HasFilter("[ShareCode] IS NOT NULL");

            builder.Entity<BabyListManager>()
                .HasIndex(m => m.ManagerEmail);

            builder.Entity<BabyListShare>()
                .HasIndex(s => s.SharedWithEmail);

            builder.Entity<BabyListReservation>()
                .HasIndex(r => r.ReservedBy);
        }
    }
}
