using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CoreTemp.Common.Helpers;
using CoreTemp.Data.DatabaseContext;
using CoreTemp.Data.DTOs.Common;
using CoreTemp.Data.DTOs.Identity;
using CoreTemp.Data.DTOs.Token;
using CoreTemp.Data.DTOs.User;
using CoreTemp.Data.Models.Identity;
using CoreTemp.Repo.Infrastructure;
using CoreTemp.Services.Sms;
using CoreTemp.Services.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreTemp.Api.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/auth")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1_Api")]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork<CoreTempDbContext> _db;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthController> _logger;
        private readonly IUtilities _utilities;
        private readonly IWebHostEnvironment _env;
        private readonly ISmsService _smsService;
        private ApiReturn<string> errorModel;
        public AuthController(IUnitOfWork<CoreTempDbContext> dbContext, UserManager<User> userManager,
             IMapper mapper, ILogger<AuthController> logger, IUtilities utilities, ISmsService smsService,
            IWebHostEnvironment env
            )
        {
            _db = dbContext;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _utilities = utilities;
            _smsService = smsService;
            _env = env;
            if (string.IsNullOrWhiteSpace(_env.WebRootPath))
            {
                env.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            errorModel = new ApiReturn<string>
            {
                Status = false,
                Message = "",
                Result = null
            };
        }


        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiReturn<LoginResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(TokenRequestDto tokenRequestDto)
        {
            ApiReturn<LoginResponseDto> model = new ApiReturn<LoginResponseDto> { Status = true };

            switch (tokenRequestDto.GrantType)
            {

                case "password":
                    var result = await _utilities.GenerateNewTokenAsync(tokenRequestDto);
                    if (result.status)
                    {
                        var userForReturn = _mapper.Map<UserForDetailedDto>(result.user);
                        model.Message = "ورود با موفقیت انجام شد";
                        model.Result = new LoginResponseDto
                        {
                            token = result.token,
                            refresh_token = result.refresh_token,
                            user = userForReturn
                        };

                        return Ok(model);
                    }
                    else
                    {
                        _logger.LogWarning($"{tokenRequestDto.UserName} درخواست لاگین ناموفق داشته است" + "---" + result.message);
                        errorModel.Message = "1x111keyvanx11";
                        return BadRequest(errorModel);
                    }
                case "social"://ToDo
                    var socialresult = await _utilities.GenerateNewTokenAsync(tokenRequestDto);
                    if (socialresult.status)
                    {
                        var userForReturn = _mapper.Map<UserForDetailedDto>(socialresult.user);
                        model.Message = "ورود با رفرش توکن با موفقیت انجام شد";
                        model.Result = new LoginResponseDto
                        {
                            token = socialresult.token,
                            refresh_token = socialresult.refresh_token,
                            user = _mapper.Map<UserForDetailedDto>(socialresult.user)
                        };
                        return Ok(model);
                    }
                    else
                    {
                        _logger.LogWarning($"{tokenRequestDto.UserName} درخواست لاگین ناموفق داشته است" + "---" + socialresult.message);
                        errorModel.Message = "1x111keyvanx11";
                        return BadRequest(errorModel);
                    }
                case "refresh_token":
                    var res = await _utilities.RefreshAccessTokenAsync(tokenRequestDto);
                    if (res.status)
                    {
                        model.Message = "ورود با رفرش توکن با موفقیت انجام شد";
                        model.Result = new LoginResponseDto
                        {
                            token = res.token,
                            refresh_token = res.refresh_token,
                            user = _mapper.Map<UserForDetailedDto>(res.user)
                        };
                        return Ok(model);
                    }
                    else
                    {
                        _logger.LogWarning($"{tokenRequestDto.UserName} درخواست لاگین ناموفق داشته است" + "---" + res.message);
                        errorModel.Message = "0x000keyvanx00";
                        return BadRequest(errorModel);
                    }
                default:
                    errorModel.Message = "خطا در اعتبار سنجی";
                    return BadRequest(errorModel);
            }
        }


        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiReturn<UserForDetailedDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
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
                    //log
                    _logger.LogInformation($"{userForRegisterDto.UserName} ثبت نام کرده است");
                    //
                    model.Message = "ثبت نام شما با موفقیت انجام شد";
                    model.Result = userForReturn;
                    return Ok(model);
                }
                else if (result.Errors.Any())
                {
                    //log
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


        [AllowAnonymous]
        [HttpPost("code")]
        [ProducesResponseType(typeof(ApiReturn<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<int>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetVerificationCode(GetVerificationCodeDto getVerificationCodeDto)
        {
            var model = new ApiReturn<int>
            {
                Result = 0
            };
            getVerificationCodeDto.Mobile = getVerificationCodeDto.Mobile.ToMobile();
            if (getVerificationCodeDto.Mobile == null)
            {
                model.Status = false;
                model.Message = "شماره موبایل صحیح نمیباشد مثال : 09121234567";
                return BadRequest(model);
            }
            var OtpId = getVerificationCodeDto.Mobile + "-OTP";

            var verfyCodes = await _db._VerificationCodeRepository.GetAllAsync();
            foreach (var vc in verfyCodes.Where(p => p.RemoveDate < DateTime.Now))
            {
                if (vc.RemoveDate < DateTime.Now)
                {
                    _db._VerificationCodeRepository.Delete(vc.Id);
                }
                await _db.SaveAsync();
            }

            var oldOTP = verfyCodes.SingleOrDefault(p => p.Id == OtpId);
            if (oldOTP != null)
            {
                if (oldOTP.ExpirationDate > DateTime.Now)
                {
                    var seconds = Math.Abs((DateTime.Now - oldOTP.ExpirationDate).Seconds);
                    model.Status = false;
                    model.Message = "لطفا " + seconds + " ثانیه دیگر دوباره امتحان کنید ";
                    model.Result = seconds;
                    return BadRequest(model);
                }
                else
                {
                    _db._VerificationCodeRepository.Delete(OtpId);
                    await _db.SaveAsync();
                }
            }
            //
            var user = await _db._UserRepository.GetAsync(p => p.UserName == getVerificationCodeDto.Mobile);
            if (user == null)
            {
                var randomOTP = new Random().Next(10000, 99999);
                if (_smsService.SendFastVerificationCode(getVerificationCodeDto.Mobile, randomOTP.ToString(),"verify"))
                {
                    var vc = new VerificationCode
                    {
                        Code = randomOTP.ToString(),
                        ExpirationDate = DateTime.Now.AddSeconds(60),
                        RemoveDate = DateTime.Now.AddMinutes(2)
                    };
                    vc.Id = OtpId;
                    //
                    await _db._VerificationCodeRepository.InsertAsync(vc);
                    await _db.SaveAsync();

                    model.Status = true;
                    model.Message = "کد فعال سازی با موفقیت ارسال شد";
                    model.Result = (int)(vc.ExpirationDate - DateTime.Now).TotalSeconds;
                    return Ok(model);
                }
                else
                {
                    model.Status = false;
                    model.Message = "خطا در ارسال کد فعال سازی";
                    return BadRequest(model);
                }
            }
            else
            {
                model.Status = false;
                model.Message = "کاربری با این شماره موبایل از قبل وجود دارد";
                return BadRequest(model);
            }
        }


        [Authorize(Policy = "RequiredUserRole")]
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _db._UserRepository.GetByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userProfile = _mapper.Map<UserProfileDto>(users);
            return Ok(userProfile);
        }

        [Authorize(Policy = "RequiredAdminRole")]
        [HttpGet("page")]
        public async Task<IActionResult> GetBlogs([FromQuery] PaginationDto paginationDto)
        {
            var blogsFromRepo = await _db._UserRepository
                .GetAllPagedListAsync(
                paginationDto);

            Response.AddPagination(blogsFromRepo.CurrentPage, blogsFromRepo.PageSize,
                blogsFromRepo.TotalCount, blogsFromRepo.TotalPage);
            //MostViewed
            return Ok(blogsFromRepo);

        }
    }
}
