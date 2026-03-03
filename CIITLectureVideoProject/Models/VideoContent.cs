using System.ComponentModel.DataAnnotations;

namespace CIITLectureVideoProject.Models
{
    public class VideoContent
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public byte[] Data { get; set; } // Stored as VARBINARY(MAX)
        public string ContentType { get; set; } // e.g., "video/mp4"
    }
}
