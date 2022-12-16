using FriendstagramApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection.Metadata;
using System.Text;
using System.Security.Cryptography;

namespace FriendstagramApi.Data
{
    public class FriendstagramContext : DbContext
    {
        public FriendstagramContext(DbContextOptions<FriendstagramContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Sharing> Sharings { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<FriendshipRequest> FriendshipRequests { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat> Chats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Friendship>()
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .Entity<Friendship>()
                .HasOne(e => e.Friend)
                .WithMany()
                .HasForeignKey("FriendId")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .Entity<FriendshipRequest>()
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .Entity<FriendshipRequest>()
                .HasOne(e => e.Friend)
                .WithMany()
                .HasForeignKey("FriendId")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .Entity<Message>()
                .HasOne(e => e.Sender)
                .WithMany()
                .HasForeignKey("SenderId")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .Entity<Message>()
                .HasOne(e => e.Receiver)
                .WithMany()
                .HasForeignKey("ReceiverId")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .Entity<Comment>()
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .Entity<Comment>()
                .HasOne(e => e.Sharing)
                .WithMany()
                .HasForeignKey("SharingId")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .Entity<Chat>()
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .Entity<Chat>()
                .HasOne(e => e.Friend)
                .WithMany()
                .HasForeignKey("FriendId")
                .OnDelete(DeleteBehavior.NoAction);


            var passwordHashForSeed = CreatePasswordHashForSeedData("123");

            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, UserName = "gandalf", Email = "gandalf@mail.com", PasswordHash = passwordHashForSeed, DisplayName = "Gandalf the white", ProfilePicture = "seed-gandalf-profile.jpg" },
                new User { UserId = 2, UserName = "aragorn", Email = "aragorn@mail.com", PasswordHash = passwordHashForSeed, DisplayName = "Aragorn", ProfilePicture = "seed-aragorn-profile.jpg" },
                new User { UserId = 3, UserName = "frodo_offical", Email = "frodo@mail.com", PasswordHash = passwordHashForSeed, DisplayName = "Frodo Offical", ProfilePicture = "seed-frodo-profile.jpg" },

                new User { UserId = 4, UserName = "sauron", Email = "sauron@mail.com", PasswordHash = passwordHashForSeed, DisplayName = "Sauron", ProfilePicture = "seed-sauron-profile.jpg" },
                new User { UserId = 5, UserName = "witch_king", Email = "witch_king@mail.com", PasswordHash = passwordHashForSeed, DisplayName = "Witch king", ProfilePicture = "seed-witch-profile.jpg" }
                );

            modelBuilder.Entity<Sharing>().HasData(
                new Sharing { SharingId = 1, UserId = 1, Path = "seed-sharing-1.jpg", SharingText = "I like shire" },
                new Sharing { SharingId = 2, UserId = 2, Path = "seed-sharing-2.jpg", SharingText = "" },
                new Sharing { SharingId = 3, UserId = 3, Path = "seed-sharing-3.jpg", SharingText = "me" },
                new Sharing { SharingId = 4, UserId = 1, Path = "seed-sharing-4.jpg", SharingText = "" },
                new Sharing { SharingId = 5, UserId = 2, Path = "seed-sharing-5.jpg", SharingText = "" },
                new Sharing { SharingId = 6, UserId = 3, Path = "seed-sharing-6.jpg", SharingText = "fellowship tbt" },
                new Sharing { SharingId = 7, UserId = 3, Path = "seed-sharing-7.jpg", SharingText = "me and ring" },

                new Sharing { SharingId = 8, UserId = 4, Path = "seed-sharing-8.jpg", SharingText = "" },
                new Sharing { SharingId = 9, UserId = 5, Path = "seed-sharing-9.jpg", SharingText = "" },

                new Sharing { SharingId = 10, UserId = 3, Path = "seed-sharing-10.jpg", SharingText = "my ring, not sauron's" }

                );

            modelBuilder.Entity<Friendship>().HasData(
                new Friendship { FriendshipId = 1, UserId = 1, FriendId = 2},
                new Friendship { FriendshipId = 2, UserId = 2, FriendId = 1 },

                new Friendship { FriendshipId = 3, UserId = 1, FriendId = 3 },
                new Friendship { FriendshipId = 4, UserId = 3, FriendId = 1 },

                new Friendship { FriendshipId = 5, UserId = 2, FriendId = 3 },
                new Friendship { FriendshipId = 6, UserId = 3, FriendId = 2 },

                new Friendship { FriendshipId = 7, UserId = 4, FriendId = 5 },
                new Friendship { FriendshipId = 8, UserId = 5, FriendId = 4 },

                new Friendship { FriendshipId = 9, UserId = 3, FriendId = 4 },
                new Friendship { FriendshipId = 10, UserId = 4, FriendId = 3 }
                );

            modelBuilder.Entity<Comment>().HasData(
                new Comment { CommentId = 1, UserId = 4, SharingId = 10, CommentText = "give it back", CreatedAt = DateTime.Now },
                new Comment { CommentId = 2, UserId = 3, SharingId = 10, CommentText = "nope", CreatedAt = DateTime.Now.AddSeconds(1) },
                new Comment { CommentId = 3, UserId = 4, SharingId = 10, CommentText = "ok", CreatedAt = DateTime.Now.AddSeconds(2) },
                new Comment { CommentId = 4, UserId = 2, SharingId = 10, CommentText = "nice", CreatedAt = DateTime.Now.AddSeconds(3) }
                );

        }



        private string CreatePasswordHashForSeedData(string password)
        {
            var md5 = new MD5CryptoServiceProvider();
            var md5data = md5.ComputeHash(Encoding.ASCII.GetBytes(password));
            var hashedPassword = Encoding.ASCII.GetString(md5data);

            return hashedPassword;
        }

    }
}
