using BTL_WEBNC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BTL_WEBNC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")] // Chỉ Admin mới có quyền truy cập
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        // Constructor để inject UserManager và RoleManager
        public AdminController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Quản lý người dùng
        public async Task<IActionResult> ManageUsers()
        {
            var users = _userManager.Users.ToList();  // Lấy tất cả người dùng
            return View(users); // Trả về danh sách người dùng trong view
        }

        // Phân quyền cho người dùng
        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles); // Xóa tất cả vai trò hiện tại
                await _userManager.AddToRoleAsync(user, role); // Thêm vai trò mới
                return RedirectToAction("ManageUsers");
            }
            return NotFound();
        }

        // Kích hoạt/Khóa tài khoản
        [HttpPost]
        public async Task<IActionResult> ToggleAccountStatus(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // Kiểm tra xem tài khoản đã bị khóa chưa
                if (user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.Now)
                {
                    user.LockoutEnd = null; // Mở khóa tài khoản
                }
                else
                {
                    user.LockoutEnd = DateTime.Now.AddYears(100); // Khóa tài khoản trong 100 năm
                }
                await _userManager.UpdateAsync(user); // Cập nhật tài khoản
                return RedirectToAction("ManageUsers");
            }
            return NotFound();
        }
    }
}
