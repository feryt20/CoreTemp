using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreTemp.Common.Common;
using CoreTemp.Common.Helpers;
using CoreTemp.Data.DatabaseContext;
using CoreTemp.Data.DTOs.Common;
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

namespace CoreTemp.Api.Controllers.Admin
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/admin/users")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1_Admin")]
    [Authorize(Policy = "RequiredAdminRole")]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork<CoreTempDbContext> _db;
        private readonly UserManager<MyUser> _userManager;
        private readonly IUploadService _uploadService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private ApiReturn<string> errorModel;


        public UsersController(IUnitOfWork<CoreTempDbContext> dbContext, UserManager<MyUser> userManager
            , IMapper mapper, IUploadService uploadService, IWebHostEnvironment env)
        {
            _db = dbContext;
            _mapper = mapper;
            _userManager = userManager;
            _uploadService = uploadService;
            _env = env;

            errorModel = new ApiReturn<string>
            {
                Status = false,
                Message = "",
                Result = null
            };
        }

        [HttpGet("list")]
        [ResponseCache(Duration = 600)]
        [ProducesResponseType(typeof(ApiReturn<PagedList<MyUser>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUsers([FromQuery] PaginationDto paginationDto)
        {
            ApiReturn<PagedList<MyUser>> model = new ApiReturn<PagedList<MyUser>> { Status = true };

            var pg = await _db._UserRepository.GetAllPagedListAsync(paginationDto,null);
            if (pg != null)
            {
                Response.AddPagination(pg.CurrentPage, pg.PageSize,
                pg.TotalCount, pg.TotalPage);

                model.Message = "Success";
                model.Result = pg;

                return Ok(model);
            }

            errorModel.Message = "Error";
            errorModel.Result = "خطا در دریافت";
            return BadRequest(errorModel);
        }


        [HttpGet("get/{id}")]
        [ProducesResponseType(typeof(ApiReturn<UserProfileDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(string id)
        {
            ApiReturn<UserProfileDto> model = new ApiReturn<UserProfileDto> { Status = true };

            var user = await _db._UserRepository.GetByIdAsync(id);

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

            var user = await _userManager.FindByIdAsync(userProfileDto.Id);
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
                model.Message = "با موفقیت ویرایش شد";
                model.Result = pgForreturn;

                return Ok(model);
            }

            errorModel.Message = "Error";
            errorModel.Result = "خطا در ویرایش";
            return BadRequest(errorModel);
        }
    }
}
