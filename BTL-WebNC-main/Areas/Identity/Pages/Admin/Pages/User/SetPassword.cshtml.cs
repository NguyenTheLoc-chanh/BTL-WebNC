#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BTL_WEBNC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BTL_WEBNC.Admin.Users
{
    public class SetPasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public SetPasswordModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        public class InputModel
        {
            [Required(ErrorMessage ="Phải nhập {0}")]
            [StringLength(100, ErrorMessage = "{0} phải dài {2} đến {1} kí tự.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Mật khẩu mới")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Xác nhận mật khẩu")]
            [Compare("NewPassword", ErrorMessage = "Nhập lại mật khẩu không chính xác.")]
            public string ConfirmPassword { get; set; }
        }

        public User user { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if(string.IsNullOrEmpty(id)){
                return NotFound($"Không có user.");
            }
            user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Không thấy user với ID '{id}'.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if(string.IsNullOrEmpty(id)){
                return NotFound($"Không có user.");
            }
            user = await _userManager.FindByIdAsync(id);
            
            if (user == null)
            {
                return NotFound($"Không thấy user với ID '{id}'.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _userManager.RemovePasswordAsync(user);
            var addPasswordResult = await _userManager.AddPasswordAsync(user, Input.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                foreach (var error in addPasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = $"Bạn đã đặt thành công mật khẩu cho: {user.UserName}";

            return RedirectToPage("./Index");
        }
    }
}
