using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreTemp.Common.Helpers;
using CoreTemp.Data.DatabaseContext;
using CoreTemp.Data.DTOs.Identity;
using CoreTemp.Data.DTOs.User;
using CoreTemp.Data.Models.Identity;
using CoreTemp.Repo.Infrastructure;
using CoreTemp.Services.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreTemp.Api.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1_Api")]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork<CoreTempDbContext> _db;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthController> _logger;
        private readonly IUtilities _utilities;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private ApiReturn<string> errorModel;
        public AuthController(IUnitOfWork<CoreTempDbContext> dbContext,
            IMapper mapper, ILogger<AuthController> logger, IUtilities utilities,
            UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _db = dbContext;
            _logger = logger;
            _mapper = mapper;
            _utilities = utilities;
            _userManager = userManager;
            _signInManager = signInManager;

            errorModel = new ApiReturn<string>
            {
                Status = false,
                Message = "",
                Result = null
            };
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            var userToCreate = new User
            {
                Name = userForRegisterDto.Name,
                UserName = userForRegisterDto.UserName,
                PhoneNumber = userForRegisterDto.PhoneNumber,
                DateOfBirth = DateTime.Now,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(userToCreate, userForRegisterDto.Password);

            if (result.Succeeded)
            {
                return Ok(new
                {
                    user = userForRegisterDto
                });
            }

            return BadRequest("نام کاربری قبلا ثبت شده");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var user = await _userManager.FindByNameAsync(userForLoginDto.UserName);
            if (user == null)
            {
                _logger.LogWarning($"{userForLoginDto.UserName} درخواست لاگین ناموفق داشته است");
                return Unauthorized("کاربری با این یوزر و پس وجود ندارد");
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, userForLoginDto.Password, false);
            if (result.Succeeded)
            {
                var appUser = _userManager.Users
                       .FirstOrDefault(u => u.NormalizedUserName == userForLoginDto.UserName.ToUpper());

                var userForReturn = _mapper.Map<UserForLoginDto>(appUser);
                _logger.LogInformation($"{userForLoginDto.UserName} لاگین کرده است");
                return Ok(new
                {
                    token = await _utilities.GenerateJwtTokenAsync(appUser, userForLoginDto.IsRemember),
                    user = userForReturn
                });
            }
            else
            {
                _logger.LogWarning($"{userForLoginDto.UserName} درخواست لاگین ناموفق داشته است");
                return Unauthorized("کاربری با این یوزر و پس وجود ندارد");
            }
        }


        [Authorize(Policy = "RequiredAdminRole")]
        [HttpGet("getval")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _db._UserRepository.GetAllAsync();

            var userProfile = _mapper.Map<IEnumerable<UserProfileDto>>(users);
            return Ok(userProfile);
        }


        [AllowAnonymous]
        [HttpPost("register2")]
        [ProducesResponseType(typeof(ApiReturn<UserForDetailedDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register2(UserForRegisterDto userForRegisterDto)
        {
            var model = new ApiReturn<UserForDetailedDto>
            {
                Status = true
            };
            userForRegisterDto.UserName = userForRegisterDto.UserName.ToMobile();
            if (userForRegisterDto.UserName == null)
            {
                model.Status = false;
                model.Message = "شماره موبایل صحیح نمیباشد مثال : 09121234567";
                return BadRequest(model);
            }
            var OtpId = userForRegisterDto.UserName + "-OTP";
            //
            var code = await _db._VerificationCodeRepository.GetByIdAsync(OtpId);
            if (code == null)
            {
                errorModel.Message = "کد فعالسازی صحیح نمباشد اقدام به ارسال دوباره ی کد بکنید";
                return BadRequest(errorModel);
            }
            if (code.ExpirationDate < DateTime.Now)
            {
                _db._VerificationCodeRepository.Delete(OtpId);
                await _db.SaveAsync();
                errorModel.Message = "کد فعالسازی منقضی شده است اقدام به ارسال دوباره ی کد بکنید";
                return BadRequest(errorModel);
            }
            if (code.Code == userForRegisterDto.Code)
            {
                var userToCreate = new User
                {
                    UserName = userForRegisterDto.UserName,
                    Name = userForRegisterDto.Name,
                    PhoneNumber = userForRegisterDto.UserName,
                    Address = "",
                    City = "",
                    Gender = true,
                    DateOfBirth = DateTime.Now,
                    IsActive = true,
                    PhoneNumberConfirmed = true
                };


                var result = await _userManager.CreateAsync(userToCreate, userForRegisterDto.Password);

                if (result.Succeeded)
                {
                    var creaatedUser = await _userManager.FindByNameAsync(userToCreate.UserName);
                    await _userManager.AddToRolesAsync(creaatedUser, new[] { "User" });

                    var userForReturn = _mapper.Map<UserForDetailedDto>(userToCreate);

                    _logger.LogInformation($"{userForRegisterDto.Name} - {userForRegisterDto.UserName} ثبت نام کرده است");
                    //
                    model.Message = "ثبت نام شما با موفقیت انجام شد";
                    model.Result = userForReturn;
                    return CreatedAtRoute("GetUsers", new
                    {
                        controller = "Users",
                        v = HttpContext.GetRequestedApiVersion().ToString(),
                        id = userToCreate.Id
                    }, model);
                }
                else if (result.Errors.Any())
                {
                    _logger.LogWarning(result.Errors.First().Description);
                    //
                    errorModel.Message = result.Errors.First().Description;
                    return BadRequest(errorModel);
                }
                else
                {
                    errorModel.Message = "خطای نامشخص";
                    return BadRequest(errorModel);
                }
            }
            else
            {
                errorModel.Message = "کد فعالسازی صحیح نمباشد اقدام به ارسال دوباره ی کد بکنید";
                return BadRequest(errorModel);
            }
        }

    }
}
