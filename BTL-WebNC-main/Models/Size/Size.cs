using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_WEBNC.Models
{
    [Table("Sizes")]
    public class Size
    {
        [Key]
        public int Size_Id { get; set; }

        [Required(ErrorMessage = "Tên kích thước không được để trống!")]
        [Column(TypeName = "nvarchar")]
        [StringLength(50)]
        public string Name { get; set; }

        // Quan hệ nhiều-nhiều với Product (thông qua bảng ProductSize)
        public virtual ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
    }
}
