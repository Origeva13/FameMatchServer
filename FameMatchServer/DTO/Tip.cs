using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FameMatchServer.DTO
{
    public class Tip
    {
       
        public int TipId { get; set; }

        public int? UserId { get; set; }

        public int TipLevel { get; set; }

       
        public string Question { get; set; } = null!;

        public string Answer1 { get; set; } = null!;

     
        public string Answer2 { get; set; } = null!;

        
        public string Answer3 { get; set; } = null!;

        public string Answer4 { get; set; } = null!;

        public Tip() { }
        public Tip(Models.Tip T)
        {
            this.TipId = T.TipId;
            this.UserId = T.UserId;
            this.TipLevel = T.TipLevel;
            this.Question = T.Question;
            this.Answer1 = T.Answer1;
            this.Answer2 = T.Answer2;
            this.Answer3 = T.Answer3;
            this.Answer4 = T.Answer4;
        }
        public Models.Tip GetModel()
        {
            Models.Tip T = new Models.Tip();
            T.TipId = this.TipId;
            T.UserId = this.UserId;
            T.TipLevel = this.TipLevel;
            T.Question = this.Question;
            T.Answer1 = this.Answer1;
            T.Answer2 = this.Answer2;
            T.Answer3 = this.Answer3;
            T.Answer4 = this.Answer4;
            return T;
        }
        //public virtual Casted? User { get; set; }
    }
}
