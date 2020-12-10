using FakeXiechen.API.DTOs;
using FakeXiechen.API.Models;
using FakeXiechen.API.Servers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FakeXiechen.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticateController : ControllerBase
    {
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthenticateController(
            IConfiguration configuration,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITouristRouteRepository touristRouteRepository
        )
        {
            _touristRouteRepository = touristRouteRepository;
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
        {
            // 1. 验证用户名和密码
            var loginResult = await _signInManager.PasswordSignInAsync(
                loginDto.Email,     //username 这里是邮箱
                loginDto.Password,  // password
                false,              // 有限次尝试，false
                false               // 超过限定次数锁定账号，false
                );
            if (!loginResult.Succeeded) //登入验证失败
            {
                return BadRequest();
            }
            
            //登入成功，数据库取用户数据
            var user = await _userManager.FindByNameAsync(loginDto.Email);

            // 2. 创建jwt: 获得用户数据后，可根据用户数据配置jwt token

            // header 主要表面用的什么加密算法
            var signingAlgorithm = SecurityAlgorithms.HmacSha256;

            // payload
            var claims = new List<Claim>
            {
                // sub表示用户ID
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, user.Id),
                //new Claim(ClaimTypes.Role, "Admin")
            };
            var roleNames = await _userManager.GetRolesAsync(user); //获取用户所以角色信息
            foreach (var roleName in roleNames) //根据用户角色创建一系列claim
            {
                var roleClaim = new Claim(ClaimTypes.Role, roleName);
                claims.Add(roleClaim);
            }

            // signature
            var secretBytes = Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]);
            var signingKey = new SymmetricSecurityKey(secretBytes);
            var signingCredentials = new SigningCredentials(signingKey, signingAlgorithm); 

            var token = new JwtSecurityToken(
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
                claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials
                );
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            
            // 3. return 200 + jwt token
            return Ok(tokenStr);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegisterDto registerDto)
        {
            var user = new AppUser()
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
            };

            
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            // 初始化购物车
            var shoppingCart = new ShoppingCart()
            {
                Id = Guid.NewGuid(),
                UserId = user.Id

            };

            await _touristRouteRepository.CreateShoppingCartAsync(shoppingCart);
            await _touristRouteRepository.SaveAsync();

            return Ok();
        }
    }
}
