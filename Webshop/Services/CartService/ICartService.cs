using System.Threading.Tasks;

namespace Webshop.Services.CartService
{
    public interface ICartService
    {
        Task AddProductAsync(int cartId, int productId, int quantity);
        Task EmptyCartAsync(int cartId);
    }
}