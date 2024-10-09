using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FameMatchServer.DTO
{
    public class Reporet
    {
        public int? UserId { get; set; }

        public int ReporetId { get; set; }

        public int? ReportedId { get; set; }
      
        public string Content { get; set; } = null!;

        
        //public virtual User? Reported { get; set; }

       
        //public virtual User? User { get; set; }
    }
}
