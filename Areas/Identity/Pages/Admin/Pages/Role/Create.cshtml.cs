using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BTL_WEBNC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BTL_WEBNC.Admin.Roles
{
    public class CreateModel : RolePageModel
    {
        public CreateModel(RoleManager<IdentityRole> roleManager, AppDBContext appDBContext) : base(roleManager, appDBContext)
        {
        }

        public class InputModel
        {
            [Display(Name="Tên của Role")]
            [Required (ErrorMessage = "Phải nhập {0}")]
            [StringLength(256, MinimumLength = 3, ErrorMessage = "Tên của {0} phải dài {2} đến {1} kí tự.")]
            public string Name { get; set; }
        }

        [BindProperty]
        public InputModel Input { set; get; }
        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            var newRole = new IdentityRole(Input.Name);
            var result = await _roleManager.CreateAsync(newRole);
            if(result.Succeeded)
            {
                StatusMessage = $"Bạn vừa tạo role mới: {Input.Name}";
                return RedirectToPage("./Index");
            }
            else{
                result.Errors.ToList().ForEach(error => {
                    ModelState.AddModelError(string.Empty, error.Description);
                });
            }
            return Page();
        }
    }
}
