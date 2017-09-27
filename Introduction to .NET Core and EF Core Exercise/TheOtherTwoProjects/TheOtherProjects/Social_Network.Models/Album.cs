namespace Social_Network.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Album
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string BackgroundColor { get; set; }

        public bool IsPublic { get; set; } = true;

        public int UserId { get; set; }

        public User User { get; set; }

        public ICollection<SharedAlbum> SharedWith { get; set; } = new HashSet<SharedAlbum>();
        
        public ICollection<AlbumPicture> Pictures { get; set; } = new HashSet<AlbumPicture>();

        public ICollection<AlbumTag> Tags { get; set; } = new HashSet<AlbumTag>();
    }
}
