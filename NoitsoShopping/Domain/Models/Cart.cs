using System;

#nullable disable

namespace NoitsoShopping.Domain.Models
{
    public partial class Cart
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }

        public virtual User User { get; set; }
    }
}
