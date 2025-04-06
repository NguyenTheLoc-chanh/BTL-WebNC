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
    public enum SellerApprovalStatus
    {
        Pending,   // Chờ duyệt
        Approved,  // Đã duyệt
        Rejected   // Bị từ chối
    }
    [Table("Users")]
    public class User : IdentityUser
    {
        public string? Address { get; set; }

        [Required(ErrorMessage ="Vui lòng chọn role!")]
        public UserRole Role { get; set; } = UserRole.Customer;
        public SellerApprovalStatus? SellerApprovalStatus { get; set; } = null; // Áp dụng khi làm người bán
        public string? StoreName { get; set; } // Tên cửa hàng
        public string? StoreDescription { get; set; } // Mô tả cửa hàng
    }

}
