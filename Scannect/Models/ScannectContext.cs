using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Scannect.Models
{
    public class ScannectContext : DbContext
    {
        public ScannectContext(DbContextOptions<ScannectContext> options) : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<ItemImage> ItemImages { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>().ToTable("Item");
            modelBuilder.Entity<ItemImage>().ToTable("ItemImage");
            modelBuilder.Entity<Tag>().ToTable("Tag");
        }

    }
}
