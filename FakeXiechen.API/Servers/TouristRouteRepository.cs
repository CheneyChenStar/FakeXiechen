using FakeXiechen.API.Database;
using FakeXiechen.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiechen.API.Servers
{
    public class TouristRouteRepository : ITouristRouteRepository
    {
        private readonly AppDbContext _context;

        public TouristRouteRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddTouristRoute(TouristRoute touristRoute)
        {
            if (touristRoute == null)
            {
                throw new ArgumentNullException();
            }

            _context.TouristRoutes.Add(touristRoute);
        }

        public void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture touristRoutePicture)
        {
            if (touristRouteId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(touristRouteId));
            }
            if (touristRoutePicture == null)
            {
                throw new ArgumentNullException(nameof(touristRoutePicture));
            }
            touristRoutePicture.TouristRouteId = touristRouteId;
            _context.Add(touristRoutePicture);

        }

        public void DeleteTouristRoutePicture(TouristRoutePicture touristRoutePicture)
        {
            _context.TouristRoutePictures.Remove(touristRoutePicture);
        }

        public void DeleteTouristRoute(TouristRoute touristRoute)
        {
            _context.TouristRoutes.Remove(touristRoute);
        }

        public async  Task<TouristRoute> GetTouristRouteAsync(Guid touristRouteId)
        {
            return await _context.TouristRoutes.Include(t=>t.TouristRoutePictures).FirstOrDefaultAsync(n => n.Id == touristRouteId);
        }
         
        public async Task<TouristRoutePicture> GetTouristRoutePictureAsync(int pictureId)
        {
            return await _context.TouristRoutePictures.Where(t => t.Id == pictureId).FirstOrDefaultAsync();
        } 

        public async Task<IEnumerable<TouristRoutePicture>> GetTouristRoutePicturesAsync(Guid touristRouteId)
        {
            return await _context.TouristRoutePictures.Where(obj => obj.TouristRouteId == touristRouteId).ToListAsync();
        }

        public async Task<IEnumerable<TouristRoute>> GetTouristRoutesAsync(string keyword, string ratingOperator, int? ratingValue)
        {
            // Linq to sql 关键词查循
            IQueryable<TouristRoute> result = _context.TouristRoutes.Include(t => t.TouristRoutePictures);
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim();
                result = result.Where(t => t.Title.Contains(keyword));
            }
            //  评分查循
            if (ratingValue >= 0)
            {
                switch (ratingOperator)
                {
                    case "largerThan":
                        result = result.Where(t => t.Rating >= ratingValue);
                        break;
                    case "lessThan":
                        result = result.Where(t => t.Rating <= ratingValue);
                        break;
                    case "equalTo":
                    default:
                        result = result.Where(t => t.Rating == ratingValue);
                        break;
                }
            }

            return await result.ToListAsync();  //此处才是真正执行数据库查询操作，前面操作都是Linq to sql操作，也就是准备查询SQL
        }

        public async Task<bool> IsExitsForTouristRouteAsync(Guid touristRouteId)
        {
            return await _context.TouristRoutes.AnyAsync(t => t.Id == touristRouteId);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public async Task<IEnumerable<TouristRoute>> GetTouristRoutesByIdsAsync(IEnumerable<Guid> Ids)
        {
            return await _context.TouristRoutes.Where(x => Ids.Contains(x.Id)).ToListAsync(); //最后的ToList表示执行查询，那么返回结果就是查询结果集合
        }

        public void DeleteTouristRoutesByListModel(IEnumerable<TouristRoute> touristRoutes)
        {
            _context.TouristRoutes.RemoveRange(touristRoutes);
        }

        public async Task<ShoppingCart> GetShoppingCartByUserIdAsync(string userId)
        {
            return await _context.ShaoppingCarts
                .Include(s => s.User)
                .Include(s => s.ShoppingCartItems).ThenInclude(li => li.TouristRoute)
                .Where(s => s.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task CreateShoppingCartAsync(ShoppingCart shoppingCart)
        {
            await _context.ShaoppingCarts.AddAsync(shoppingCart);
        }

        public async Task AddShoppingCartItemAsync(LineItem lineItem)
        {
            await _context.LineItems.AddAsync(lineItem);
        }

        public async Task<LineItem> GetLineItemByItemIdAsync(int id)
        {
            return await _context.LineItems
                .Where(l => l.Id == id)
                .FirstOrDefaultAsync();
        }

        public void DeleteLineItem(LineItem lineItem)
        {
             _context.Remove(lineItem);
        }

        public async Task<IEnumerable<LineItem>> GetLineItemsByItemIdsAsync(IEnumerable<int> itemIds)
        {
            return await _context.LineItems.Where(li => itemIds.Contains(li.Id)).ToListAsync();
        }

        public void DeleteLineItems(IEnumerable<LineItem> lineItems)
        {
            _context.LineItems.RemoveRange(lineItems);
        }

        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders.Where(o => o.UserId == userId).ToListAsync();
        }

        public async Task<Order> GetOrderByUserIdAndOrderIdAsync(string userId, Guid orderId)
        {
            return await _context.Orders
                .Include(o=>o.ShoppingCartItems).ThenInclude(oi=>oi.TouristRoute)
                .Where(o=>o.Id == orderId && o.UserId == userId)
                .FirstOrDefaultAsync();
        }
    }
}
