namespace Social_Network.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Validations;

    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Tag]
        public string TagName { get; set; }

        public ICollection<AlbumTag> Albums { get; set; } = new HashSet<AlbumTag>();
    }
}
