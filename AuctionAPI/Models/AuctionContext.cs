
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text.Json;
namespace AuctionAPI.Models;
public class AuctionContext : DbContext
{
    public DbSet<Auction> Auctions { get; set; } = null!;
    public DbSet<Product> Products { get; set; }= null!;
    public DbSet<User> Users { get; set; }= null!;
    public DbSet<Bid> Bids { get; set; }= null!;
    public DbSet<Sale> Sales { get; set; }= null!;

    public DbSet<Image> Images {get; set;} = null!;
    // public DbSet<ProductImage> ProductImages {get; set;} = null!;


    public IQueryable<Sale> SalesWithIncludes 
        => Sales
            .Include(s => s.Product)
            .Include(s => s.Auction)
                .ThenInclude(a => a.Bids)
            .Include(s => s.WinningBid)
            .Include(s => s.Buyer)
            .Include(s => s.Seller);

    public IQueryable<Auction> AuctionsWithIncludes 
        => Auctions
            .Include(a => a.Owner)
            .Include(a => a.Product)
            .Include(a => a.Bids)
                .ThenInclude(b => b.User)
            .Include(a => a.Sale);

    public IQueryable<User> UsersWithIncludes
        => Users
            .Include(u => u.Auctions)
                .ThenInclude(a => a.Bids)
            .Include(u => u.Products)
            .Include(u => u.Bids)
                .ThenInclude(b => b.Auction)
            .Include(u => u.Buys)
            .Include(u => u.Sales)
            .Include(u => u.Hearts);


    //public string DbPath { get; }

    public AuctionContext(DbContextOptions<AuctionContext> options) : base(options)
    {
    }

    // public AuctionContext()
    // {
    //     // Set the database path
    //     //DbPath = System.IO.Path.Join("auktion.db");
    // }

    // Configure EF to use a SQLite database
    // protected override void OnConfiguring(DbContextOptionsBuilder options)
    //     => options.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Auction entity configurations
        modelBuilder.Entity<Auction>()
            .HasOne(a => a.Product);
            
        modelBuilder.Entity<Auction>()
            .HasIndex(a => a.ProductId)
            .IsUnique();

        modelBuilder.Entity<Auction>()
            .HasOne(a => a.Owner)
            .WithMany(u => u.Auctions);

        modelBuilder.Entity<Auction>()
            .HasMany(a => a.Bids)
            .WithOne(b => b.Auction);

        modelBuilder.Entity<Auction>()
            .OwnsOne(a => a.AuctionPeriod, period =>
            {
                period.Property(p => p.StartDate).HasColumnName("AuctionStartDate");
                period.Property(p => p.EndDate).HasColumnName("AuctionEndDate");
            });

        modelBuilder.Entity<Auction>()
            .OwnsOne(a => a.AuctionPricing, pricing =>
            {
                pricing.Property(p => p.MinimumBidPrice).HasColumnName("MinimumBidPrice");
                pricing.Property(p => p.MinimumSalePrice).HasColumnName("MinimumSalePrice");
            });

        // Product entity conf
        modelBuilder.Entity<Product>()
            .HasMany(a => a.ProductImages)
            .WithOne(i => i.Product);

        // Sale entity configurations
        modelBuilder.Entity<Sale>()
            .HasOne(s => s.Product)
            .WithOne(p => p.Sale)
            .HasForeignKey<Sale>(s => s.ProductId);

        modelBuilder.Entity<Sale>()
            .HasOne(s => s.Auction)
            .WithOne(a => a.Sale)
            .HasForeignKey<Sale>(s => s.AuctionId);

        modelBuilder.Entity<Sale>()
            .HasIndex(s => s.AuctionId)
            .IsUnique();

        modelBuilder.Entity<Sale>()
            .HasOne(s => s.Seller)
            .WithMany(a => a.Sales);

        modelBuilder.Entity<Sale>()
            .HasOne(s => s.Buyer)
            .WithMany(a => a.Buys);

        modelBuilder.Entity<Sale>()
            .HasOne(s => s.WinningBid);
        
        modelBuilder.Entity<Sale>()
            .OwnsOne(s => s.PaymentStatus,
            ps => {
                ps.Property(p => p.CurrentState).HasColumnName("PaymentStatus");
                ps.Property(p => p.ReceivedDate).HasColumnName("DatePaymentRecieved");
                ps.Property(p => p.SentDate).HasColumnName("DatePaymentSent");
            });
        
        modelBuilder.Entity<Sale>()
            .OwnsOne(s => s.DeliveryStatus,
            ps => {
                ps.Property(p => p.CurrentState).HasColumnName("DeliveryStatus");
                ps.Property(p => p.ReceivedDate).HasColumnName("DateDeliveryRecieved");
                ps.Property(p => p.SentDate).HasColumnName("DateDeliverySent");
            });

        // User entity configurations
        modelBuilder.Entity<User>()
            .HasMany(u => u.Products)
            .WithOne(p => p.Owner);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        
        modelBuilder.Entity<User>()
            .HasMany(u => u.Hearts)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "UserHeart", //join table
                j => j.HasOne<Auction>().WithMany().HasForeignKey("AuctionId"),
                j => j.HasOne<User>().WithMany().HasForeignKey("UserId"),
                j =>
                {
                    j.HasKey("UserId", "AuctionId"); // Composite key to enforce uniqueness
            });

        // Bid entity configurations
        modelBuilder.Entity<Bid>()
            .HasOne(b => b.User)
            .WithMany(u => u.Bids);

    }
}
        
// Seed data programmatically
public class AuctionContextSeeder
{
    public static void Seed(AuctionContext context)
    {
        User fred;
        if (!context.Users.Any())
        {
            fred = User.Create(new CreateUser
            {
                Name = "Fred",
                Email = "fredde@gmail.com",
                Password = "123456",
                Address = "Borås",
            }).Value!;

            var alice = User.Create(new CreateUser
            {
                Name = "Alice",
                Email = "alice@gmail.com",
                Password = "password123",
                Address = "Stockholm",
            }).Value!;

            context.Users.AddRange(fred, alice);

            if (!context.Products.Any())
            {
                var product = new Product
                {
                    Name = "Gaming Laptop",
                    Owner = fred // Associate with Fred
                };

                context.Products.Add(product);
            }
        }

        context.SaveChanges();
    }
}

