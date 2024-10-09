using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FameMatchServer.DTO
{
    public class Message
    {
        public int? SenderId { get; set; }

        public int? ReciverId { get; set; }

        public int MessageId { get; set; }

        public string Content { get; set; } = null!;

        public DateTime MessageTime { get; set; }

        public virtual User? Reciver { get; set; }

        public virtual User? Sender { get; set; }
    }
}
