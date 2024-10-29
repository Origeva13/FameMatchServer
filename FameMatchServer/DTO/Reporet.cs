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
        public Reporet() { }
        public Reporet(Models.Reporet R)
        {
            this.UserId = R.UserId;
            this.ReporetId = R.ReporetId;
            this.Content = R.Content;
        }
        public Models.Reporet GetModel()
        {
            Models.Reporet R = new Models.Reporet();
            R.UserId = this.UserId;
            R.ReporetId= this.ReporetId;
            R.ReportedId = this.ReportedId;
            R.Content = this.Content;
            return R;
        }

        //public virtual User? Reported { get; set; }


        //public virtual User? User { get; set; }
    }
}
