using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FameMatchServer.Models;

[Table("Casted")]
public partial class Casted
{
    [Key]
    public int UserId { get; set; }

    public int UserAge { get; set; }

    [StringLength(400)]
    public string UserLocation { get; set; } = null!;

    public int UserHigth { get; set; }

    [StringLength(50)]
    public string UserHair { get; set; } = null!;

    [StringLength(50)]
    public string UserEyes { get; set; } = null!;

    [StringLength(50)]
    public string UserBody { get; set; } = null!;

    [StringLength(50)]
    public string UserSkin { get; set; } = null!;

    [StringLength(800)]
    public string AboutMe { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<Tip> Tips { get; set; } = new List<Tip>();

    [ForeignKey("UserId")]
    [InverseProperty("Casted")]
    public virtual User User { get; set; } = null!;
}
