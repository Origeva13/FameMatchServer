using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FameMatchServer.DTO
{
    public class User
    {
        public int UserId { get; set; }


        public string UserName { get; set; } = null!;

       
        public string UserLastName { get; set; } = null!;

        public string UserEmail { get; set; } = null!;

        
        public string UserPassword { get; set; } = null!;

        public bool IsManager { get; set; }

      
        public string UserGender { get; set; } = null!;

        public bool IsReported { get; set; }

        public bool IsBlocked { get; set; }

        //public virtual Casted? Casted { get; set; }

        //public virtual Castor? Castor { get; set; }

        //public virtual ICollection<Message> MessageRecivers { get; set; } = new List<Message>();


        //public virtual ICollection<Message> MessageSenders { get; set; } = new List<Message>();

        public virtual ICollection<File> Files { get; set; } = new List<File>();

        //public virtual ICollection<Reporet> ReporetReporteds { get; set; } = new List<Reporet>();

        //public virtual ICollection<Reporet> ReporetUsers { get; set; } = new List<Reporet>();
        public User() { }
        public User(Models.User modelUser)
        {
            this.UserId = modelUser.UserId;
            this.UserName = modelUser.UserName;
            this.UserLastName = modelUser.UserLastName;
            this.UserEmail = modelUser.UserEmail;
            this.UserPassword = modelUser.UserPassword;
            this.IsManager = modelUser.IsManager;
            this.UserGender= modelUser.UserGender;
            this.IsReported= modelUser.IsReported;
            this.IsBlocked = modelUser.IsBlocked;
            if (modelUser.Files != null)
            {
                foreach (var file in modelUser.Files)
                {
                    this.Files.Add(new File(file));
                }
            }

        }
        public Models.User GetModel()
        {
            Models.User U= new Models.User();
            U.UserId = this.UserId;
            U.UserName = this.UserName;
            U.UserLastName = this.UserLastName;
            U.UserEmail = this.UserEmail;
            U.UserPassword = this.UserPassword;
            U.IsManager = this.IsManager;
            U.UserGender = this.UserGender;
            U.IsReported = this.IsReported;
            U.IsBlocked = this.IsBlocked;
            return U;
        }
    }
}
