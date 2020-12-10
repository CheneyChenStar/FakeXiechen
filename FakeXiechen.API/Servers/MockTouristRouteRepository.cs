//using FakeXiechen.API.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace FakeXiechen.API.Servers
//{
//    public class MockTouristRouteRepository : ITouristRouteRepository
//    {
//        private List<TouristRoute> _routes;
//        public MockTouristRouteRepository()
//        {
//            if (_routes == null )
//            {
//                InitTouristRoutes();
//            }
//        }

//        private void InitTouristRoutes()
//        {
//            _routes = new List<TouristRoute>
//            {
//                new TouristRoute
//                {
//                    Id = Guid.NewGuid(),
//                    Title = "雪山",
//                    Description = "一个巍峨的雪山",
//                    OriginalPrice = 1299,
//                    Features = "<p> 吃住行娱购 </p>",
//                    Fees = "<p> 交通费用自理< /p>",
//                    Notes = "<p> 小心危险 < /p>"
//                },
//                new TouristRoute
//                {
//                    Id = Guid.NewGuid(),
//                    Title = "土山",
//                    Description = "一个巍峨的土山",
//                    OriginalPrice = 1099,
//                    Features = "<p> 吃住行娱购 </p>",
//                    Fees = "<p> 交通费用自理< /p>",
//                    Notes = "<p> 小心危险 < /p>"
//                },
//                new TouristRoute
//                {
//                    Id = Guid.NewGuid(),
//                    Title = "火山",
//                    Description = "一个巍峨的火山",
//                    OriginalPrice = 1999,
//                    Features = "<p> 吃住行娱购 </p>",
//                    Fees = "<p> 交通费用自理< /p>",
//                    Notes = "<p> 小心危险 < /p>"
//                }
//            };
//        }

//        public TouristRoute GetTouristRoute(Guid touristRouteId)
//        {
//            return _routes.FirstOrDefault(n => n.Id == touristRouteId);
//        }

//        public IEnumerable<TouristRoute> GetTouristRoutes()
//        {
//            return _routes;
//        }

//        public bool IsExitsForTouristRoute(Guid touristRouteId)
//        {
//            throw new NotImplementedException();
//        }

//        public IEnumerable<TouristRoutePicture> GetTouristRoutePictures(Guid touristRouteId)
//        {
//            throw new NotImplementedException();
//        }

//        public TouristRoutePicture GetTouristRoutePicture(int pictureId)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
