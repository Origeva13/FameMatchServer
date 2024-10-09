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

        public bool IsPublic { get; set; }

        
        //public virtual Castor? User { get; set; }
    }
}

