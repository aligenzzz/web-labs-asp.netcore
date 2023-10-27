using Web_153505_Bybko.Domain.Entities;

namespace Web_153505_Bybko.Domain.Models
{
    public class CartItem
    {
        public Book Item { get; set; } = null!;

        public int Count { get; set; } = 0;
    }
}
