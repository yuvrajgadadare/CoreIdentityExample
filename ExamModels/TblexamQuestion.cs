using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExamModels;

public partial class TblexamQuestion
{
    [Key]
    public int ExamQuestionId { get; set; }

    public int? ExamId { get; set; }

    public int? QuestionId { get; set; }

    public int? SubmittedOptionNumber { get; set; }

    public virtual TblstudentExam? Exam { get; set; }

    public virtual TblcontentQuestion? Question { get; set; }
}
