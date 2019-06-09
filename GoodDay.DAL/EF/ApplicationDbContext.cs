using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoodDay.DAL.EF
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Dialog> Dialogs { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasOne<File>(s => s.File)
            .WithOne(ad => ad.User)
            .HasForeignKey<File>(ad => ad.UserId);

            modelBuilder.Entity<Contact>()
                .HasOne(a => a.User)
                .WithMany(b => b.UsersContacts)
                .HasForeignKey(c => c.UserId);
            modelBuilder.Entity<Contact>()
                .HasOne(a => a.Friend)
                .WithMany(b => b.UserInContacts)
                .HasForeignKey(c => c.FriendId);
            base.OnModelCreating(modelBuilder);
        }
    }
}
