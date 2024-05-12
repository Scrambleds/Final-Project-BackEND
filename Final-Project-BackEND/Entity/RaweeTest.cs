using System.ComponentModel.DataAnnotations;

namespace Final_Project_BackEND.Entity
{
    public class RaweeTest
    {
        [Key]
        public int rawee_id { get; set; }
        public string rawee_name { get; set; }

    }
}