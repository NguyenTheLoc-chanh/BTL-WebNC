using BTL_WEBNC.Models;
using BTL_WEBNC.Models.ViewModels;
using BTL_WEBNC.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL_WEBNC.Controllers
{
    [Route("Cart")]
    public class CartController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICartService _cartService;
        private readonly AppDBContext _context;
        private readonly UserManager<User> _userManager;

        public CartController(ILogger<HomeController> logger,UserManager<User> userManager,ICartService cartService, AppDBContext context)
        {
            _logger = logger;
            _cartService = cartService;
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("CartDetailsList")]
        public async Task<IActionResult> CartDetailList()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/login");
            }
            var cartDetails = await _cartService.GetCartDetailsByUserId(user.Id);
            
            var products = await _cartService.GetProductsAsync();

            var model = new CartDetailProductModel
            {
                CartDetails = cartDetails,
                ProductCategory = products,
            };
            return View(model);
        }

        [HttpPost("IncreaseQuantity")]
        public async Task<IActionResult> IncreaseQuantity(int cartDetailId)
        {
            var success = await _cartService.IncreaseQuantityAsync(cartDetailId);
            if (!success) return Json(new { success = false });

            var cartDetail = await _context.CartDetails.FindAsync(cartDetailId);
            return Json(new { success = true, newQuantity = cartDetail.Quantity });
        }

        [HttpPost("DecreaseQuantity")]
        public async Task<IActionResult> DecreaseQuantity(int cartDetailId)
        {
            var success = await _cartService.DecreaseQuantityAsync(cartDetailId);
            if (!success) return Json(new { success = false });

            var cartDetail = await _context.CartDetails.FindAsync(cartDetailId);
            return Json(new { success = true, newQuantity = cartDetail.Quantity });
        }

        [HttpPost("RemoveProductCart")]
        public async Task<IActionResult> RemoveFromCart(int cartDetailId)
        {
            bool success = await _cartService.RemoveFromCart(cartDetailId);
            return Json(new { success });
        }

        // Checkout
        [HttpGet("Checkout/{selectedItems}")]
        public async Task<IActionResult> Checkout(string selectedItems)
        {
            if (string.IsNullOrEmpty(selectedItems))
            {
                return RedirectToAction("Index");
            }

            try
            {
                // Kiểm tra xem tất cả phần tử có phải số hay không
                var cartDetailIds = selectedItems
                    .Split(',', StringSplitOptions.RemoveEmptyEntries) // Loại bỏ khoảng trắng và phần tử trống
                    .Select(s => 
                    {
                        if (int.TryParse(s, out int id))
                            return id;
                        throw new FormatException($"Giá trị '{s}' không hợp lệ.");
                    })
                    .ToList();

                var userId = _userManager.GetUserId(User);
                if (userId == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                // Lấy thông tin sản phẩm
                var selectedProducts = await _cartService.GetSelectedCartDetailsAsync(userId, cartDetailIds);

                // Tính tổng tiền
                float totalAmount = await _cartService.CalculateTotalAmountAsync(cartDetailIds);
                // Lấy danh sách địa chỉ của người dùng
                var addresses = await _cartService.GetUserAddressesAsync(userId);
                var defaultAddress = addresses.FirstOrDefault(a => a.IsDefault) ?? addresses.FirstOrDefault();

                ViewBag.TotalAmount = totalAmount;
                ViewBag.SelectedAddress = defaultAddress;
                ViewBag.Addresses = addresses;
                return View(selectedProducts);
            }
            catch (FormatException ex)
            {
                // Log lỗi và hiển thị thông báo lỗi thân thiện
                _logger.LogError(ex, "Lỗi khi xử lý selectedItems: {SelectedItems}", selectedItems);
                return BadRequest("Dữ liệu không hợp lệ. Vui lòng chọn sản phẩm đúng định dạng.");
            }
        }


        [HttpPost("AddAddress")]
        public async Task<IActionResult> AddAddress([FromBody] AddressDto addressDto)
        {
            try
            {
                // Gọi service
                var result = await _cartService.AddAddressAsync(addressDto.UserId, addressDto.FullName, addressDto.PhoneNumber, addressDto.StreetAddress, addressDto.IsDefault);

                if (result == true)
                {
                    return Json(new { success = true, message = "Thêm địa chỉ mới thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Thêm địa chỉ mới thất bại!" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xử lý thêm địa chỉ");
                return Json(new { success = false, message = "Đã xảy ra lỗi hệ thống: " + ex.Message });
            }
        }

        public class AddressDto
        {
            public string FullName { get; set; }
            public string PhoneNumber { get; set; }
            public string StreetAddress { get; set; }
            public bool IsDefault { get; set; }
            public string UserId { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAddress([FromBody] Address updatedAddress)
        {
            try
            {
                bool result = await _cartService.UpdateAddressAsync(updatedAddress);
                if (result)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "Không thể cập nhật địa chỉ." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật địa chỉ.");
                return Json(new { success = false, message = "Có lỗi xảy ra." });
            }
        }

        // Checkout 
        [HttpPost("ConfirmCheckout")]
        public async Task<IActionResult> ConfirmCheckout([FromBody] CheckoutRequest request)
        {
            var userId = _userManager.GetUserId(User);
            var result = await _cartService.ConfirmCheckoutAsync(userId, request);

            if (result.Success)
            {
                return Json(new
                {
                    success = true,
                    message = "Xác nhận đơn hàng thành công!",
                    orderId = result.OrderId,
                    paymentMethod = request.PaymentMethod
                });
            }

            return Json(new
            {
                success = false,
                message = result.Message
            });
        }

        [HttpGet("CheckoutSuccess/{orderId}")]
        public IActionResult CheckoutSuccess(string orderId)
        {
            if (!int.TryParse(orderId, out int parsedOrderId) || parsedOrderId <= 0)
            {
                return BadRequest("ID đơn hàng không hợp lệ");
            }
            // Lấy thông tin đơn hàng từ database
            try
            {
                var order = _context.OrderDetails
                    // .Include(o => o.OrderDetails)
                    // .ThenInclude(od => od.Product)
                    .FirstOrDefault(o => o.order_Id == parsedOrderId); // Sử dụng parsedOrderId
                if (order == null)
                {
                    return NotFound("Đơn hàng không tồn tại");
                }

                return View(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error loading order {orderId}");
                 return Problem(
                    title: "Lỗi hệ thống",
                    detail: $"Không thể xử lý đơn hàng {orderId}. Chi tiết: {ex.Message}",
                    statusCode: 500
                );
            }
        }
    }
}