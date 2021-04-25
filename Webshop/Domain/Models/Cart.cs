using System;
using System.Collections.Generic;

#nullable disable

namespace Webshop.Domain.Models
{
    public class Cart
    {
        public Cart()
        {
            CartProducts = new HashSet<CartProduct>();
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }

        public virtual ICollection<CartProduct> CartProducts { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}