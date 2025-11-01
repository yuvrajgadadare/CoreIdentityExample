using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExamModels;

public partial class TbltopicContent
{
    [Key]
    public int ContentId { get; set; }

    public string ContentName { get; set; } = null!;

    public int? TopicId { get; set; }

    public virtual ICollection<TblcontentQuestion> TblcontentQuestions { get; set; } = new List<TblcontentQuestion>();

    public virtual Tbltopic? Topic { get; set; }
}
