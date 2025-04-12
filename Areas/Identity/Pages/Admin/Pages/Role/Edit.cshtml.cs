using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BTL_WEBNC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BTL_WEBNC.Admin.Roles
{
    [Authorize(Roles ="Admin")]
    public class EditModel : RolePageModel
    {
        public EditModel(RoleManager<IdentityRole> roleManager, AppDBContext appDBContext) : base(roleManager, appDBContext)
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
        public IdentityRole role { get; set; }

        public async Task<IActionResult> OnGet(string roleid)
        {
            if(roleid == null) return NotFound("Không tìm thấy role");
            role = await _roleManager.FindByIdAsync(roleid);
            if(role != null)
            {
                Input = new InputModel()
                {
                    Name = role.Name,
                };
                return Page();
            }
            return NotFound("Không tìm thấy role");
        }
        public async Task<IActionResult> OnPostAsync(string roleid)
        {
            if(roleid == null) return NotFound("Không tìm thấy role");
            role = await _roleManager.FindByIdAsync(roleid);
            if(role == null) return NotFound("Không tìm thấy Role");
            if(!ModelState.IsValid)
            {
                return Page();
            }

            role.Name = Input.Name;
            var result = await _roleManager.UpdateAsync(role);

            if(result.Succeeded)
            {
                StatusMessage = $"Bạn vừa cập nhật lại role: {Input.Name}";
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
