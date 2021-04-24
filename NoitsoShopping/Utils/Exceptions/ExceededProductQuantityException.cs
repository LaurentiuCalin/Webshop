using System;

namespace NoitsoShopping.Utils.Exceptions
{
    public class ExceededProductQuantityException : Exception
    {
        public ExceededProductQuantityException()
        {

        }

        public ExceededProductQuantityException(string productName, string packageType, int availableQuantity, int requestedQuantity)
            : base($"There aren't {requestedQuantity} {packageType} available of {productName}. Available amount is {availableQuantity}.")
        {

        }
    }
}