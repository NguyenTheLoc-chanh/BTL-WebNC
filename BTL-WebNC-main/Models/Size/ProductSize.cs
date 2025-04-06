using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_WEBNC.Models
{
    [Table("ProductSizes")]
    public class ProductSize
    {
        [Key]
        public int ProductSize_Id { get; set; }

        [Required]
        public int Product_Id { get; set; }  // Khóa ngoại liên kết với Product

        [Required]
        public int Size_Id { get; set; }  // Khóa ngoại liên kết với Size

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0!")]
        public int Stock { get; set; } = 0;  // Số lượng tồn kho cho sản phẩm-size  

        // Thiết lập quan hệ với Product
        [ForeignKey("Product_Id")]
        public virtual Product Product { get; set; }

        // Thiết lập quan hệ với Size
        [ForeignKey("Size_Id")]
        public virtual Size Size { get; set; }
    }
}
