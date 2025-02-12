using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FameMatchServer.DTO
{
    public class Audition
    {
        public int? UserId { get; set; }

       
        public int AudId { get; set; }

        
        public string Description { get; set; } = null!;

        public int AudAge { get; set; }

       
        public string? AudLocation { get; set; }

        public int? AudHigth { get; set; }

        
        public string? AudHair { get; set; }

      
        public string? AudEyes { get; set; }

       
        public string? UserBody { get; set; }

      
        public string? AudSkin { get; set; }

        public string? AudName { get; set; }

        public string? AudGender {  get; set; }

        public bool IsPublic { get; set; }

        public Audition() { }
        public Audition(Models.Audition A)
        {
            this.UserId = A.UserId;
            this.AudId = A.AudId;
            this.Description = A.Description;
            this.AudAge = A.AudAge;
            this.AudLocation = A.AudLocation;
            this.AudHigth = A.AudHigth;
            this.AudHair= A.AudHair;
            this.AudEyes = A.AudEyes;
            this.UserBody = A.UserBody;
            this.AudSkin = A.AudSkin;
            this.AudName = A.AudName;
            this.IsPublic = A.IsPublic;
            this.AudGender = A.AudGender;
        }
        public Models.Audition GetModel()
        {
            Models.Audition A = new Models.Audition();
            A.UserId = this.UserId;
            A.AudId = this.AudId;
            A.Description = this.Description;
            A.AudAge = this.AudAge;
            A.AudLocation = this.AudLocation;
            A.AudHigth= this.AudHigth;
            A.AudHair = this.AudHair;
            A.AudEyes= this.AudEyes;
            A.UserBody = this.UserBody;
            A.AudSkin = this.AudSkin;
            A.AudName = this.AudName;
            A.IsPublic = this.IsPublic;
            A.AudGender = this.AudGender;
            return A;
        }
        //public virtual Castor? User { get; set; }
    }
}

