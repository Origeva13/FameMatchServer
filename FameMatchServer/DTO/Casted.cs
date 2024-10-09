using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FameMatchServer.DTO
{
    public class Casted
    {
      
        public int UserId { get; set; }

        public int UserAge { get; set; }

       
        public string UserLocation { get; set; } = null!;

        public int UserHigth { get; set; }

   
        public string UserHair { get; set; } = null!;

       
        public string UserEyes { get; set; } = null!;

    
        public string UserBody { get; set; } = null!;

        
        public string UserSkin { get; set; } = null!;

       
        public string AboutMe { get; set; } = null!;

        
        //public virtual ICollection<Tip> Tips { get; set; } = new List<Tip>();

        
        //public virtual User User { get; set; } = null!; לשאול את עופר
    }
}
