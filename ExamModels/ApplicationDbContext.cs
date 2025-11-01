using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace ExamModels.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<TblcontentQuestion> TblcontentQuestions { get; set; }
        public virtual DbSet<TblexamQuestion> TblexamQuestions { get; set; }
        public virtual DbSet<TblstudentExam> TblstudentExams { get; set; }
        public virtual DbSet<TblstudentProfile> TblstudentProfiles { get; set; }
        public virtual DbSet<Tbltopic> Tbltopics { get; set; }
        public virtual DbSet<TbltopicContent> TbltopicContents { get; set; }
    }
}
