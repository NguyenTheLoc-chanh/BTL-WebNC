using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_WEBNC.Models
{
    // Ánh xạ class này với bảng Categories trong database.
    [Table("Categories")]
    public class Category
    {
        [Key]
        public int category_id { get; set; }

        [Required(ErrorMessage ="Tên danh mục không được để trống!")]
        [Column(TypeName ="nvarchar")]
        [StringLength(255)]
        public string category_Name { get; set; }

        [Column(TypeName ="nvarchar")]
        [StringLength(500)] 

        // Mô tả không dc để trống
        public string? Description { get; set; }
    }
}