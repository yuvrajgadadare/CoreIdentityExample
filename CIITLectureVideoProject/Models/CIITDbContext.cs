using Microsoft.EntityFrameworkCore;

namespace CIITLectureVideoProject.Models
{
    public class CIITDbContext:DbContext
    {
        public CIITDbContext(DbContextOptions<CIITDbContext> options) : base(options) { }
        public DbSet<VideoContent> TblVideoContents {  get; set; }
    }
}
