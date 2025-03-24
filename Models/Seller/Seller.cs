using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_WEBNC.Models
{
    public enum SellerStatus
    {
        Active,
        Inactive,
        Banned
    }

    [Table("Sellers")]
    public class Seller
    {
        [Key]
        public int seller_Id { get; set; }

        [Required]
        public string user_Id { get; set; }

        [Required(ErrorMessage ="Tên cửa hàng không được để trống!")]
        [StringLength(255)]
        public string StoreName { get; set; }

        [StringLength(255)]
        public string? StoreLogo { get; set; }

        [Required]
        public SellerStatus Status { get; set; } = SellerStatus.Active;

        // Thiết lập quan hệ với User
        [ForeignKey("user_Id")]
        public virtual User User { get; set; }
    }
}
