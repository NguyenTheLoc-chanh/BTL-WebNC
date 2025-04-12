using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_WEBNC.Models
{
    public enum OrderStatus
    {
        Pending,   // Chờ xác nhận
        Confirmed, // Đã xác nhận
        Completed  // Hoàn thành
    }

    [Table("Orders")]
    public class Orders
    {
        [Key] 
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "float")]
        public float total_price { get; set; }

        [Required]
        public string user_Id { get; set; }
        
        [Required]
        public string seller_Id { get; set; }

        [Required]
        public int address_Id { get; set; }

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [ForeignKey("seller_Id")]
        public virtual Seller Seller { get; set; }

        // Thiết lập quan hệ với User
        [ForeignKey("user_Id")]
        public virtual User User { get; set; }

        [ForeignKey("address_Id")]
        public virtual Address Address { get; set; }

        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}