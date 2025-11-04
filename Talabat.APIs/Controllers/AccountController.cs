using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,IAuthService authService,IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
                return Unauthorized(new ApiResponse(401));
            //Check Password
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password,false);

            if (result.Succeeded is false)
                return Unauthorized(new ApiResponse(401));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager)

            });


        }

        [HttpPost("register")]

        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {

            if (CheckEmailExists(model.Email).Result.Value)
                 return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "This Email is already exist!!" } });
            
            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split("@")[0],
                PhoneNumber = model.PhoneNumber,

            };

            var result = await _userManager.CreateAsync(user,model.Password);

            if (result.Succeeded is false)
                return BadRequest(new ApiResponse(400));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });
        }

        
        [Authorize/*(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)*/]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            
            var user = await _userManager.FindByEmailAsync(email);

            return Ok(new UserDto
            {
                DisplayName= user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user,_userManager)
            });
        }

        [Authorize ]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAdress()
        {  
            var user = await _userManager.FindUserAsync(User);

            return Ok(user.Adress);


        }

        [Authorize ]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto updatedAddress)
        {
            var address = _mapper.Map<AddressDto, Address>(updatedAddress);

            var user = await _userManager.FindUserAsync(User);

            address.Id = user.Adress.Id;

            user.Adress = address;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400));

            return Ok(updatedAddress);

        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return _userManager.FindByEmailAsync(email) is not null;
        }

    }
}
