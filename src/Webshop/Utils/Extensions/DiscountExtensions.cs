using System;
using Webshop.Domain.Models;

namespace Webshop.Utils.Extensions
{
    public static class DiscountExtensions
    {
        public static decimal ApplyDiscount(this int quantityToDiscount, decimal productPrice, int percentage)
        {
            var totalPrice = productPrice * quantityToDiscount;
            var totalDiscount = GetPercentage(percentage, totalPrice);
            var discountedPrice = totalPrice - totalDiscount;
            return discountedPrice / quantityToDiscount;
        }

        private static decimal GetPercentage(this int percentage, decimal total)
        {
            return total * percentage / 100;
        }

        public static int GetMaxQuantityToDiscount(this int productQuantity, int maxQuantity, int? minQuantity)
        {
            var quantityToDiscount = productQuantity < maxQuantity ? productQuantity : maxQuantity;

            if (minQuantity is not null && !(minQuantity > quantityToDiscount))
                quantityToDiscount = quantityToDiscount / (int) minQuantity * (int) minQuantity;

            return quantityToDiscount;
        }

        public static bool IsAvailable(this Discount discount, DateTime currentDateTime)
        {
            return discount.IsActive && discount.ValidFrom == null && discount.ValidUntil == null ||
                   discount.ValidFrom < currentDateTime &&
                   discount.ValidUntil > currentDateTime;
        }
    }
}