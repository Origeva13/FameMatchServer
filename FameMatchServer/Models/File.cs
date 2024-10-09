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

    [StringLength(50)]
    public string FileExt { get; set; } = null!;

    [InverseProperty("File")]
    public virtual ICollection<Picture> Pictures { get; set; } = new List<Picture>();
}
