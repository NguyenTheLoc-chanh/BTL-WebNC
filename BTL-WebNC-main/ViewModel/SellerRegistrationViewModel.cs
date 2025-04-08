using System.ComponentModel.DataAnnotations;

namespace BTL_WEBNC.ViewModels
{
    public class SellerRegistrationViewModel
    {
        [Required(ErrorMessage = "Tên cửa hàng là bắt buộc.")]
        [Display(Name = "Tên cửa hàng")]
        public string StoreName { get; set; }

        [Required(ErrorMessage = "Mô tả cửa hàng là bắt buộc.")]
        [Display(Name = "Mô tả cửa hàng")]
        public string StoreDescription { get; set; }
    }
}
