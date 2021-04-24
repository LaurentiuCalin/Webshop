using System.Threading.Tasks;

namespace NoitsoShopping.Services.CartService
{
    public interface ICartService
    {
        Task AddProductAsync(int cartId, int productId, int quantity);
    }
}