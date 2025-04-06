using System.ComponentModel.DataAnnotations;

namespace BTL_WEBNC.Areas.Admin.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Vai trò")]
        public string Role { get; set; }
    }
}
