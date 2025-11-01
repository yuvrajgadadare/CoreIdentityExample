using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExamModels;

public partial class TblcontentQuestion
{
    [Key]
    public int QuestionId { get; set; }

    public int? ContentId { get; set; }

    public string Question { get; set; } = null!;

    public string Option1 { get; set; } = null!;

    public string Option2 { get; set; } = null!;

    public string Option3 { get; set; } = null!;

    public string Option4 { get; set; } = null!;

    public int? CorrectOptionNumber { get; set; }

    public virtual TbltopicContent? Content { get; set; }

    public virtual ICollection<TblexamQuestion> TblexamQuestions { get; set; } = new List<TblexamQuestion>();
}
