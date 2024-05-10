using System.ComponentModel.DataAnnotations;

namespace Final_Project_BackEND.Entity
{
    public class GradeImportHeader
    {
        [Key]
        public int gradeId { get; set; }
        public string importHeaderNumber { get; set; } = string.Empty;
        public int? A { get; set; } 
        public int? Bplus { get; set; } 
        public int? B { get; set; }
        public int? Cplus { get; set; } 
        public int? C { get; set; }
        public int? Dplus { get; set; }
        public int? D { get; set; }
        public int? F { get; set; }
        public int? I { get; set; }
        public int? W { get; set; }
        public int? Total { get; set; }
        public int createByUserId { get; set; }
        public DateTime dateCreated { get; set; } = DateTime.Now;
        public DateTime? dateUpdated { get; set; } = null;
    }

    public class GradeSumStudent
    {
        public string? id { get; set; }
        public string? name { get; set; } 
        public string? a { get; set; }
        public string? bPlus { get; set; }
        public string? b { get; set; }
        public string? cPlus { get; set; }
        public string? c { get; set; }
        public string? dPlus { get; set; }
        public string? d { get; set; }
        public string? f { get; set; }
        public string? i { get; set; }
        public string? w { get; set; }
        public string? total { get; set; }
    }
    public class CourseInformation
    {
        public string importHeaderNumber { get; set; } =string.Empty;
        public string courseID { get; set; } = string.Empty;
        public string courseName { get; set; } = string.Empty;
        public string semester { get; set; } = string.Empty;
        public int total { get; set; }
        public int? a { get; set; }
        public int? bplus { get; set; }
        public int? b { get; set; }
        public int? cplus { get; set; }
        public int? c { get; set; }  
        public int? dplus { get; set; }
        public int? d { get; set; }
        public int? f { get; set; }
        public int? i { get; set; }
        public int? w { get; set; }
        public DateTime dateCreated { get; set; }
    }

}

