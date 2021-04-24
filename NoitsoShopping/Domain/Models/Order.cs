using System;
using System.Collections.Generic;

#nullable disable

namespace NoitsoShopping.Domain.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderProducts = new HashSet<OrderProduct>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int PaymentId { get; set; }
        public int AddressId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Address Address { get; set; }
        public virtual Payment Payment { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
