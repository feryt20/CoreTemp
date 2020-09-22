using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreTemp.Data.DatabaseContext;
using CoreTemp.Data.DTOs.Identity;
using CoreTemp.Data.DTOs.Token;
using CoreTemp.Data.DTOs.User;
using CoreTemp.Repo.Infrastructure;
using CoreTemp.Services.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreTemp.Api.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1_Api")]
    public class TokenController : ControllerBase
    {
        private readonly IUnitOfWork<CoreTempDbContext> _db;
        private readonly IMapper _mapper;
        private readonly ILogger<TokenController> _logger;
        private readonly IUtilities _utilities;
        private readonly IWebHostEnvironment _env;
        private ApiReturn<string> errorModel;
        public TokenController(IUnitOfWork<CoreTempDbContext> dbContext,
             IMapper mapper, ILogger<TokenController> logger, IUtilities utilities,
            IWebHostEnvironment env
            )
        {
            _db = dbContext;
            _mapper = mapper;
            _logger = logger;
            _utilities = utilities;
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
        [HttpPost("login2")]
        [ProducesResponseType(typeof(ApiReturn<LoginResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(TokenRequestDto tokenRequestDto)
        {
            ApiReturn<LoginResponseDto> model = new ApiReturn<LoginResponseDto> { Status = true };

            switch (tokenRequestDto.GrantType)
            {

                case "password":
                    var result = await _utilities.GenerateNewTokenAsync(tokenRequestDto, true);
                    if (result.status)
                    {
                        var userForReturn = _mapper.Map<UserForDetailedDto>(result.user);
                        //userForReturn.Provider = tokenRequestDto.Provider;
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
                        return Unauthorized(errorModel);
                    }
                case "social":
                    var socialresult = await _utilities.GenerateNewTokenAsync(tokenRequestDto, false);
                    if (socialresult.status)
                    {
                        //var userForReturn = _mapper.Map<UserForDetailedDto>(socialresult.user);
                        //userForReturn.Provider = tokenRequestDto.Provider;
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
                        return Unauthorized(errorModel);
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
                        return Unauthorized(errorModel);
                    }
                default:
                    errorModel.Message = "خطا در اعتبار سنجی";
                    return Unauthorized(errorModel);
            }
        }


    }
}
