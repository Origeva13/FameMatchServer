using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FameMatchServer.DTO
{
    public class Picture
    {
        public int UserId { get; set; }

        public int? FileId { get; set; }
        public Picture() { }
        public Picture(Models.Picture P)
        {
            this.UserId = P.UserId;
            this.FileId = P.FileId;
        }
        public Models.Picture GetModel()
        {
            Models.Picture P = new Models.Picture();
            P.UserId = this.UserId;
            P.FileId = this.FileId;
            return P;
        }

        //public virtual File? File { get; set; }

        //public virtual User User { get; set; } = null!; לשאול את עופר
    }
}
