using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExamModels;

public partial class TblstudentProfile
{
    [Key]
    public int StudentId { get; set; }

    public string? UserId { get; set; }

    public string? FullName { get; set; }

    public string PrnNumber { get; set; } = null!;

    public string? Gender { get; set; }

    public string EmailAddress { get; set; } = null!;

    public string? Password { get; set; }

    public string? ProfilePhoto { get; set; }
}
