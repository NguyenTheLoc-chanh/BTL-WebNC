using BTL_WEBNC.Models;
using BTL_WEBNC.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
                return RedirectToAction("Login", "Account"); 
            }
            var cartDetails = await _cartService.GetCartDetailsByUserId(user.Id);
            return View(cartDetails);
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
    }
}