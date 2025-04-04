using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_WEBNC.Models
{
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
        public string? Description { get; set; }
    }
}