using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ExamModels;

public partial class CiitExamPortalContext : DbContext
{
    public CiitExamPortalContext()
    {
    }

    public CiitExamPortalContext(DbContextOptions<CiitExamPortalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblcontentQuestion> TblcontentQuestions { get; set; }

    public virtual DbSet<TblexamQuestion> TblexamQuestions { get; set; }

    public virtual DbSet<TblstudentExam> TblstudentExams { get; set; }

    public virtual DbSet<TblstudentProfile> TblstudentProfiles { get; set; }

    public virtual DbSet<Tbltopic> Tbltopics { get; set; }

    public virtual DbSet<TbltopicContent> TbltopicContents { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-VRMFV23\\SQLEXPRESS;Database=CIIT_ExamPortal;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblcontentQuestion>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__tblconte__2EC21549E7B5D9BB");

            entity.ToTable("tblcontent_questions");

            entity.Property(e => e.QuestionId).HasColumnName("question_id");
            entity.Property(e => e.ContentId).HasColumnName("content_id");
            entity.Property(e => e.CorrectOptionNumber).HasColumnName("correct_option_number");
            entity.Property(e => e.Option1)
                .IsUnicode(false)
                .HasColumnName("option1");
            entity.Property(e => e.Option2)
                .IsUnicode(false)
                .HasColumnName("option2");
            entity.Property(e => e.Option3)
                .IsUnicode(false)
                .HasColumnName("option3");
            entity.Property(e => e.Option4)
                .IsUnicode(false)
                .HasColumnName("option4");
            entity.Property(e => e.Question)
                .IsUnicode(false)
                .HasColumnName("question");

            entity.HasOne(d => d.Content).WithMany(p => p.TblcontentQuestions)
                .HasForeignKey(d => d.ContentId)
                .HasConstraintName("fkcontent");
        });

        modelBuilder.Entity<TblexamQuestion>(entity =>
        {
            entity.HasKey(e => e.ExamQuestionId).HasName("PK__tblexam___27F8BFF887E75B15");

            entity.ToTable("tblexam_questions");

            entity.Property(e => e.ExamQuestionId).HasColumnName("exam_question_id");
            entity.Property(e => e.ExamId).HasColumnName("exam_id");
            entity.Property(e => e.QuestionId).HasColumnName("question_id");
            entity.Property(e => e.SubmittedOptionNumber).HasColumnName("submitted_option_number");

            entity.HasOne(d => d.Exam).WithMany(p => p.TblexamQuestions)
                .HasForeignKey(d => d.ExamId)
                .HasConstraintName("fkexamid");

            entity.HasOne(d => d.Question).WithMany(p => p.TblexamQuestions)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("fkquestionid");
        });

        modelBuilder.Entity<TblstudentExam>(entity =>
        {
            entity.HasKey(e => e.ExamId).HasName("PK__tblstude__9C8C7BE94C6EDDDA");

            entity.ToTable("tblstudent_exams");

            entity.Property(e => e.ExamId).HasColumnName("exam_id");
            entity.Property(e => e.EndExamDate)
                .HasColumnType("datetime")
                .HasColumnName("end_exam_date");
            entity.Property(e => e.StartExamDate)
                .HasColumnType("datetime")
                .HasColumnName("start_exam_date");
            entity.Property(e => e.TopicId).HasColumnName("topic_id");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("user_id");

            entity.HasOne(d => d.Topic).WithMany(p => p.TblstudentExams)
                .HasForeignKey(d => d.TopicId)
                .HasConstraintName("fktopic");
        });

        modelBuilder.Entity<TblstudentProfile>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__tblstude__2A33069ADF7E563C");

            entity.ToTable("tblstudent_profiles");

            entity.HasIndex(e => e.EmailAddress, "UQ__tblstude__20C6DFF52D5A5180").IsUnique();

            entity.HasIndex(e => e.PrnNumber, "UQ__tblstude__6BCA32E9EE83BA39").IsUnique();

            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email_address");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("full_name");
            entity.Property(e => e.Gender)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("gender");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.PrnNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PRN_number");
            entity.Property(e => e.ProfilePhoto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("profile_photo");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("user_id");
        });

        modelBuilder.Entity<Tbltopic>(entity =>
        {
            entity.HasKey(e => e.TopicId).HasName("PK__tbltopic__D5DAA3E98EC63AB4");

            entity.ToTable("tbltopics");

            entity.HasIndex(e => e.TopicName, "UQ__tbltopic__54BAE5ECA77DC560").IsUnique();

            entity.Property(e => e.TopicId).HasColumnName("topic_id");
            entity.Property(e => e.TopicName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("topic_name");
        });

        modelBuilder.Entity<TbltopicContent>(entity =>
        {
            entity.HasKey(e => e.ContentId).HasName("PK__tbltopic__655FE510395666B8");

            entity.ToTable("tbltopic_contents");

            entity.Property(e => e.ContentId).HasColumnName("content_id");
            entity.Property(e => e.ContentName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("content_name");
            entity.Property(e => e.TopicId).HasColumnName("topic_id");

            entity.HasOne(d => d.Topic).WithMany(p => p.TbltopicContents)
                .HasForeignKey(d => d.TopicId)
                .HasConstraintName("fktopicid");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
