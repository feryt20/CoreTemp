using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreTemp.Data.DatabaseContext;
using CoreTemp.Data.DTOs.Identity;
using CoreTemp.Data.DTOs.User;
using CoreTemp.Data.Models.Identity;
using CoreTemp.Repo.Infrastructure;
using CoreTemp.Services.Upload;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreTemp.Api.Controllers.User
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/user/profile")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1_User")]
    [Authorize(Policy = "RequiredUserRole")]
    public class ProfileController : ControllerBase
    {
        //Change Image
        //Change Phone
        private readonly IUnitOfWork<CoreTempDbContext> _db;
        private readonly UserManager<MyUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;
        private readonly IWebHostEnvironment _env;
        private ApiReturn<string> errorModel;

        public ProfileController(IUnitOfWork<CoreTempDbContext> dbContext, UserManager<MyUser> userManager,
            IMapper mapper, IUploadService uploadService,
            IWebHostEnvironment env)
        {
            _db = dbContext;
            _userManager = userManager;
            _mapper = mapper;
            _uploadService = uploadService;
            _env = env;

            errorModel = new ApiReturn<string>
            {
                Status = false,
                Message = "",
                Result = null
            };
        }


        [HttpGet("get")]
        [ProducesResponseType(typeof(ApiReturn<UserProfileDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get()
        {
            ApiReturn<UserProfileDto> model = new ApiReturn<UserProfileDto> { Status = true };

            var user = await _db._UserRepository.GetUserByUserNameAsync(User.Identity.Name);

            if (user != null)
            {
                var pgForreturn = _mapper.Map<UserProfileDto>(user);
                model.Message = "Success";
                model.Result = pgForreturn;

                return Ok(model);
            }

            errorModel.Message = "Error";
            errorModel.Result = "خطا در دریافت";
            return BadRequest(errorModel);
        }

        [HttpPut("update")]
        [ProducesResponseType(typeof(ApiReturn<UserProfileDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromForm] UserProfileDto userProfileDto)
        {
            ApiReturn<UserProfileDto> model = new ApiReturn<UserProfileDto> { Status = true };

            //var user = await _db._UserRepository.GetUserByUserNameAsync(User.Identity.Name);
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (userProfileDto.File != null)
            {
                if (userProfileDto.File.Length > 0)
                {
                    var uploadRes = await _uploadService.UploadFileToLocal(
                        userProfileDto.File,
                        Guid.NewGuid().ToString(),
                        _env.WebRootPath,
                        $"{Request.Scheme ?? ""}://{Request.Host.Value ?? ""}{Request.PathBase.Value ?? ""}",
                        "Files\\Profile"
                    );
                    if (uploadRes.Status)
                    {
                        userProfileDto.ImageUrl = uploadRes.Url;
                    }
                    else
                    {
                        return BadRequest(uploadRes.Message);
                    }
                }
                else
                {
                    userProfileDto.ImageUrl = string.Format("{0}://{1}{2}/{3}",
                        Request.Scheme,
                        Request.Host.Value ?? "",
                        Request.PathBase.Value ?? "",
                        "wwwroot/Files/Profile/Logo.jpg");
                }
            }
            user.Name = userProfileDto.Name;
            user.PhoneNumber = userProfileDto.PhoneNumber;
            user.Gender = userProfileDto.Gender;
            user.City = userProfileDto.City;
            user.Address = userProfileDto.Address;
            user.DateOfBirth = userProfileDto.DateOfBirth;
            user.ImageUrl = userProfileDto.ImageUrl;

            if (!string.IsNullOrEmpty(userProfileDto.NewPass))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _userManager.ResetPasswordAsync(user, token, userProfileDto.NewPass);
            }
            var s = await _userManager.UpdateAsync(user);

            if (s.Succeeded)
            {
                var pgForreturn = _mapper.Map<UserProfileDto>(user);
                model.Message = "Success";
                model.Result = pgForreturn;

                return Ok(model);
            }

            errorModel.Message = "Error";
            errorModel.Result = "خطا در ویرایش";
            return BadRequest(errorModel);
        }

    }
}
