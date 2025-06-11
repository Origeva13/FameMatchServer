using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FameMatchServer.Models;

public partial class File
{
    [Key]
    public int FileId { get; set; }

    public int UserId { get; set; }

    [StringLength(50)]
    public string FileExt { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Files")]
    public virtual User User { get; set; } = null!;
}
