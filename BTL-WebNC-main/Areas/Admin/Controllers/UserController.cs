using BTL_WEBNC.Areas.Admin.ViewModels;
using BTL_WEBNC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace BTL_WEBNC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] // Chỉ Admin mới được truy cập phần này
    [Route("Admin/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        //public IActionResult Index()
        //{
        //    return View(); // Trả về view Index.cshtml trong Areas/Admin/Views/User/
        //}
        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: /Admin/User/Index
        [HttpGet]
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users); // Cần tạo view Index.cshtml tại Areas/Admin/Views/User/Index.cshtml
        }

        // GET: /Admin/User/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateUserViewModel());
        }

        // POST: /Admin/User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.UserName, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Nếu chọn gán vai trò
                    if (!string.IsNullOrEmpty(model.Role))
                    {
                        if (!await _roleManager.RoleExistsAsync(model.Role))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(model.Role));
                        }
                        await _userManager.AddToRoleAsync(user, model.Role);
                    }
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        // GET: /Admin/User/Edit/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            var model = new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = roles.FirstOrDefault() // Giả sử chỉ cho phép 1 role
            };
            return View(model);
        }

        // POST: /Admin/User/Edit/{id}
        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditUserViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                    return NotFound();

                user.UserName = model.UserName;
                user.Email = model.Email;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    // Cập nhật lại role
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    if (!string.IsNullOrEmpty(model.Role))
                    {
                        if (!await _roleManager.RoleExistsAsync(model.Role))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(model.Role));
                        }
                        await _userManager.AddToRoleAsync(user, model.Role);
                    }
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        // GET: /Admin/User/Delete/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            return View(user);
        }

        // POST: /Admin/User/Delete/{id}
        [HttpPost("{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return RedirectToAction(nameof(Index));

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
            return View(user);
        }

        // POST: /Admin/User/ToggleAccountStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleAccountStatus(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            // Nếu tài khoản đang bị khóa thì mở khóa, ngược lại khóa tài khoản
            if (user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.Now)
            {
                user.LockoutEnd = null;
            }
            else
            {
                user.LockoutEnd = DateTime.Now.AddYears(100);
            }
            await _userManager.UpdateAsync(user);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult PendingApproval()
        {
            return View(); // Trả về View PendingApproval.cshtml
        }
        //
        [HttpGet]
        [Authorize(Roles = "Admin")] // Chỉ admin được truy cập
        public IActionResult PendingRequests()
        {
            var pendingUsers = _userManager.Users
                .Where(u => u.SellerApprovalStatus == SellerApprovalStatus.Pending) // Lọc trạng thái "Chờ duyệt"
                .ToList();

            return View(pendingUsers); // Trả về danh sách yêu cầu chờ duyệt
        }


        // phê duyệt
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            // Tạo role Seller nếu chưa có
            if (!await _roleManager.RoleExistsAsync("Seller"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Seller"));
            }

            // Phê duyệt người dùng thành Seller
            user.SellerApprovalStatus = SellerApprovalStatus.Approved;
            user.Role = UserRole.Seller; // Dùng Enum trong mã nguồn
            await _userManager.AddToRoleAsync(user, "Seller"); // Gán chuỗi "Seller" cho Identity

            await _userManager.UpdateAsync(user);
            return RedirectToAction(nameof(PendingRequests));
        }


        //từ chối
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId); // Tìm người dùng theo ID
            if (user == null) return NotFound(); // Nếu không tìm thấy, trả về lỗi

            user.SellerApprovalStatus = SellerApprovalStatus.Rejected; // Cập nhật trạng thái "Từ chối"
            await _userManager.UpdateAsync(user); // Lưu thay đổi vào cơ sở dữ liệu
            return RedirectToAction(nameof(PendingRequests)); // Quay lại danh sách yêu cầu chờ duyệt
        }




    }


}
