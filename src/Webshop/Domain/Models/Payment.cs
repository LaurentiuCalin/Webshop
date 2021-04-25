using System.Collections.Generic;

#nullable disable

namespace Webshop.Domain.Models
{
    public class Payment
    {
        public Payment()
        {
            Orders = new HashSet<Order>();
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string CreditCardNumber { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}