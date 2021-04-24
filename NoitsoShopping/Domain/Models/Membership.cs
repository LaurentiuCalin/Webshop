using System.Collections.Generic;

#nullable disable

namespace NoitsoShopping.Domain.Models
{
    public partial class Membership
    {
        public Membership()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public int? DiscountId { get; set; }
        public string Label { get; set; }

        public virtual Discount Discount { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
