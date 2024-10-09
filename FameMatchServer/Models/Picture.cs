using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FameMatchServer.Models;

public partial class Picture
{
    [Key]
    public int UserId { get; set; }

    public int? FileId { get; set; }

    [ForeignKey("FileId")]
    [InverseProperty("Pictures")]
    public virtual File? File { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Picture")]
    public virtual User User { get; set; } = null!;
}
