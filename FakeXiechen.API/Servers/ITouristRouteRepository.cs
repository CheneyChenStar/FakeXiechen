using FakeXiechen.API.Helper;
using FakeXiechen.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiechen.API.Servers
{
    public interface ITouristRouteRepository
    {

        Task<PaginationList<TouristRoute>> GetTouristRoutesAsync( string keyword, string ratingOperator, int? ratingValue,
                                                                int pageSize, int pageNumber, string orderBy);
        Task<TouristRoute> GetTouristRouteAsync(Guid touristRouteId);

        Task<IEnumerable<TouristRoute>> GetTouristRoutesByIdsAsync(IEnumerable<Guid> Ids);

        Task<bool> IsExitsForTouristRouteAsync(Guid touristRouteId);

        Task<IEnumerable<TouristRoutePicture>> GetTouristRoutePicturesAsync(Guid touristRouteId);

        Task<TouristRoutePicture> GetTouristRoutePictureAsync(int pictureId);

        void AddTouristRoute(TouristRoute touristRoute);

        void AddTouristRoutePicture(Guid touristRouteId ,TouristRoutePicture touristRoutePicture);

        void DeleteTouristRoute(TouristRoute touristRoute);

        void DeleteTouristRoutePicture(TouristRoutePicture touristRoutePicture);

        void DeleteTouristRoutesByListModel(IEnumerable<TouristRoute> touristRoutes);

        Task<bool> SaveAsync();

        Task<ShoppingCart> GetShoppingCartByUserIdAsync(string userId);

        Task CreateShoppingCartAsync(ShoppingCart shoppingCart);

        Task AddShoppingCartItemAsync(LineItem lineItem);

        Task<LineItem> GetLineItemByItemIdAsync(int id);

        void DeleteLineItem(LineItem lineItem);

        Task<IEnumerable<LineItem>> GetLineItemsByItemIdsAsync(IEnumerable<int> itemIds);

        void DeleteLineItems(IEnumerable<LineItem> lineItems);

        Task AddOrderAsync(Order order);

        Task<PaginationList<Order>> GetOrdersByUserIdAsync(string userId, int pageNumber, int pageSize);

        Task<Order> GetOrderByUserIdAndOrderIdAsync(string userId, Guid orderId);
    }
}
