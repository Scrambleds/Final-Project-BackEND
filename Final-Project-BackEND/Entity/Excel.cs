using System.ComponentModel.DataAnnotations;

namespace Final_Project_BackEND.Entity
{
    public class Excel
    {
        [Key] 
        public int excelId { get; set; }
        public string importHeaderNumber { get; set; } = string.Empty;
        public string no { get; set; } = string.Empty;
        public string id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string grade { get; set; } = string.Empty;
        public int createByUserId { get; set; }   
        public DateTime dateCreated { get; set; } = DateTime.Now;
        public DateTime? dateUpdated { get; set; } = null;
        
    }
    public class StudentExcel
    {
        public string? No { get; set; }
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Grade { get; set; }
    }

    public class StudentDetailExcel
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public string? courseID { get; set; }
        public string? courseName { get; set; }
        public string? semester { get; set; }
        public string? yearEducation { get; set; }
        public string? grade { get; set; }
    }
}
