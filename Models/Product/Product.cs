using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_WEBNC.Models
{
    public enum ProductStatus
    {
        Active,
        OutOfStock,
        Disabled,
        NoActive
    }
    // Mới: Quản lý trạng thái phê duyệt của sản phẩm
    public enum ProductApprovalStatus
    {
        Pending,   // Chờ duyệt
        Approved,  // Đã duyệt
        Rejected   // Từ chối
    }

    [Table("Products")]
    public class Product
    {
        [Key]
        public int Product_Id { get; set; }

        [Required]
        public string seller_Id { get; set; }

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

        // Thuộc tính mới: Trạng thái phê duyệt của sản phẩm
        [Required]
        public ProductApprovalStatus? ApprovalStatus { get; set; } = ProductApprovalStatus.Pending;

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Thiết lập quan hệ với Seller
        [ForeignKey("seller_Id")]
        [Column(TypeName = "nvarchar(50)")]
        public virtual Seller Seller { get; set; }

        // Thiết lập quan hệ với Category
        [ForeignKey("category_id")]
        public virtual Category Category { get; set; }
        public virtual ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
    }
}
