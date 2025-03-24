using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BTL_WEBNC.Models
{
    public enum UserRole
    {
        Customer,
        Seller,
        Admin
    }
    [Table("Users")]
    public class User : IdentityUser
    {
        public string? Address { get; set; }

        [Required(ErrorMessage ="Vui lòng chọn role!")]
        public UserRole Role { get; set; } = UserRole.Customer;
    }
}
