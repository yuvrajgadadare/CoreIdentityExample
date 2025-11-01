using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExamModels;

public partial class Tbltopic
{
    [Key]
    public int TopicId { get; set; }

    public string TopicName { get; set; } = null!;

    public virtual ICollection<TblstudentExam> TblstudentExams { get; set; } = new List<TblstudentExam>();

    public virtual ICollection<TbltopicContent> TbltopicContents { get; set; } = new List<TbltopicContent>();
}
