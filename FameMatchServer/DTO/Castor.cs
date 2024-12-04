using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FameMatchServer.Models;

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
        public Models.Castor GetModel()
        {
            var BaseUser = base.GetModel();
            var castor = new Models.Castor
            {
               CompanyName=this.CompanyName,
               NumOfLisence=this.NumOfLisence,
               User = BaseUser
            };
        
           

            return castor;

        }
        //public virtual ICollection<Audition> Auditions { get; set; } = new List<Audition>();


        //public virtual User User { get; set; } = null!; לשאול את עופר
    }
}
