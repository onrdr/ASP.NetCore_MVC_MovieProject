
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class ShoppingCart
    {
        public Product Product { get; set; }

        [Range(1, 1000, ErrorMessage = "The value must be between 1 and 1000")]
        public int Count { get; set; }
    }
}
