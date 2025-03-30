using BTL_WEBNC.Models;
using BTL_WEBNC.Services;
using Microsoft.AspNetCore.Mvc;

namespace BTL_WEBNC.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICartService _cartService;
        private readonly AppDBContext _context;
        public CartController(ILogger<HomeController> logger,ICartService cartService, AppDBContext context)
        {
            _logger = logger;
            _cartService = cartService;
            _context = context;
        }
    }
}