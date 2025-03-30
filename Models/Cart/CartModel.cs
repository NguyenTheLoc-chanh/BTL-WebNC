using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_WEBNC.Models
{
    [Table("Carts")]
    public class CartModel
    {
        [Key]
        public int CartId { get; set; }

        [Required]
        public string Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<CartDetails> CartDetails { get; set; }
    }
}