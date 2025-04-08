using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BTL_WEBNC.ViewModels
{
    public class CreateProductViewModel
    {
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(255, ErrorMessage = "Tên sản phẩm không được quá 255 ký tự")]
        public string Name { get; set; }

        public string Description { get; set; }

        // Lưu ý: Trong cơ sở dữ liệu Price có kiểu float nên ta đổi sang float
        [Required(ErrorMessage = "Giá sản phẩm không được để trống")]
        [Range(1, float.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public float Price { get; set; }

        [Required(ErrorMessage = "Số lượng sản phẩm không được để trống")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục sản phẩm")]
        public int CategoryId { get; set; }

        // Thuộc tính dùng để upload file ảnh sản phẩm. Sau khi xử lý, hệ thống sẽ lưu đường dẫn ảnh vào cột Images.
        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn trạng thái sản phẩm")]
        public int Status { get; set; }
        // Có thể interpret: 0 - Active, 1 - OutOfStock, 2 - Disabled (tùy theo định nghĩa Enum)

        // Ngày tạo sản phẩm: nếu muốn người dùng nhập thì để bên view, 
        // tuy nhiên thông thường trường này được gán tự động trong controller.
        [Required(ErrorMessage = "Vui lòng nhập ngày tạo sản phẩm")]
        public DateTime CreatedAt { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn trạng thái phê duyệt sản phẩm")]
        public int ApprovalStatus { get; set; }
        // Có thể interpret: 0 - Pending, 1 - Approved, 2 - Rejected (theo định nghĩa enum của bạn)
    }
}
