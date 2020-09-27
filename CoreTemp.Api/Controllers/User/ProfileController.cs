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
using CoreTemp.Services.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreTemp.Api.Controllers.User
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/user/profile")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1_Api")]
    [Authorize(Policy = "RequiredUserRole")]
    public class ProfileController : ControllerBase
    {
        //Show Profile
        //Edit Profile
        //Change Password
        //Change Image
        //Change Phone
        private readonly IUnitOfWork<CoreTempDbContext> _db;
        private readonly UserManager<MyUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<ProfileController> _logger;
        private readonly IUtilities _utilities;
        private ApiReturn<string> errorModel;

        public ProfileController(IUnitOfWork<CoreTempDbContext> dbContext, UserManager<MyUser> userManager,
            IMapper mapper, ILogger<ProfileController> logger, IUtilities utilities)
        {
            _db = dbContext;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _utilities = utilities;

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
            return BadRequest(errorModel);
        }

        [HttpPut("update")]
        [ProducesResponseType(typeof(ApiReturn<UserProfileDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(UserProfileDto userProfileDto)
        {
            ApiReturn<UserProfileDto> model = new ApiReturn<UserProfileDto> { Status = true };

            //var user = await _db._UserRepository.GetUserByUserNameAsync(User.Identity.Name);
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            user.Name = userProfileDto.Name;
            user.PhoneNumber = userProfileDto.PhoneNumber;
            user.Gender = userProfileDto.Gender;
            user.City = userProfileDto.City;
            user.Address = userProfileDto.Address;
            user.DateOfBirth = userProfileDto.DateOfBirth;

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
            return BadRequest(errorModel);
        }

    }
}
