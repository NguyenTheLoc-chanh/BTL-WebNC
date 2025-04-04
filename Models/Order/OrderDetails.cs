using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_WEBNC.Models
{
    [Table("OrderDetails")]
    public class OrderDetails
    {
        [Key]
        public int OrderItem_Id { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "float")]
        public float Price { get; set; }

        // Khóa ngoại đến bảng Orders
        [Required]
        public int order_Id { get; set; }

        [ForeignKey("order_Id")]
        public virtual Orders Order { get; set; }

        // Khóa ngoại đến bảng Products
        [Required]
        public int Product_Id { get; set; }

        [ForeignKey("Product_Id")]
        public virtual Product Product { get; set; }
    }
}
