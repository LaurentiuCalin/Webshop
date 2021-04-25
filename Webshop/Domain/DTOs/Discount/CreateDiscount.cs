using System;

namespace Webshop.Domain.DTOs.Discount
{
    public class CreateDiscount
    {
        public int Name { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidUntil { get; set; }
        public bool IsActive { get; set; }
        public int MaxQuantity { get; set; }
        public int Percentage { get; set; }
        public int? MinQuantity { get; set; }
    }
}