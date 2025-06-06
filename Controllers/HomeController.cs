using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BTL_WEBNC.Models;
using Microsoft.EntityFrameworkCore;
using BTL_WEBNC.Models.ViewModels;
using BTL_WEBNC.Services;

namespace BTL_WEBNC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHomeService _homeService;
    private readonly ICartService _cartService;
    private readonly AppDBContext _context;
    public HomeController(ILogger<HomeController> logger, IHomeService homeService,ICartService cartService, AppDBContext context)
    {
        _logger = logger;
        _homeService = homeService;
        _cartService = cartService;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? categoryId)
    {
        var CategoryList = await _context.Categories.ToListAsync(); 
        List<Product> ProductList;
        if(categoryId.HasValue){
            ProductList = await _homeService.GetProductsByCategoryAsync(categoryId.Value);
        }else{
            ProductList = await _homeService.GetAllProductsAsync();
        }
        
        var model = new HomeViewModel
        {
            Categories = CategoryList, 
            Products = ProductList
        };

        return View(model);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetProductsByCategory(int categoryId)
    {
        var products = await _homeService.GetProductsByCategoryAsync(categoryId);
        return Json(products);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public async Task<IActionResult> GetLatestProducts()
    {
        var latestProducts = await _homeService.GetLatestProductsAsync();
        return Json(latestProducts);
    }
    [HttpGet]
    public IActionResult GetProductsByPrice(string order)
    {
        var products = _homeService.GetProductsByPriceAsync(order);

        return Json(products);
    }

    [HttpGet]
    public JsonResult SearchProducts(string keyword)
    {
        var products = _homeService.SearchProductsAsync(keyword);

        return Json(products);
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts(int page = 1, int pageSize = 15)
    {
        var (products, totalPages, currentPage) = await _homeService.GetPagedProductsAsync(page, pageSize);

        return Json(new
        {
            products,
            totalPages,
            currentPage
        });
    }

    [HttpGet("Home/ProductDetail/{productId:int}")]
    [HttpGet("Home/ProductDetail")]
    public async Task<IActionResult> ProductDetail(int productId)
    {
        try
        {
            var product = await _homeService.GetProductByIdAsync(productId);

            if (product == null)
            {
                Console.WriteLine($"Không tìm thấy sản phẩm với ID = {productId}");
                return NotFound("Sản phẩm không tồn tại.");
            }
            var size = await _homeService.GetSizesByProductIdAsync(productId);
            var productSeller = await _homeService.GetProductBySelerIdAsync(product.seller_Id, product.Name);
            var shop = await _homeService.GetSellerByIDAsync(product.seller_Id);
            if (shop == null)
            {
                return NotFound();
            }
            var model = new ProductSizeModel
            {
                Sizes = size,
                Product = product,
                ProductSeller = productSeller,
                Sellers = shop
            };
            return View(model);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi xảy ra: {ex.Message}");
            return StatusCode(500, "Lỗi hệ thống, vui lòng thử lại sau!");
        }
    }

    [HttpPost("AddToCart")]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
    {
        bool success = await _cartService.AddToCartAsync(request.UserId, request.ProductId, request.SizeId, request.quantity);
        return Json(new { success, message = "Sản phẩm đã được thêm vào giỏ hàng!" });
    }

    [HttpGet("Home/GetCartCount")]
    public async Task<IActionResult> GetCartCount(string userId)
    {
        Console.WriteLine($"API nhận userId: {userId}");
        int count = await _cartService.GetCartCountAsync(userId);
        return Json(new { count });
    }
    public class AddToCartRequest
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public int quantity { get; set; }
    }

}
