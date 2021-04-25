namespace Webshop.Domain.DTOs.Discount
{
    public class CreateMembershipDiscount
    {
        public int MembershipId { get; set; }
        public CreateDiscount Discount { get; set; }
    }
}