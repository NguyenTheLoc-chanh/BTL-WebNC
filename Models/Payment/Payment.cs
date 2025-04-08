using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_WEBNC.Models
{
    [Table("Payments")]
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int pay_id { get; set; }  // Primary key (tuỳ chọn)

        [Required]
        [StringLength(20)]
        public string payment_method { get; set; }  // Phương thức thanh toán

        [Required]
        [StringLength(20)]
        public string status { get; set; }  // Trạng thái (pending, completed, failed...)

        [Required]
        [StringLength(50)]
        public int order_Id { get; set; } 

        // Thiết lập quan hệ với User
        [ForeignKey("order_Id")]
        public virtual Orders Orders { get; set; } 

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime created_at { get; set; } = DateTime.UtcNow; 
    }
}