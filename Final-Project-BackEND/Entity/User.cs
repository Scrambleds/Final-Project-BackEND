namespace Final_Project_BackEND.Entity
{
    public class User
    {
        public int userId { get; set; }
        public string username { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string firstname { get; set; } = string.Empty;
        public string lastname { get; set; } = string.Empty;
        public int isAdmin { get; set; } = 0;
        public DateTime dateCreated { get; set; } = DateTime.Now;
        public DateTime? dateUpdated { get; set; } = null;
        public int isenable { get; set; } = 1;

    }
}
