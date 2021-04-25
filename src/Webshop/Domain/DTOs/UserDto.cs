using System;

namespace Webshop.Domain.DTOs
{
    public class UserDto
    {
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
    }
}