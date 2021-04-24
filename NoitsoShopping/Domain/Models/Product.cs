using System;
using System.Collections.Generic;

#nullable disable

namespace NoitsoShopping.Domain.Models
{
    public partial class Product
    {
        public Product()
        {
            CartProducts = new HashSet<CartProduct>();
            OrderProducts = new HashSet<OrderProduct>();
        }

        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int? DiscountId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int PackageQuantity { get; set; }
        public int AvailableQuantity { get; set; }
        public string PackageType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }

        public virtual Category Category { get; set; }
        public virtual Discount Discount { get; set; }
        public virtual ICollection<CartProduct> CartProducts { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
