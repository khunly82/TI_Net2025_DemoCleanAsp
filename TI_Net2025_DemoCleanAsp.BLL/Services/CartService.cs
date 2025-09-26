using TI_Net2025_DemoCleanAsp.DAL.Repositories;
using TI_Net2025_DemoCleanAsp.DL.Entities;

namespace TI_Net2025_DemoCleanAsp.BLL.Services
{
    public class CartService
    {
        private readonly CartRepository _cartRepository;
        private readonly CartItemRepository _cartItemRepository;

        public CartService(CartRepository cartRepository, CartItemRepository cartItemRepository)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
        }

        public Cart MergeCarts(int userId, List<CartItem> sessionCart)
        {
            var userCart = _cartRepository.GetWithCartLineByUserId(userId);

            if(userCart == null)
            {
                userCart = _cartRepository.Add(userId);
            }

            if (userCart!.Items.Count == 0)
            {
                sessionCart.ForEach(sc => sc.CartId = userCart.Id);
                _cartItemRepository.AddAll(sessionCart);
            }
            else
            { 
                foreach (var item in sessionCart)
                {
                    var existingItem = userCart.Items
                        .FirstOrDefault(i => i.ProductId == item.ProductId);

                    if (existingItem != null)
                        existingItem.Quantity += item.Quantity;
                    else
                        userCart.Items.Add(new CartItem
                        {
                            ProductId = item.ProductId,
                            Quantity = item.Quantity
                        });
                }

                _cartRepository.UpdateBatch(userCart);
            }

            return _cartRepository.GetWithCartLineByUserId(userId)!;
        }

        public void AddCartItem(int userId, int productId)
        {
            Cart? cart = _cartRepository.GetByUserId(userId);

            if(cart is null)
            {
                cart = _cartRepository.Add(userId);
            }

            _cartItemRepository.AddItem(cart.Id, productId);
        }

        public Cart? GetWithCartLineByUserId(int userId)
        {
            return _cartRepository.GetWithCartLineByUserId(userId);
        }
    }
}
