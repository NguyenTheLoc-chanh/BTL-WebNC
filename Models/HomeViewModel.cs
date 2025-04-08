namespace BTL_WEBNC.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<Category> Categories { get; set; } 
        public List<Product> Products { get; set; }
    }

    public class ProductSizeModel
    {
        public List<ProductSizeViewModel> Sizes { get; set; }
        public Product Product { get; set; }

        public List<Product> ProductSeller { get; set; }

        public Seller Sellers { get; set; }
    }

    public class ProductSizeViewModel
    {
        public int Size_Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
    }

    public class CartDetailModel
    {
        public int CartDetailId { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public float ProductPrice { get; set; }
        public string ImageUrl { get; set; }
    }

    public class CartDetailProductModel
    {
        public List<CartDetailModel> CartDetails { get; set; }

        public List<Product> ProductCategory { get; set; }
    }

    // Check out
    public class CheckoutRequest
    {
        public List<int> CartItemIds { get; set; }
        public int AddressId { get; set; }
        public float TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public List<CartItemDto> CartItems { get; set; }
    }

    public class CartItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
    }

}