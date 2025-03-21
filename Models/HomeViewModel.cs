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
    }

    public class ProductSizeViewModel
    {
        public int Size_Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
    }

}