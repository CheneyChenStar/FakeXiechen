using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FakeXiechen.API.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace FakeXiechen.API.Database
{
    public class AppDbContext :  IdentityDbContext<AppUser> //DbContext
    {
        public DbSet<TouristRoute> TouristRoutes { get; set; }
        
        public DbSet<TouristRoutePicture> TouristRoutePictures { get; set; }

        public DbSet<LineItem> LineItems { get; set; }

        public DbSet<ShoppingCart> ShaoppingCarts { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 添加种子数据
            //modelBuilder.Entity<TouristRoute>().HasData(new TouristRoute()
            //{
            //    Id = Guid.NewGuid(),
            //    Titile = "new Title",
            //    Description = "this is description",
            //    OrininalPrice = 0,
            //    CreateTime = DateTime.UtcNow
            //}) ;
            //string workSpacePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            var touristReouteJsonData = File.ReadAllText(@"touristRoutesMockData.json");
            IList<TouristRoute> touristRoutes = JsonConvert.DeserializeObject<IList<TouristRoute>>(touristReouteJsonData);
            modelBuilder.Entity<TouristRoute>().HasData(touristRoutes);

            var touristRoutePictureJsonData = File.ReadAllText(@"touristRoutePicturesMockData.json");
            IList<TouristRoutePicture> touristRoutePictures = JsonConvert.DeserializeObject<IList<TouristRoutePicture>>(touristRoutePictureJsonData);
            modelBuilder.Entity<TouristRoutePicture>().HasData(touristRoutePictures);

            // 初始化用户与角色种子数据
            // 1.配置用户与角色外键关系
            modelBuilder.Entity<AppUser>(u =>
                u.HasMany(x=>x.UserRoles)
                .WithOne().HasForeignKey(ur => ur.UserId).IsRequired()
            );

            //2.添加管理员角色
            var adminRoleId = "308660dc-ae51-480f-824d-7dca6714c3e2";
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole(  )
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper()
                }
            );

            // 3.添加用户
            var adminUserId = "90184155-dee0-40c9-bb1e-b5ed07afc04e";
            AppUser adminUser = new AppUser
            {
                Id = adminUserId,
                UserName = "admin@fakexiecheng.com",
                NormalizedUserName = "admin@fakexiecheng.com".ToUpper(),
                Email = "admin@fakexiecheng.com",
                NormalizedEmail = "admin@fakexiecheng.com".ToUpper(),
                TwoFactorEnabled = false,
                EmailConfirmed = true,
                PhoneNumber = "123456789",
                PhoneNumberConfirmed = false
            };

            var ph = new PasswordHasher<AppUser>();
            adminUser.PasswordHash = ph.HashPassword(adminUser, "Fake123$");
            modelBuilder.Entity<AppUser>().HasData(adminUser);

            // 4.给用户设置管理员角色
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>()
            {
                RoleId =  adminRoleId,
                UserId = adminUserId
            });

            base.OnModelCreating(modelBuilder);
        }
    }
} 
