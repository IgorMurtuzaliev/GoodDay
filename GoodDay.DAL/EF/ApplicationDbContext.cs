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

        public DbSet<BlockList> BlockLists { get; set; }
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

            modelBuilder.Entity<Dialog>()
               .HasOne(a => a.User1)
               .WithMany(b => b.UsersDialogs)
               .HasForeignKey(c => c.User1Id);

            modelBuilder.Entity<Dialog>()
               .HasOne(a => a.User2)
               .WithMany(b => b.InterlocutorsDialogs)
               .HasForeignKey(c => c.User2Id);
            modelBuilder.Entity<Message>()
               .HasOne(a => a.Dialog)
               .WithMany(b => b.Messages)
               .HasForeignKey(c => c.DialogId);
            modelBuilder.Entity<Message>()
               .HasOne(a => a.Sender)
               .WithMany(b => b.Messages)
               .HasForeignKey(c => c.SenderId);

            modelBuilder.Entity<BlockList>()
               .HasOne(a => a.User)
               .WithMany(b => b.UsersBlockList)
               .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<BlockList>()
                .HasOne(a => a.Friend)
                .WithMany(b => b.UserInBlockList)
                .HasForeignKey(c => c.FriendId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
