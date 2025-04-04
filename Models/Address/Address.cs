using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_WEBNC.Models
{
    [Table("AddressUser")]
    public class Address
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }
        
        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
        
        [Required(ErrorMessage = "Địa chỉ cụ thể là bắt buộc")]
        [Display(Name = "Địa chỉ cụ thể")]
        public string StreetAddress { get; set; }
        
        public bool IsDefault { get; set; }
        
        [Required]
        public string user_Id { get; set; }
        // Thiết lập quan hệ với User
        [ForeignKey("user_Id")]
        public virtual User User { get; set;} 
    }
}