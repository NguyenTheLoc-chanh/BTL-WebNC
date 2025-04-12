using BTL_WEBNC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BTL_WEBNC.Controllers
{
    [Route("OrderHistory")]
    public class OrderHistoryController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<User> _userManager;

        public OrderHistoryController(AppDBContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: /OrderHistory
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            var orders = await _context.Orders
                .Where(o => o.user_Id == userId)
                // .OrderByDescending(o => o.CreatedAt) // Sắp xếp theo ngày tạo mới nhất
                .ToListAsync();

            return View(orders);
        }
        // GET: /OrderHistory/Details/{order_id}
        [HttpGet("OrderDetails/{order_id}")]
        public async Task<IActionResult> OrderDetails(int order_id)
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
            return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            var order = await _context.Orders
            .Include(o => o.OrderDetails)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == order_id && o.user_Id == userId);

            if (order == null)
            {
            return NotFound();
            }

            return View(order);
        }
    }
}