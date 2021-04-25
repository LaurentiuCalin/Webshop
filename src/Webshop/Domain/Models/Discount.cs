using System;
using System.Collections.Generic;

#nullable disable

namespace Webshop.Domain.Models
{
    public class Discount
    {
        public Discount()
        {
            Memberships = new HashSet<Membership>();
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidUntil { get; set; }
        public bool IsActive { get; set; }
        public int MaxQuantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public int Percentage { get; set; }
        public int? MinQuantity { get; set; }

        public virtual ICollection<Membership> Memberships { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}