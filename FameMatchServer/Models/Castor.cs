using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FameMatchServer.Models;

[Table("Castor")]
public partial class Castor
{
    [Key]
    public int UserId { get; set; }

    [StringLength(400)]
    public string CompanyName { get; set; } = null!;

    public int NumOfLisence { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Audition> Auditions { get; set; } = new List<Audition>();

    [ForeignKey("UserId")]
    [InverseProperty("Castor")]
    public virtual User User { get; set; } = null!;
}
