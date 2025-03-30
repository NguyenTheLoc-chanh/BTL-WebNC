using BTL_WEBNC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BTL_WEBNC.Admin.Roles
{
    // [Authorize(Roles ="Admin")]
    public class IndexModel : RolePageModel
    {
        public IndexModel(RoleManager<IdentityRole> roleManager, AppDBContext appDBContext) : base(roleManager, appDBContext)
        {
        }

        public List<IdentityRole> roles { set; get;}

        public async Task OnGet()
        {
            roles = await _roleManager.Roles.OrderBy(r => r.Name).ToListAsync();
        }

        public void OnPost() => RedirectToPage();
    }
}