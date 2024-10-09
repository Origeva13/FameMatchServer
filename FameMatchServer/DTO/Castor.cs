using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FameMatchServer.DTO
{
    public class Castor
    {
        public int UserId { get; set; }

       
        public string CompanyName { get; set; } = null!;

        public int NumOfLisence { get; set; }

        
        public virtual ICollection<Audition> Auditions { get; set; } = new List<Audition>();

        
        public virtual User User { get; set; } = null!;
    }
}
