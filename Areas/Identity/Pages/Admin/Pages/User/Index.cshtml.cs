using BTL_WEBNC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BTL_WEBNC.Admin.Users
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        public IndexModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        
        [TempData] 
        public string? StatusMessage { get; set; }

        public class UserAndRole : User
        {
            public string RoleNames { get; set; }
        }
        public List<UserAndRole> users { set; get;}

        public const int ITEMS_PER_PAGE = 10;

        [BindProperty(SupportsGet =true, Name = "p")]
        public int currentPage { get; set; }
        public int countPage { get; set; }

        public async Task OnGet()
        {
            users = await _userManager.Users
                .OrderBy(u => u.UserName)
                .Select(u => new UserAndRole() {
                    Id = u.Id,
                    UserName = u.UserName
                })
                .ToListAsync();
            foreach(var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.RoleNames = string.Join(",", roles);
            }
            // var qr = _userManager.Users.OrderBy(u => u.UserName);

            // int totalUsers = await qr.CountAsync();
            // countPage = (int)Math.Ceiling((double)totalUsers / ITEMS_PER_PAGE);
            // if(currentPage < 1)
            // {
            //     currentPage = 1;
            // }
            // if(currentPage > countPage){
            //     currentPage = countPage;
            // }
            // var qr1 = qr.Skip((currentPage - 1) * ITEMS_PER_PAGE)
            //             .Take(ITEMS_PER_PAGE);
            // users = await qr1.ToListAsync();
        }

        public void OnPost() => RedirectToPage();
    }
}