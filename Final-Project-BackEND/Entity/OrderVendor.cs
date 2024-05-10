using System.ComponentModel.DataAnnotations;

namespace Final_Project_BackEND.Entity
{
    public class OrderVendor
    {
        [Key]
        public int order_id { get; set; }
        public string vendor_name { get; set; } = string.Empty;
    }
}
