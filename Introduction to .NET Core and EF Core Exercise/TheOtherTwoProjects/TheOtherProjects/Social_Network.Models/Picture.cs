namespace Social_Network.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Picture
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Caption { get; set; }

        public string Path { get; set; }

        public ICollection<AlbumPicture> Albums { get; set; } = new HashSet<AlbumPicture>();

        public override string ToString()
        {
            return $"---{this.Title} - {this.Path}";
        }
    }
}
