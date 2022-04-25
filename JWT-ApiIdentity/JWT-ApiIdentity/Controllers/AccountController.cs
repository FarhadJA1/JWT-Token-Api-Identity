using JWT_ApiIdentity.Data;
using JWT_ApiIdentity.DTOs.Account;
using JWT_ApiIdentity.Models;
using JWT_ApiIdentity.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT_ApiIdentity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        public AccountController(AppDbContext context, 
                                UserManager<AppUser> userManager, 
                                RoleManager<IdentityRole> roleManager, 
                                ITokenService tokenService)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            AppUser user = new()
            {
                Fullname = registerDto.Fullname,
                Email=registerDto.Email,
                UserName=registerDto.Email
            };
            await _userManager.CreateAsync(user, registerDto.Password);
            
            
            return Ok();
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null) return NotFound();
            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password)) return Unauthorized();
            var roles = await _userManager.GetRolesAsync(user);
            string token = _tokenService.GenerateJwtToken(user.UserName, (List<string>)roles);
            
            return Ok(token);
        }
        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> CreateRole([FromQuery] string role)
        {
            await _roleManager.CreateAsync(new IdentityRole { Name = role });
            return Ok();
        }
    }
}
