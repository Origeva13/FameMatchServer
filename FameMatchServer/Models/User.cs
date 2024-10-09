using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FameMatchServer.Models;

[Index("UserEmail", Name = "UQ__Users__08638DF8C72EA190", IsUnique = true)]
public partial class User
{
    [Key]
    public int UserId { get; set; }

    [StringLength(50)]
    public string UserName { get; set; } = null!;

    [StringLength(50)]
    public string UserLastName { get; set; } = null!;

    [StringLength(50)]
    public string UserEmail { get; set; } = null!;

    [StringLength(50)]
    public string UserPassword { get; set; } = null!;

    public bool IsManager { get; set; }

    [StringLength(50)]
    public string UserGender { get; set; } = null!;

    public bool IsReported { get; set; }

    public bool IsBlocked { get; set; }

    [InverseProperty("User")]
    public virtual Casted? Casted { get; set; }

    [InverseProperty("User")]
    public virtual Castor? Castor { get; set; }

    [InverseProperty("Reciver")]
    public virtual ICollection<Message> MessageRecivers { get; set; } = new List<Message>();

    [InverseProperty("Sender")]
    public virtual ICollection<Message> MessageSenders { get; set; } = new List<Message>();

    [InverseProperty("User")]
    public virtual Picture? Picture { get; set; }

    [InverseProperty("Reported")]
    public virtual ICollection<Reporet> ReporetReporteds { get; set; } = new List<Reporet>();

    [InverseProperty("User")]
    public virtual ICollection<Reporet> ReporetUsers { get; set; } = new List<Reporet>();
}
