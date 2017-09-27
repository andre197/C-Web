namespace Social_Network.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Models;

    public class SocialNetworkDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<UserFriend> UserFriends { get; set; }

        public DbSet<Album> Albums { get; set; }

        public DbSet<Picture> Pictures { get; set; }

        public DbSet<AlbumPicture> AlbumPictures { get; set; }

        public DbSet<AlbumTag> AlbumTags { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<SharedAlbum> SharedAlbums { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(@"Server=LAPTOP-22QSF26P\SQLEXPRESS;Database=SocialNetworkTestDB;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserFriend>()
                .HasKey(uf => new { uf.UserId, uf.FriendId });

            builder.Entity<UserFriend>()
                .HasOne(uf => uf.User)
                .WithMany(u => u.FriendsOfMine)
                .HasForeignKey(uf => uf.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserFriend>()
                .HasOne(uf => uf.Friend)
                .WithMany(f => f.FriendsToMe)
                .HasForeignKey(uf => uf.FriendId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Album>()
                .HasOne(a => a.User)
                .WithMany(u => u.Albums)
                .HasForeignKey(a => a.UserId);

            builder.Entity<AlbumPicture>()
                .HasKey(ap => new { ap.AlbumId, ap.PictureId });

            builder.Entity<AlbumPicture>()
                .HasOne(ap => ap.Album)
                .WithMany(a => a.Pictures)
                .HasForeignKey(ap => ap.AlbumId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AlbumPicture>()
                .HasOne(ap => ap.Picture)
                .WithMany(p => p.Albums)
                .HasForeignKey(ap => ap.PictureId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AlbumTag>()
                .HasKey(at => new {at.AlbumId, at.TagId});

            builder.Entity<AlbumTag>()
                .HasOne(at => at.Album)
                .WithMany(a => a.Tags)
                .HasForeignKey(at => at.AlbumId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AlbumTag>()
                .HasOne(at => at.Tag)
                .WithMany(t => t.Albums)
                .HasForeignKey(at => at.TagId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SharedAlbum>()
                .HasKey(sa => new {sa.UserId, sa.AlbumId});

            builder.Entity<SharedAlbum>()
                .HasOne(sa => sa.User)
                .WithMany(u => u.SharedWithMe)
                .HasForeignKey(sa => sa.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SharedAlbum>()
                .HasOne(sa => sa.Album)
                .WithMany(a => a.SharedWith)
                .HasForeignKey(sa => sa.AlbumId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var serviceProvider = this.GetService<IServiceProvider>();

            var items = new Dictionary<object, object>();

            foreach (var entry in this.ChangeTracker.Entries().Where(e => (e.State == EntityState.Added) || (e.State == EntityState.Modified)))
            {
                var entity = entry.Entity;

                var context = new ValidationContext(entity, serviceProvider, items);

                var results = new List<ValidationResult>();

                if (Validator.TryValidateObject(entity, context, results, true) == false)
                {
                    foreach (var result in results)
                    {
                        if (result != ValidationResult.Success)
                        {
                            throw new ValidationException(result.ErrorMessage);
                        }
                    }
                }
            }

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
    }
}
