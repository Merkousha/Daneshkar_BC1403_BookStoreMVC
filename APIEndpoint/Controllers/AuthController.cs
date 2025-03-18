// Controllers/AuthController.cs
using API.Models;
using APIEndpoint.DTOs;
using APIEndpoint.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace APIEndpoint.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole<long>> _roleManager;
        private readonly JwtService _jwtService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
             RoleManager<IdentityRole<long>> roleManager , 
            JwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _roleManager = roleManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new AuthResponseDto { Success = false, Message = "Invalid input data" });

            // Create a new role using the custom type
            var adminRole = new IdentityRole<long>
            {
                Name = "Admin"
            };

            await _roleManager.CreateAsync(adminRole);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                var token = _jwtService.GenerateJwtToken(user);
                return Ok(new AuthResponseDto
                {
                    Success = true,
                    Message = "Registration successful",
                    Token = token,
                    User = new UserDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FullName = user.FullName,
                        PhoneNumber = user.PhoneNumber
                    }
                });
            }

            return BadRequest(new AuthResponseDto
            {
                Success = false,
                Message = string.Join(", ", result.Errors.Select(e => e.Description))
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized(new AuthResponseDto { Success = false, Message = "Invalid email or password" });

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (result.Succeeded)
            {
                var token = _jwtService.GenerateJwtToken(user);
                return Ok(new AuthResponseDto
                {
                    Success = true,
                    Message = "Login successful",
                    Token = token,
                    User = new UserDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FullName = user.FullName,
                        PhoneNumber = user.PhoneNumber
                    }
                });
            }

            return Unauthorized(new AuthResponseDto { Success = false, Message = "Invalid email or password" });
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<ActionResult<AuthResponseDto>> ChangePassword(ChangePasswordDto model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound(new AuthResponseDto { Success = false, Message = "User not found" });

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                return Ok(new AuthResponseDto { Success = true, Message = "Password changed successfully" });
            }

            return BadRequest(new AuthResponseDto
            {
                Success = false,
                Message = string.Join(", ", result.Errors.Select(e => e.Description))
            });
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<UserDto>> GetProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound(new AuthResponseDto { Success = false, Message = "User not found" });

            return Ok(new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber
            });
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<ActionResult<AuthResponseDto>> UpdateProfile(UserDto model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound(new AuthResponseDto { Success = false, Message = "User not found" });

            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(new AuthResponseDto
                {
                    Success = true,
                    Message = "Profile updated successfully",
                    User = new UserDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FullName = user.FullName,
                        PhoneNumber = user.PhoneNumber
                    }
                });
            }

            return BadRequest(new AuthResponseDto
            {
                Success = false,
                Message = string.Join(", ", result.Errors.Select(e => e.Description))
            });
        }
    }
}