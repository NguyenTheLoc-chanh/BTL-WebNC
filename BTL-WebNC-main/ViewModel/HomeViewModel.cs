using BTL_WEBNC.Models;
using System.Collections.Generic;

namespace BTL_WEBNC.ViewModel
{
    public class HomeViewModel
    {
        // Khởi tạo để tránh trường hợp null khi truy cập trong view
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Product> Products { get; set; } = new List<Product>();
    }

    public class ProductSizeModel
    {
        // Danh sách các ProductSizeViewModel, được khởi tạo để tránh lỗi null
        public List<ProductSizeViewModel> Sizes { get; set; } = new List<ProductSizeViewModel>();

        // Sản phẩm hiện tại
        public Product Product { get; set; }

        // Danh sách sản phẩm của nhà bán (nếu cần thiết) được khởi tạo
        public List<Product> ProductSeller { get; set; } = new List<Product>();

        // Thông tin của nhà bán (Seller)
        public Seller Sellers { get; set; }
    }

    public class ProductSizeViewModel
    {
        public int Size_Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
    }
}
