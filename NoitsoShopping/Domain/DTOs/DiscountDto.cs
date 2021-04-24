﻿using System;

namespace NoitsoShopping.Domain.DTOs
{
    public class DiscountDto
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidUntil { get; set; }
        public bool IsActive { get; set; }
        public int MaxQuantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public decimal Percentage { get; set; }
        public int? MinQuantity { get; set; }
    }
}