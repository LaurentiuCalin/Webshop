using System;
using System.Linq;

namespace Webshop.Utils.Exceptions
{
    public class InvalidDiscountConfigurationException : Exception
    {
        public InvalidDiscountConfigurationException(string discountName, System.Collections.Generic.List<FluentValidation.Results.ValidationFailure> errors)
            : base($"The configuration for the {discountName} discount was invalid. Errors found: {string.Join(", ", errors.Select(_=>_.ErrorMessage))}")
        {
        }
    }
}