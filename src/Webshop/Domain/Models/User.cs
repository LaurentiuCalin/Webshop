using System;
using System.Collections.Generic;

#nullable disable

namespace Webshop.Domain.Models
{
    public class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int MembershipId { get; set; }
        public int AddressId { get; set; }
        public int PaymentId { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int CartId { get; set; }

        public virtual Address Address { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual Membership Membership { get; set; }
        public virtual Payment Payment { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}