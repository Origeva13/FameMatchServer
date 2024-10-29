using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FameMatchServer.DTO
{
    public class Casted:User
    {
      

        public int UserAge { get; set; }

       
        public string UserLocation { get; set; } = null!;

        public int UserHigth { get; set; }

   
        public string UserHair { get; set; } = null!;

       
        public string UserEyes { get; set; } = null!;

    
        public string UserBody { get; set; } = null!;

        
        public string UserSkin { get; set; } = null!;

       
        public string AboutMe { get; set; } = null!;
        public Casted() { }
        public Casted(Models.Casted C):base(C.User)
        {
           
            this.UserAge = C.UserAge;
            this.UserLocation = C.UserLocation;
            this.UserHigth = C.UserHigth;
            this.UserHair = C.UserHair;
            this.UserEyes = C.UserEyes;
            this.UserBody = C.UserBody;
            this.UserSkin = C.UserSkin;
            this.AboutMe = C.AboutMe;
        }
        public Models.Casted GetModel()
        {
            var User = this.GetModel();
            var casted = new Models.Casted 
            { 
                UserAge = this.UserAge,
                UserLocation = this.UserLocation,
                UserHigth = this.UserHigth,
                UserHair = this.UserHair,
                UserEyes = this.UserEyes,
                UserBody = this.UserBody,
                UserSkin=this.UserSkin,
                AboutMe = this.AboutMe
                

            };
            casted.User.UserId = this.UserId;
            casted.User.UserName = this.UserName;
            casted.User.UserLastName = this.UserLastName;
            casted.User.UserEmail = this.UserEmail;
            casted.User.UserPassword = this.UserPassword;
            casted.User.IsManager = this.IsManager;
            casted.User.UserGender = this.UserGender;
            casted.User.IsReported = this.IsReported;
            casted.User.IsBlocked = this.IsBlocked;

            return casted;

        }
        //public virtual ICollection<Tip> Tips { get; set; } = new List<Tip>();


        //public virtual User User { get; set; } = null!; לשאול את עופר
    }
}
