using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FameMatchServer.Models;

[Table("Message")]
public partial class Message
{
    public int? SenderId { get; set; }

    public int? ReciverId { get; set; }

    [Key]
    public int MessageId { get; set; }

    [StringLength(800)]
    public string Content { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime MessageTime { get; set; }

    [ForeignKey("ReciverId")]
    [InverseProperty("MessageRecivers")]
    public virtual User? Reciver { get; set; }

    [ForeignKey("SenderId")]
    [InverseProperty("MessageSenders")]
    public virtual User? Sender { get; set; }
}
