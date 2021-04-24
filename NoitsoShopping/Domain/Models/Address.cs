using System;
using System.Collections.Generic;

#nullable disable

namespace NoitsoShopping.Domain.Models
{
    public partial class Address
    {
        public Address()
        {
            Orders = new HashSet<Order>();
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
