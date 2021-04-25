namespace Webshop.Domain.DTOs
{
    public class CreateAddress
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}