using BTL_WEBNC.Areas.Admin.ViewModels;
using BTL_WEBNC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BTL_WEBNC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class SellerController : Controller
    {
        private readonly UserManager<User> _userManager;

        public SellerController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        // GET: /Admin/Seller/RegisterSeller
        [HttpGet]
        public IActionResult RegisterSeller()
        {
            return View();
        }

        // POST: /Admin/Seller/RegisterSeller
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterSeller(SellerRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User); // Lấy người dùng hiện tại
                if (user == null)
                    return RedirectToAction("Login", "Account"); // Nếu chưa đăng nhập, chuyển đến trang đăng nhập

                // Cập nhật trạng thái của người dùng
                user.SellerApprovalStatus = SellerApprovalStatus.Pending;
                user.StoreName = model.StoreName;
                user.StoreDescription = model.StoreDescription;

                await _userManager.UpdateAsync(user); // Lưu vào cơ sở dữ liệu

                return RedirectToAction("PendingApproval"); // Chuyển đến trang thông báo chờ duyệt
            }

            return View(model); // Hiển thị lại form nếu có lỗi
        }

        // GET: /Admin/Seller/PendingApproval
        [HttpGet]
        public IActionResult PendingApproval()
        {
            return View();
        }
    }
}
