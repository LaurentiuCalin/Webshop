#nullable disable

namespace NoitsoShopping.Domain.Models
{
    public partial class CartProduct
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public virtual Cart Cart { get; set; }
        public virtual Product Product { get; set; }
    }
}
