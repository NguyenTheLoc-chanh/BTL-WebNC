using BTL_WEBNC.Models;
using BTL_WEBNC.Repositories;

namespace BTL_WEBNC.Services
{
    public class HomeService : IHomeService
    {
        private readonly IHomeRepository _homeRepository;

        public HomeService(IHomeRepository homeRepository)
        {
            _homeRepository = homeRepository;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _homeRepository.GetAllProductsAsync();
        }
        
        public async Task<(List<Product>, int, int)> GetPagedProductsAsync(int page, int pageSize)
        {
            var totalProducts = await _homeRepository.GetTotalProductsAsync();
            var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            var products = await _homeRepository.GetProductsAsync(page, pageSize);

            return (products, totalPages, page);
        }

        public async Task<List<Product>> GetLatestProductsAsync()
        {
            return await _homeRepository.GetLatestProductsAsync();
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _homeRepository.GetProductsByCategoryAsync(categoryId);
        }

        public List<Product> GetProductsByPriceAsync(string order)
        {
            return _homeRepository.GetProductsByPriceAsync(order);
        }

        public List<Product> SearchProductsAsync(string keyword)
        {
            return _homeRepository.SearchProductsAsync(keyword);
        }

        public Task<Product?> GetProductByIdAsync(int productId)
        {
            return _homeRepository.GetProductByIdAsync(productId);
        }
    }
}
