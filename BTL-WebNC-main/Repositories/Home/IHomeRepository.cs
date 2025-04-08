using BTL_WEBNC.Models;
using BTL_WEBNC.ViewModel;

namespace BTL_WEBNC.Repositories
{
    public interface IHomeRepository
    {
        // Lấy ra sản phẩm 2 ngày gần nhất cho trang Home
        Task<List<Product>> GetAllProductsAsync();
        Task<List<Product>> GetProductsAsync(int page, int pageSize);
        Task<int> GetTotalProductsAsync();
        Task<List<Product>> GetLatestProductsAsync();
        Task<List<Product>> GetProductsByCategoryAsync(int categoryId);
        List<Product> GetProductsByPriceAsync(string order);
        List<Product> SearchProductsAsync(string keyword);

        //Lấy ra chi tiết sản phẩm
        Task<Product?> GetProductByIdAsync(int id);
        Task<List<ProductSizeViewModel>> GetSizesByProductIdAsync(int productId);
        Task<List<Product>> GetProductBySelerIdAsync(int sellerID, string keywords);

        Task<Seller?> GetSellerByIDAsync(int seller_Id);
    }
}
