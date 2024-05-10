namespace Final_Project_BackEND.Entity
{
    public class ImportHeader
    {
        public int importHeaderID {  get; set; }
        public string importHeaderNumber { get; set; } = string.Empty;
        public string courseID { get; set; } = string.Empty;
        public string courseName {  get; set; } = string.Empty;
        public string semester {  get; set; } = string.Empty;
        public string yearEducation { get; set; } = string.Empty;
        public int createByUserId { get; set; }
        public DateTime dateCreated { get; set; } = DateTime.Now;
        public DateTime? dateUpdated { get; set; } = null;
        public int isenable { get; set; } = 1;
    }
}
