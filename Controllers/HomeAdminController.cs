using BTL_WEBNC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL_WEBNC.Controllers
{
    [Authorize(Roles = "Admin")] // Có thể bật khi cần phân quyền
    [Route("HomeAdmin")]
    public class HomeAdminController : Controller
    {
        private readonly AppDBContext _context; // DbContext của bạn

        public HomeAdminController(AppDBContext context)
        {
            _context = context;
        }

        // GET: /Admin/HomeAdmin
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Seller)
                .Include(p => p.Category)
                .ToListAsync();
            return View(products);
        }

        // GET: /Admin/HomeAdmin/Create
        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Sellers = await _context.Sellers.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View();
        }

        // POST: /Admin/HomeAdmin/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                // Cập nhật thời gian tạo
                product.CreatedAt = DateTime.Now;

                // Thêm sản phẩm vào DbContext và lưu
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                // Sử dụng TempData để lưu thông báo thành công
                TempData["SuccessMessage"] = "Đã thêm thành công!";

                // Chuyển hướng về trang Index của HomeAdmin
                return RedirectToAction("Index", "HomeAdmin");
            }

            // Nếu model không hợp lệ, load lại danh sách Sellers và Categories để hiển thị form
            ViewBag.Sellers = await _context.Sellers.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(product);
        }

        // GET: /Admin/HomeAdmin/Edit/{id}
        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            ViewBag.Sellers = await _context.Sellers.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(product);
        }

        // POST: /Admin/HomeAdmin/Edit/{id}
        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Product_Id,seller_Id,category_id,Name,Description,Price,Stock,Images,Status,CreatedAt,ApprovalStatus")] Product product)
        {
            if (id != product.Product_Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Products.Any(p => p.Product_Id == product.Product_Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Sellers = await _context.Sellers.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(product);
        }

        // GET: /Admin/HomeAdmin/Delete/{id}
        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products
                .Include(p => p.Seller)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Product_Id == id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: /Admin/HomeAdmin/Delete/{id}
        [HttpPost("Delete/{id:int}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/HomeAdmin/ConfirmProduct/{id}
        [HttpGet("ConfirmProduct/{id:int}")]
        public async Task<IActionResult> ConfirmProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            // Nếu sản phẩm đang ở trạng thái Pending (0) thì cập nhật thành Approved (1)
            if (product.ApprovalStatus == ProductApprovalStatus.Pending)
            {
                product.ApprovalStatus = ProductApprovalStatus.Approved;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Sản phẩm đã được xác nhận.";
            }
            return RedirectToAction("Index");
        }

        // GET: /Admin/HomeAdmin/RejectProduct/{id}
        [HttpGet("RejectProduct/{id:int}")]
        public async Task<IActionResult> RejectProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            // Nếu sản phẩm đang ở trạng thái Pending (0) thì cập nhật thành Rejected (2)
            if (product.ApprovalStatus == ProductApprovalStatus.Pending)
            {
                product.ApprovalStatus = ProductApprovalStatus.Rejected;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Sản phẩm đã bị từ chối.";
            }
            return RedirectToAction("Index");
        }

        // GET: /Admin/Logout
        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            // Thực hiện thao tác đăng xuất nếu cần, ví dụ: 
            // await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}