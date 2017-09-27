namespace Social_Network.Models
{
    public class SharedAlbum
    {
        public int UserId { get; set; }
        
        public int AlbumId { get; set; }

        public User User { get; set; }

        public Album Album { get; set; }
    }
}
