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
        public Message() { }
        
        public Message(Models.Message M)
        {
            this.SenderId = M.SenderId;
            this.ReciverId = M.ReciverId;
            this.MessageId = M.MessageId;
            this.Content = M.Content;
            this.MessageTime = M.MessageTime;
        }
        public Models.Message GetModel()
        {
            Models.Message M = new Models.Message();
            M.SenderId = this.SenderId;
            M.ReciverId= this.ReciverId;
            M.MessageId = this.MessageId;
            M.Content = this.Content;
            M.MessageTime = this.MessageTime;
            return M;
        }

        //public virtual User? Reciver { get; set; }

        //public virtual User? Sender { get; set; }
    }
}
