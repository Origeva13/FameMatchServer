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

        public virtual Casted? Casted { get; set; }

        public virtual Castor? Castor { get; set; }

        //public virtual ICollection<Message> MessageRecivers { get; set; } = new List<Message>();

        
        //public virtual ICollection<Message> MessageSenders { get; set; } = new List<Message>();

        public virtual Picture? Picture { get; set; }

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
            this.UserTasks = new List<UserTask>();
            foreach (var task in modelUser.UserTasks)
            {
                this.UserTasks.Add(new UserTask(task));
            }
        }
    }
}
