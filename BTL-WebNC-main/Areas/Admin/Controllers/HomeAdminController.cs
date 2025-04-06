using BTL_WEBNC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BTL_WEBNC.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")] // Có thể bật khi cần phân quyền
    [Route("HomeAdmin")]
    public class HomeAdminController : Controller
    {
        private readonly AppDBContext _context; // Giả sử đây là DbContext của bạn
        

        public HomeAdminController(AppDBContext context)
        {
            _context = context;
        }
        // --------------------------
        // Quản lý sản phẩm
        // --------------------------
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
        public async Task<IActionResult> Create([Bind("Product_Id,seller_Id,category_id,Name,Description,Price,Stock,Images,Status,CreatedAt")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.CreatedAt = DateTime.Now;
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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
            ViewBag.Categories = await _context.Categories.ToListAsync();  // Gán đúng tên "Categories"
            return View(product);
        }


        // POST: /Admin/HomeAdmin/Edit/{id}
        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Product_Id,seller_Id,category_id,Name,Description,Price,Stock,Images,Status,CreatedAt")] Product product)
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
