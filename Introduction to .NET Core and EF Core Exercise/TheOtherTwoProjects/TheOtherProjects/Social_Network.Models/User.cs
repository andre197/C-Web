namespace Social_Network.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(4, 30)]
        public string Username { get; set; }

        [Required]
        [Range(6, 50)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public ICollection<Album> Albums { get; set; } = new HashSet<Album>();

        public ICollection<SharedAlbum> SharedWithMe { get; set; } = new HashSet<SharedAlbum>();

        public DateTime RegisteredOn { get; set; }

        public DateTime LastTimeLogedIn { get; set; }

        [Range(1, 120)]
        public int Age { get; set; }

        public bool IsDeleted { get; set; } = false;

        public ICollection<UserFriend> FriendsOfMine { get; set; } = new HashSet<UserFriend>();

        public ICollection<UserFriend> FriendsToMe { get; set; } = new HashSet<UserFriend>();
    }
}
