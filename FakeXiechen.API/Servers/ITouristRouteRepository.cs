using FakeXiechen.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiechen.API.Servers
{
    public interface ITouristRouteRepository
    {

        Task<IEnumerable<TouristRoute>> GetTouristRoutesAsync( string keyword, string ratingOperator, int? ratingValue);
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

        Task AddShoppingCartItem(LineItem lineItem);

    }
}
