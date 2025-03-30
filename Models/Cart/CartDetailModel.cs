using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_WEBNC.Models
{
    public class CartDetails
    {
        [Key]
        public int CartDetailId { get; set; }

        [Required]
        public int CartId { get; set; }

        [Required]
        public int Product_Id { get; set; } 

        [Required]
        public int Size_Id { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
        public int Quantity { get; set; } 

        // Navigation properties
        [ForeignKey("CartId")]
        public virtual CartModel Cart { get; set; }

        [ForeignKey("Product_Id")]
        public virtual Product Product { get; set; }

        [ForeignKey("Size_Id")]
        public virtual Size Size { get; set; }
    }
}
