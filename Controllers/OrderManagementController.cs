using BTL_WEBNC.Models;
using BTL_WEBNC.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL_WEBNC.Controllers
{
    [Authorize(Roles = "Seller")]
    [Route("api/OrderManagement")]
    [ApiController]
    public class OrderManagementController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<User> _userManager;

        public OrderManagementController(AppDBContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("orders")]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var seller = await _context.Sellers
                .FirstOrDefaultAsync(s => s.user_Id == userId);
            if (seller == null)
            {
                return Ok(new List<Orders>());
            }

            var orders = await _context.Orders
                .Include(o => o.OrderDetails)
                .Where(o => o.seller_Id == seller.seller_Id)
                .ToListAsync();

            return View(orders);
        }

        [HttpGet("order-details/{orderId}")]
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {
            var userId = _userManager.GetUserId(User);

            var seller = await _context.Sellers.FirstOrDefaultAsync(s => s.user_Id == userId);
            if (seller == null)
            {
                return new JsonResult(new { success = false, message = "Bạn không có quyền xem chi tiết đơn hàng này." });
            }

            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.seller_Id == seller.seller_Id);

            if (order == null)
            {
                return new JsonResult(new { success = false, message = "Đơn hàng không tồn tại." });
            }

            return new JsonResult(new
            {
                success = true,
                data = new
                {
                    order.Id,
                    // order.CreatedAt,
                    order.total_price,
                    order.Status,
                    OrderDetails = order.OrderDetails.Select(od => new
                    {
                        od.Product.Name,
                        od.Quantity,
                        od.Price,
                        Total = od.Quantity * od.Price
                    }).ToList()
                }
            });
        }

        [HttpPut("confirm-order/{orderId}")]
        public IActionResult ConfirmOrder(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
                return NotFound(new { success = false, message = "Không tìm thấy đơn hàng." });

            order.Status = OrderStatus.Confirmed;
            _context.SaveChanges();

            return Ok(new { success = true, message = "Xác nhận đơn hàng thành công!" });
        }


        [HttpPost("delete-product")]
        public async Task<IActionResult> DeleteProductAsync([FromBody] DeleteRequest request)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Product_Id == request.id);
            if (product == null)
            {
                return BadRequest(new { success = false, message = "Sản phẩm không tồn tại hoặc bạn không có quyền xóa." });
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Xóa sản phẩm thành công!" });
        }

        public class DeleteRequest
        {
            public int id { get; set; }
        }
    }
}
