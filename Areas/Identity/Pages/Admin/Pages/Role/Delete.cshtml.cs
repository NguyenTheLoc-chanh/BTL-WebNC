using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BTL_WEBNC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace BTL_WEBNC.Admin.Roles
{
    [Authorize(Roles ="Admin")]
    public class DeleteModel : RolePageModel
    {
        public DeleteModel(RoleManager<IdentityRole> roleManager, AppDBContext appDBContext) : base(roleManager, appDBContext)
        {
        }

        public IdentityRole role { get; set; }

        public async Task<IActionResult> OnGet(string roleid)
        {
            if(roleid == null) return NotFound("Không tìm thấy role");
            role = await _roleManager.FindByIdAsync(roleid);
            if(role == null)
            {
                return NotFound("Không tìm thấy role");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string roleid)
        {
            if(roleid == null) return NotFound("Không tìm thấy role");
            role = await _roleManager.FindByIdAsync(roleid);
            if(role == null) return NotFound("Không tìm thấy Role");


            var result = await _roleManager.DeleteAsync(role);

            if(result.Succeeded)
            {
                StatusMessage = $"Bạn vừa xóa role: {role.Name}";
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
