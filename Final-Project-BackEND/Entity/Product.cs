using System.ComponentModel.DataAnnotations;

namespace Final_Project_BackEND.Entity
{   public class Product
    {
        [Key]
        public int productId { get; set; }
        public string productName { get; set; } = string.Empty;
    }
}