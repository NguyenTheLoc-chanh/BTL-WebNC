using BTL_WEBNC.Areas.Admin.ViewModels;
using BTL_WEBNC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

      
        private readonly AppDBContext _dbContext;
        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, AppDBContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext; // Inject DbContext
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


        //// phê duyệt
      

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            // Tạo role "Seller" nếu chưa có
            if (!await _roleManager.RoleExistsAsync("Seller"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Seller"));
            }

            await _userManager.AddToRoleAsync(user, "Seller");
            user.SellerApprovalStatus = SellerApprovalStatus.Approved;

            // Thêm bản ghi Seller vào bảng Sellers
            var seller = new BTL_WEBNC.Models.Seller
            {
                user_Id = user.Id,
                StoreName = user.StoreName ?? "Cửa hàng chưa có tên",
                StoreLogo = null,
                Status = SellerStatus.Active
            };


            try
            {
                _dbContext.Sellers.Add(seller);
                await _dbContext.SaveChangesAsync();

                TempData["SuccessMessage"] = "Người dùng đã được phê duyệt thành người bán.";
                //return RedirectToAction(nameof(PendingRequests));
                return Ok(); // Trả về 200 OK, không cần chuyển hướng
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Lỗi khi phê duyệt người bán: " + ex.Message);
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xử lý yêu cầu.";
                return RedirectToAction(nameof(PendingRequests));
            }
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
            return Ok(); // Trả về 200 OK, không cần chuyển hướng
            //return RedirectToAction(nameof(PendingRequests)); // Quay lại danh sách yêu cầu chờ duyệt
        }




    }


}
