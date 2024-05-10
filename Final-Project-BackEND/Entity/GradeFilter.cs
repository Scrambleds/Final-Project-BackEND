using System.ComponentModel.DataAnnotations;

namespace Final_Project_BackEND.Entity
{
    public class GradeFilter
    {
        [Key]
        public int No { get; set; }

        public string? Grade { get; set; }

    }

    public class ModelGrade
    {
        public string? GradeString { get; set; }
        public decimal? Percentage { get; set; }
    }
}
