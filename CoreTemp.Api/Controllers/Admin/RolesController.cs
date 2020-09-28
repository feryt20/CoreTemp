using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreTemp.Data.DatabaseContext;
using CoreTemp.Data.DTOs.Identity;
using CoreTemp.Data.Models.Identity;
using CoreTemp.Repo.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreTemp.Api.Controllers.Admin
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/admin/roles")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1_Admin")]
    [Authorize(Policy = "RequiredAdminRole")]
    public class RolesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<MyUser> _userManager;
        private ApiReturn<string> errorModel;


        public RolesController(IMapper mapper, UserManager<MyUser> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;

            errorModel = new ApiReturn<string>
            {
                Status = false,
                Message = "",
                Result = null
            };
        }


        [HttpGet("{userId}/list")]
        [ProducesResponseType(typeof(ApiReturn<List<RolesForReturnDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            ApiReturn<List<RolesForReturnDto>> model = new ApiReturn<List<RolesForReturnDto>> { Status = true };

            var user = await _userManager.FindByIdAsync(userId);

            var userRoles = await _userManager.GetRolesAsync(user);


            var roles = new List<RolesForReturnDto>
            {
                new RolesForReturnDto
                {UserId = userId, Value = "Admin", Has = false},
                new RolesForReturnDto
                {UserId = userId, Value = "Operator", Has = false},
                new RolesForReturnDto
                {UserId = userId, Value = "Vip", Has = false},
                new RolesForReturnDto
                {UserId = userId, Value = "User", Has = false}
            };


            foreach (var item in userRoles)
            {
                roles.Single(p => p.Value == item).Has = true;
            }

            model.Message = "Success";
            model.Result = roles;

            return Ok(model);

        }


        [HttpPatch("{userId}/changerole")]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangeRoles(string userId, RoleEditDto roleEditDto)
        {
            ApiReturn<string> model = new ApiReturn<string>() { Status = true };

            var user = await _userManager.FindByIdAsync(userId);

            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles.Any(p => p == roleEditDto.Value))
            {
                if (roleEditDto.Check)
                {
                    model.Message = "Success";
                    model.Result = "";

                    return Ok(model);
                }
                else
                {
                    var result = await _userManager
                         .RemoveFromRoleAsync(user, roleEditDto.Value);
                    if (!result.Succeeded)
                    {
                        errorModel.Message = "Error";
                        errorModel.Result = "خطا در پاک کردن نقش";
                        return BadRequest(errorModel);
                    }
                    else
                    {
                        model.Message = "Success";
                        model.Result = "";

                        return Ok(model);
                    }
                }
            }
            else
            {
                if (!roleEditDto.Check)
                {
                    model.Message = "Success";
                    model.Result = "";

                    return Ok(model);
                }
                else
                {
                    var result = await _userManager
                        .AddToRoleAsync(user, roleEditDto.Value);
                    if (!result.Succeeded)
                    {
                        errorModel.Message = "Error";
                        errorModel.Result = "خطا در اضافه کردن نقش";
                        return BadRequest(errorModel);
                    }
                    else
                    {
                        model.Message = "Success";
                        model.Result = "";

                        return Ok(model);
                    }
                }
            }
        }
    }
}
