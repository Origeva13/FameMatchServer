using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FameMatchServer.Models;

[Table("Tip")]
public partial class Tip
{
    [Key]
    public int TipId { get; set; }

    public int? UserId { get; set; }

    public int TipLevel { get; set; }

    [StringLength(800)]
    public string Question { get; set; } = null!;

    [StringLength(800)]
    public string Answer1 { get; set; } = null!;

    [StringLength(800)]
    public string Answer2 { get; set; } = null!;

    [StringLength(800)]
    public string Answer3 { get; set; } = null!;

    [StringLength(800)]
    public string Answer4 { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Tips")]
    public virtual Casted? User { get; set; }
}
