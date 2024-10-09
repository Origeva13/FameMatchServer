using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FameMatchServer.Models;

public partial class Audition
{
    public int? UserId { get; set; }

    [Key]
    public int AudId { get; set; }

    [StringLength(800)]
    public string Description { get; set; } = null!;

    public int AudAge { get; set; }

    [StringLength(400)]
    public string? AudLocation { get; set; }

    public int? AudHigth { get; set; }

    [StringLength(50)]
    public string? AudHair { get; set; }

    [StringLength(50)]
    public string? AudEyes { get; set; }

    [StringLength(50)]
    public string? UserBody { get; set; }

    [StringLength(50)]
    public string? AudSkin { get; set; }

    public bool IsPublic { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Auditions")]
    public virtual Castor? User { get; set; }
}
