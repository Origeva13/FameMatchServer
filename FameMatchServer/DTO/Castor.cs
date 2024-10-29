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
        public Models.Castor GetModel()
        {
            var User = this.GetModel();
            var castor = new Models.Castor
            {
               CompanyName=this.CompanyName,
               NumOfLisence=this.NumOfLisence
            };
            castor.User.UserId = this.UserId;
            castor.User.UserName = this.UserName;
            castor.User.UserLastName = this.UserLastName;
            castor.User.UserEmail = this.UserEmail;
            castor.User.UserPassword = this.UserPassword;
            castor.User.IsManager = this.IsManager;
            castor.User.UserGender = this.UserGender;
            castor.User.IsReported = this.IsReported;
            castor.User.IsBlocked = this.IsBlocked;

            return castor;

        }
        //public virtual ICollection<Audition> Auditions { get; set; } = new List<Audition>();


        //public virtual User User { get; set; } = null!; לשאול את עופר
    }
}
