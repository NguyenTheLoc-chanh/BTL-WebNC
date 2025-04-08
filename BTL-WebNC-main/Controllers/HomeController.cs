using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BTL_WEBNC.Models;
using Microsoft.EntityFrameworkCore;
using BTL_WEBNC.ViewModel; // Lưu ý: HomeViewModel, ProductSizeModel nằm trong namespace này
using BTL_WEBNC.Services;

namespace BTL_WEBNC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeService _homeService;
        private readonly AppDBContext _context;

        public HomeController(ILogger<HomeController> logger, IHomeService homeService, AppDBContext context)
        {
            _logger = logger;
            _homeService = homeService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? categoryId)
        {
            // Lấy danh sách danh mục
            var categoryList = await _context.Categories.ToListAsync();

            List<Product> productList;
            if (categoryId.HasValue)
            {
                // Nếu có chỉ định danh mục, lấy sản phẩm theo danh mục đó
                productList = await _homeService.GetProductsByCategoryAsync(categoryId.Value);
            }
            else
            {
                // Lấy tất cả các sản phẩm
                productList = await _homeService.GetAllProductsAsync();
            }

            // Lọc sản phẩm chỉ lấy những sản phẩm đã được duyệt (Approved)
            productList = productList
                .Where(p => p.ApprovalStatus == BTL_WEBNC.Models.ProductApprovalStatus.Approved)
                .ToList();

            // Khởi tạo model cho view
            var model = new HomeViewModel
            {
                Categories = categoryList,
                Products = productList
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
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
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
                    _logger.LogWarning($"Không tìm thấy sản phẩm với ID = {productId}");
                    return NotFound("Sản phẩm không tồn tại.");
                }

                var sizes = await _homeService.GetSizesByProductIdAsync(productId);
                var productSeller = await _homeService.GetProductBySelerIdAsync(product.seller_Id, product.Name);
                var shop = await _homeService.GetSellerByIDAsync(product.seller_Id);
                if (shop == null)
                {
                    return NotFound("Không tìm thấy thông tin cửa hàng.");
                }

                var model = new ProductSizeModel
                {
                    Sizes = sizes,
                    Product = product,
                    ProductSeller = productSeller,
                    Sellers = shop
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi xảy ra: {ex.Message}");
                return StatusCode(500, "Lỗi hệ thống, vui lòng thử lại sau!");
            }
        }
    }
}
