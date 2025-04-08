using Microsoft.AspNetCore.Mvc;

namespace BTL_WEBNC.Controllers
{
    public class SellerProductController : Controller
    {
        public IActionResult SellerProducts()
        {
            return View();
        }
    }
}
