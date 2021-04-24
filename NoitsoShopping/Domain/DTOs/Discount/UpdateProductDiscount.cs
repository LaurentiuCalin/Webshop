using System;

namespace NoitsoShopping.Domain.DTOs.Discount
{
    public class UpdateProductDiscount
    {
        public int ProductId { get; set; }
        public int Name { get; set; }
        public string Format { get; set; }
        public string FormatConfiguration { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidUntil { get; set; }
        public bool IsActive { get; set; }
        public int MaxQuantity { get; set; }
    }
}