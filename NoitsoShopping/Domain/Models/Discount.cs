using System;
using System.Collections.Generic;

#nullable disable

namespace NoitsoShopping.Domain.Models
{
    public partial class Discount
    {
        public Discount()
        {
            Memberships = new HashSet<Membership>();
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public int Name { get; set; }
        public int Format { get; set; }
        public string FormatConfiguration { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidUntil { get; set; }
        public bool IsActive { get; set; }
        public int MaxQuantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }

        public virtual ICollection<Membership> Memberships { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
