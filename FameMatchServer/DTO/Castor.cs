using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FameMatchServer.DTO
{
    public class Castor:User
    {

       
        public string CompanyName { get; set; } = null!;

        public int NumOfLisence { get; set; }

        public Castor() { }
        public Castor(Models.Castor CA) : base(CA.User)
        {

            this.CompanyName = CA.CompanyName;
            this.NumOfLisence =CA.NumOfLisence;
        }
        //public virtual ICollection<Audition> Auditions { get; set; } = new List<Audition>();


        //public virtual User User { get; set; } = null!; לשאול את עופר
    }
}
