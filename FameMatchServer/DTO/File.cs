using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FameMatchServer.DTO
{
    public class File
    {
       
        public int FileId { get; set; }

        
        public string FileExt { get; set; } = null!;
        public File() { }
        public File(Models.File F)
        {
            this.FileId = F.FileId;
            this.FileExt = F.FileExt;
        }

        
        //public virtual ICollection<Picture> Pictures { get; set; } = new List<Picture>();
    }
}
