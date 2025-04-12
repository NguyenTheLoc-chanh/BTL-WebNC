using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BTL_WEBNC.Models;
using Org.BouncyCastle.Tls;
using Microsoft.AspNetCore.Authorization;

namespace BTL_WEBNC.ProductManagement.Sellers
{
    [Authorize(Roles = "Seller")]
    public class EditModel : PageModel
    {
        private readonly AppDBContext _context;

        public EditModel(AppDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; }
        public List<Category> Categories { get; set; }

        public async Task<IActionResult> OnGetAsync(int? productid)
        {
            if (productid == null)
            {
                return NotFound();
            }

            Product = await _context.Products.FindAsync(productid);
            Categories = await _context.Categories.ToListAsync();
            if (Categories == null)
            {
                Categories = new List<Category>();
            }

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateAsync(int? productid)
        {
            if (productid == null)
            {
            return NotFound();
            }

            var productToUpdate = await _context.Products.FindAsync(productid);
            if (productToUpdate == null)
            {
            return NotFound();
            }

            // Manually updating the properties
            productToUpdate.Name = Product.Name;
            productToUpdate.Price = Product.Price;
            productToUpdate.Images = Product.Images;
            productToUpdate.Stock = Product.Stock;
            productToUpdate.Status = Product.Status;
            productToUpdate.Description = Product.Description;
            productToUpdate.category_id = Product.category_id;

            try
            {
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
            return RedirectToPage("./Index");
            }
            catch (DbUpdateException)
            {
            TempData["ErrorMessage"] = "Cập nhật sản phẩm thất bại!";
            }

            Categories = await _context.Categories.ToListAsync();
            if (Categories == null)
            {
            Categories = new List<Category>();
            }

            return Page();
        }
    }
}