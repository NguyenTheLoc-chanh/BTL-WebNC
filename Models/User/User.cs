using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_WEBNC.Models
{
    public enum UserRole
    {
        Customer,
        Seller,
        Admin
    }
    [Table("Users")]
    public class User
    {
        [Key]
        public int user_Id { get; set; }

        [Required(ErrorMessage ="Tên không được để trống!")]
        [StringLength(255)]
        public required string FullName { get; set; }

        [Required(ErrorMessage ="Email không được để trống!")]
        [EmailAddress]
        [StringLength(255)]
        public required string Email { get; set; }

        [Required]
        [StringLength(255)]
        public required string Password { get; set; }

        [Phone(ErrorMessage ="Số điện thoại không được để trống!")]
        [StringLength(20)]
        public string? Phone { get; set; }

        public string? Address { get; set; }

        [Required(ErrorMessage ="Vui lòng chọn role!")]
        public UserRole Role { get; set; } = UserRole.Customer;
    }
}
