namespace Social_Network.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class StartUp
    {
        private static Random random = new Random();
        private const int totalUsers = 30;
        private static string separator = new string('#', 100);

        public static void Main()
        {
            using (SocialNetworkDbContext db = new SocialNetworkDbContext())
            {
                //Print(TagTransformator.Transform("abcd efghijklmnopqrstuvwxyz"));

                //db.Database.Migrate();

                //SeedData(db);

                //Print(UsersWithFriendsCount(db));
                //Print(ActiveUsersWithMoreThan6Friends(db));

                //Print(AlbumDetails(db));
                //Print(PicturesInMoreThan2Albums(db));
                //Print(AlbumFullDetails(db));

                //Print(AllAlbumsWithTag(db));
                //Print(NameTitleTagsOfUserWithAlbumsHavingMoreThanThreeTags(db));

                //Print(UsersWithSharedAlbumsAndTheyUserWithWhichTheAlbumsWereShared(db));
                //Print(AlbumsSharedWithMoreThan2People(db));
                //Print(AlbumsSharedWithUserByName(db));
            }
        }

        private static string UsersWithFriendsCount(SocialNetworkDbContext db)
        {
            StringBuilder sb = new StringBuilder();

            var data = db.Users.Select(u => new
            {
                Name = u.Username,
                FriendsCount = u.FriendsOfMine.Count + u.FriendsToMe.Count,
                Status = u.IsDeleted
            })
                .OrderByDescending(d => d.FriendsCount)
                .ThenBy(d => d.Name)
                .ToList();

            foreach (var user in data)
            {
                string status = !user.Status ? "Active" : "Inactive";

                sb.AppendLine($"{user.Name} has {user.FriendsCount} friends and is {status}");

                sb.AppendLine(separator);
            }

            return sb.ToString();
        }

        private static string ActiveUsersWithMoreThan6Friends(SocialNetworkDbContext db)
        {
            StringBuilder sb = new StringBuilder();

            var data = db.Users
                .Where(u => u.IsDeleted == false)
                .OrderBy(u => u.RegisteredOn)
                .ThenByDescending(u => u.FriendsOfMine.Count + u.FriendsToMe.Count)
                .Select(u => new
                {
                    u.Username,
                    FirendsCount = u.FriendsOfMine.Count + u.FriendsToMe.Count,
                    ActivePeriod = DateTime.Now.Subtract(u.RegisteredOn).Days
                })
                .ToList();

            foreach (var user in data)
            {
                sb.AppendLine(
                    $"{user.Username} has {user.FirendsCount} firends and he is being active for {user.ActivePeriod} days");

                sb.AppendLine(separator);
            }

            return sb.ToString();
        }

        private static string AlbumDetails(SocialNetworkDbContext db)
        {
            StringBuilder sb = new StringBuilder();

            var data = db.Albums
                .Select(a => new
                {
                    a.Name,
                    a.User.Username,
                    PicsCount = a.Pictures.Count
                })
                .OrderByDescending(d => d.PicsCount)
                .ThenBy(d => d.Username)
                .ToList();

            foreach (var album in data)
            {
                sb.AppendLine($"Album {album.Name} is owned by {album.Username} and has {album.PicsCount} pictures");

                sb.AppendLine(separator);
            }

            return sb.ToString();
        }

        private static string PicturesInMoreThan2Albums(SocialNetworkDbContext db)
        {
            StringBuilder sb = new StringBuilder();

            var data = db.Pictures
                .Where(p => p.Albums.Any())
                .Where(p => p.Albums.Count > 2)
                .Select(p => new
                {
                    p.Title,
                    Albums = p.Albums
                        .Select(a => new
                        {
                            a.Album.Name,
                            a.Album.User.Username
                        })
                })
                .OrderByDescending(d => d.Albums.Count())
                .ThenBy(d => d.Title)
                .ToList();

            foreach (var picture in data)
            {
                sb.AppendLine($"Picture {picture.Title} is included in albums:");


                foreach (var album in picture.Albums)
                {
                    sb.AppendLine($"---{album.Name} owned by {album.Username}");
                }

                sb.AppendLine(separator);
            }

            return sb.ToString();
        }

        private static string AlbumFullDetails(SocialNetworkDbContext db)
        {
            StringBuilder sb = new StringBuilder();

            int userId = int.Parse(Console.ReadLine());

            //var userWithPrivateAlbum = db.Users.FirstOrDefault(u => u.Id == userId);

            //userWithPrivateAlbum.Albums.Add(new Album()
            //{
            //    Name = "Private Albume 1",
            //    BackgroundColor = "none",
            //    IsPublic = false
            //});

            //db.SaveChanges();

            string newLine = Environment.NewLine;

            var user = db.Users
                .Where(u => u.Id == userId)
                .Select(u => new
                {
                    u.Username,
                    Albums = u.Albums
                        .Select(a => new
                        {
                            a.Name,
                            AlbumContent = a.IsPublic ? $"{string.Join(newLine, a.Pictures.Select(p => p.Picture))}" : "Private Content"
                        })
                        .OrderBy(albs => albs.Name)
                })
                .FirstOrDefault();

            sb.AppendLine($"{user.Username} has albums:");

            foreach (var album in user.Albums)
            {
                sb.AppendLine($"{album.Name} which has pictures:");
                sb.AppendLine($"{album.AlbumContent}");

                sb.AppendLine(separator);
            }

            return sb.ToString();
        }

        private static string AllAlbumsWithTag(SocialNetworkDbContext db)
        {
            StringBuilder sb = new StringBuilder();

            int tagId = int.Parse(Console.ReadLine());

            var data = db.Albums
                .Where(a => a.Tags
                    .Any(t => t.TagId == tagId))
                .OrderByDescending(a => a.Tags.Count)
                .ThenBy(a => a.Name)
                .Select(a => new
                {
                    a.Name,
                    a.User.Username
                })
                .ToList();

            foreach (var album in data)
            {
                sb.AppendLine($"{album.Name} owned by {album.Username}");
            }

            sb.AppendLine(separator);

            return sb.ToString();
        }

        private static string NameTitleTagsOfUserWithAlbumsHavingMoreThanThreeTags(SocialNetworkDbContext db)
        {
            StringBuilder sb = new StringBuilder();

            var data = db.Users
                .Where(u => u.Albums.Any(a => a.Tags.Count > 3))
                .Select(u => new
                {
                    u.Username,
                    Albums = u.Albums
                        .Where(album => album.Tags.Count > 3)
                        .Select(album => new
                        {
                            album.Name,
                            Tags = album.Tags.Select(t => t.Tag.TagName)
                        })
                    //.ToList() // Without ToList() throws exception ̄̄ ̄ _(ツ)_/ ̄ ̄  
                    //System.InvalidOperationException: variable 'album' of type 'Microsoft.EntityFrameworkCore.Storage.ValueBuffer' referenced from scope '', but it is not defined
                })
                .OrderByDescending(d => d.Albums.Count())
                .ThenByDescending(d => d.Albums.Sum(a => a.Tags.Count()))
                .ThenBy(d => d.Username)
                .ToList();

            foreach (var user in data)
            {
                sb.AppendLine($"{user.Username} has albums:");

                foreach (var album in user.Albums)
                {
                    sb.AppendLine($"---{album.Name} with tags:")
                       .AppendLine($"------{string.Join(", ", album.Tags)}");
                }

                sb.AppendLine(separator);
            }

            return sb.ToString();
        }

        private static string UsersWithSharedAlbumsAndTheyUserWithWhichTheAlbumsWereShared(SocialNetworkDbContext db)
        {
            StringBuilder sb = new StringBuilder();

            var dbcheck = db.Users.Where(u => u.FriendsToMe.Any() || u.FriendsOfMine.Any()).Select(u => u.FriendsOfMine.Concat(u.FriendsToMe).ToList()).FirstOrDefault();

            var data = db.Users
                .Where(u => u.Albums.Any(a => a.SharedWith.Count != 0))
                .Where(u => u.FriendsToMe.Any() || u.FriendsOfMine.Any())
                .Select(u => new
                {
                    u.Username,
                    FriendsToMeNames = u.FriendsToMe.Select(f => f.User.Username),
                    FriendsOfMineNames = u.FriendsOfMine.Select(f => f.Friend.Username),
                    NameOfAlbumsSharedWithFriends = u.Albums
                        .Where(a => a.SharedWith
                            .All(s => u.FriendsToMe
                                            .Any(f => f.FriendId == s.UserId)
                                        || u.FriendsOfMine
                                            .Any(fm => fm.FriendId == s.UserId)))
                        .Select(a => a.Name)
                })
                .OrderBy(d => d.Username);

            foreach (var user in data)
            {
                sb.AppendLine($"{user.Username} is friend with:")
                    .AppendLine($"---{string.Join(", ", user.FriendsOfMineNames.Concat(user.FriendsToMeNames))}; and shared with them:")
                    .AppendLine($"------{string.Join(", ", user.NameOfAlbumsSharedWithFriends)} albums");

                sb.AppendLine(separator);
            }

            return sb.ToString();
        }

        private static string AlbumsSharedWithMoreThan2People(SocialNetworkDbContext db)
        {
            StringBuilder sb = new StringBuilder();

            var data = db.Albums
                .Where(a => a.SharedWith.Count > 2)
                .Select(a => new
                {
                    a.Name,
                    a.SharedWith.Count,
                    Public = a.IsPublic ? "Public" : "Private"
                })
                .OrderByDescending(d => d.Count)
                .ThenBy(d => d.Name)
                .ToList();

            foreach (var album in data)
            {
                sb.AppendLine($"{album.Name} is shared with {album.Count} people and is {album.Public}");
            }

            sb.AppendLine(separator);

            return sb.ToString();
        }

        private static string AlbumsSharedWithUserByName(SocialNetworkDbContext db)
        {
            StringBuilder sb = new StringBuilder();

            string username = "New User 27";//Console.ReadLine();

            var data = db.Users
                .Where(u => u.Username == username)
                .Select(u => new
                {
                    Albums = u.SharedWithMe.Select(sw => sw.Album.Name).ToList(),
                    Count = u.SharedWithMe.Select(sw => sw.Album.Pictures.Count).ToList()
                })
                .OrderByDescending(d => d.Count)
                .ThenBy(d => d.Albums)
                .FirstOrDefault();

            sb.AppendLine("The Following Albums are shared with this user:");

            for (int i = 0; i < data.Albums.Count; i++)
            {
                sb.AppendLine($"----{data.Albums[i]} which has {data.Count[i]} pictures");
            }
            sb.AppendLine($"{separator}");

            return sb.ToString();
        }

        private static void Print(string output)
        {
            Console.WriteLine(output);
        }

        private static void SeedData(SocialNetworkDbContext db)
        {
            SeedUsers(db);
            AddFriends(db);
            AddAlbums(db);
            AddPictures(db);
            AddPicturesInAlbums(db);
            AddTags(db);
            AddTagsToAlbums(db);
            ShareAlbums(db);
        }

        private static void SeedUsers(SocialNetworkDbContext db)
        {
            for (int i = 0; i < totalUsers; i++)
            {
                db.Users.Add(new User
                {
                    Username = $"New User {i}",
                    Email = $"a{i}@abv.bg",
                    Age = random.Next(18, 25),
                    Password = $"12345{i}",
                    RegisteredOn = DateTime.Now.AddDays(-1 * (i + 1)),
                    LastTimeLogedIn = DateTime.Now.AddHours(-1 * (i + 1)),
                    IsDeleted = false
                });
            }

            db.SaveChanges();

            // add inactive user
            db.Users.Add(new User
            {
                Username = "Inactive User 1",
                Email = "new@email.bg",
                Password = "blablablallslfs",
                LastTimeLogedIn = DateTime.Now,
                RegisteredOn = DateTime.Now,
                Age = 15,
                IsDeleted = true
            });

            db.SaveChanges();
        }

        private static void AddFriends(SocialNetworkDbContext db)
        {
            var users = db.Users.Select(u => u.Id).ToList();

            for (int i = 0; i < totalUsers; i++)
            {
                int numberOfFriends = random.Next(5, 7);
                int currentUserId = users[i];

                for (int j = 0; j < numberOfFriends; j++)
                {
                    int randomFriendId = users[random.Next(0, users.Count)];

                    // cannot befriend myself
                    bool validFriendship = currentUserId != randomFriendId;

                    bool friendshipExists = db.UserFriends
                        .Any(uf => (uf.FriendId == currentUserId && uf.UserId == randomFriendId)
                                   || (uf.FriendId == randomFriendId && uf.UserId == currentUserId));

                    if (friendshipExists)
                    {
                        validFriendship = false;
                    }

                    if (!validFriendship)
                    {
                        j--;
                        continue;
                    }

                    db.UserFriends.Add(new UserFriend { UserId = currentUserId, FriendId = randomFriendId });

                    db.SaveChanges();
                }
            }
        }

        private static void AddAlbums(SocialNetworkDbContext db)
        {
            var usersIds = db.Users.Select(u => u.Id).ToList();

            for (int i = 0; i < usersIds.Count; i++)
            {
                int currentUserId = usersIds[i];
                int numberOfAlbums = random.Next(1, 3);

                for (int j = 0; j < numberOfAlbums; j++)
                {
                    db.Albums.Add(new Album()
                    {
                        Name = $"New Album {i}.{j}",
                        BackgroundColor = $"NewBackGroundColor {i}.{j}",
                        UserId = currentUserId
                    });
                }

            }
            db.SaveChanges();
        }

        private static void AddPictures(SocialNetworkDbContext db)
        {

            string[] pics = { "Lake", "Frog", "FrogInLake", "LakeInFrog" };

            int totalPics = db.Albums.Count() * 3;

            for (int j = 0; j < totalPics; j++)
            {
                string pic = pics[random.Next(0, pics.Length)];

                db.Pictures.Add(new Picture
                {
                    Title = pic,
                    Caption = $"this is a {pic}",
                    Path = $@"Pictures\{pic}.jpg"
                });
            }

            db.SaveChanges();
        }

        private static void AddPicturesInAlbums(SocialNetworkDbContext db)
        {
            var albums = db.Albums.ToList();
            var picsIds = db.Pictures.Select(p => p.Id).ToList();

            for (int i = 0; i < albums.Count; i++)
            {
                var currentAlbum = albums[i];

                for (int j = 0; j < 3; j++)
                {
                    var currentPicId = picsIds[random.Next(0, picsIds.Count)];

                    if (currentAlbum.Pictures.Any(p => p.PictureId == currentPicId))
                    {
                        j--;
                        continue;
                    }

                    currentAlbum.Pictures.Add(new AlbumPicture
                    {
                        PictureId = currentPicId
                    });
                }

                db.SaveChanges();
            }
        }

        private static void AddTags(SocialNetworkDbContext db)
        {
            int totalTags = db.Albums.Count() * 5;

            string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            for (int i = 0; i < totalTags; i++)
            {
                int tagLenght = random.Next(3, 14);

                StringBuilder sb = new StringBuilder();

                for (int j = 0; j < tagLenght; j++)
                {
                    sb.Append(alpha[random.Next(0, alpha.Length - 1)]);
                }

                db.Tags.Add(new Tag
                {
                    TagName = "#" + sb
                });
            }

            db.SaveChanges();
        }

        private static void AddTagsToAlbums(SocialNetworkDbContext db)
        {
            var albums = db.Albums.ToList();
            var TagIds = db.Tags.Select(t => t.Id).ToList();

            for (int i = 0; i < albums.Count; i++)
            {
                var currentAlbum = albums[i];

                int totalTagsForThisAlbum = random.Next(2, 8);

                for (int j = 0; j < totalTagsForThisAlbum; j++)
                {
                    int randomTagId = TagIds[random.Next(0, TagIds.Count)];

                    if (currentAlbum.Tags.Any(t => t.TagId == randomTagId))
                    {
                        j--;
                        continue;
                    }

                    currentAlbum.Tags.Add(new AlbumTag { TagId = randomTagId });
                }
            }

            db.SaveChanges();
        }

        private static void ShareAlbums(SocialNetworkDbContext db)
        {
            var albums = db.Albums.ToList();
            var userIds = db.Users.Select(u => u.Id).ToList();

            for (int i = 0; i < albums.Count; i++)
            {
                if (i % 3 == 0)
                {
                    i++;
                }

                var currentAlbum = albums[i];

                int totalPeopleToBeSharedWith = random.Next(3, 7);

                for (int j = 0; j < totalPeopleToBeSharedWith; j++)
                {
                    int userToBeSharedWith = userIds[random.Next(0, userIds.Count)];

                    if (currentAlbum.SharedWith.Any(sw => sw.UserId == userToBeSharedWith) || currentAlbum.UserId == userToBeSharedWith)
                    {
                        j--;
                        continue;
                    }

                    currentAlbum.SharedWith.Add(new SharedAlbum()
                    {
                        UserId = userToBeSharedWith
                    });
                }

                db.SaveChanges();
            }
        }
    }
}
