using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NoitsoShopping.Utils.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SaleType
    {
        Percentage = 0,
        Fraction = 1,
        MultiItem = 2
    }
}