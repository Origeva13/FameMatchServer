using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FameMatchServer.Models;

[Table("Reporet")]
public partial class Reporet
{
    public int? UserId { get; set; }

    [Key]
    public int ReporetId { get; set; }

    public int? ReportedId { get; set; }

    [StringLength(800)]
    public string Content { get; set; } = null!;

    [ForeignKey("ReportedId")]
    [InverseProperty("ReporetReporteds")]
    public virtual User? Reported { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("ReporetUsers")]
    public virtual User? User { get; set; }
}
