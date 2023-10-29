﻿using MagicVilla_CoupounAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_CoupounAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Coupon> Coupons { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coupon>().HasData(
                new Coupon()
                {
                    Id = 1,
                    Name = "10OFF",
                    Percentage = 10,
                    IsActive = true
                },
                new Coupon()
                {
                    Id = 2,
                    Name = "20OFF",
                    Percentage = 20,
                    IsActive = true
                }
                );
        }
    }
}