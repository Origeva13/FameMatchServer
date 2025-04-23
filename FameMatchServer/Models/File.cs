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

    [ForeignKey("FileId")]
    [InverseProperty("Files")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
