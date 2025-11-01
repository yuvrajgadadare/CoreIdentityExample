using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExamModels;

public partial class TblstudentExam
{
    [Key]
    public int ExamId { get; set; }

    public string? UserId { get; set; }

    public int? TopicId { get; set; }

    public DateTime? StartExamDate { get; set; }

    public DateTime? EndExamDate { get; set; }

    public virtual ICollection<TblexamQuestion> TblexamQuestions { get; set; } = new List<TblexamQuestion>();

    public virtual Tbltopic? Topic { get; set; }
}
