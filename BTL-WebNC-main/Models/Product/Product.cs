using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_WEBNC.Models
{
    // quản lí trang thái sản 
    public enum ProductStatus
    {
        Active,
        OutOfStock,
        Disabled
    }

    [Table("Products")]
    public class Product
    {
        [Key]
        public int Product_Id { get; set; }

        [Required]
        public int seller_Id { get; set; }

        [Required]
        public int category_id { get; set; }

        [Required(ErrorMessage ="Tên sản phẩm không được để trống!") ]
        [Column(TypeName ="nvarchar")]
        [StringLength(255)]
        public string Name { get; set; }

        [Column(TypeName ="nvarchar(max)")]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "float")]
        public float Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Stock { get; set; } = 0;

        public string? Images { get; set; }

        [Required]
        public ProductStatus Status { get; set; } = ProductStatus.Active;

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Thiết lập quan hệ với Seller
        [ForeignKey("seller_Id")]
        public virtual Seller Seller { get; set; }

        // Thiết lập quan hệ với Category
        [ForeignKey("category_id")]
        public virtual Category Category { get; set; }
        public virtual ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
    }
}
