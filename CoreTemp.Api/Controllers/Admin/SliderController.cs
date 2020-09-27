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
using CoreTemp.Data.DTOs.Slider;
using CoreTemp.Data.Models.Site;
using CoreTemp.Repo.Infrastructure;
using CoreTemp.Services.Upload;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreTemp.Api.Controllers.Admin
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/slider")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1_Admin")]
    [Authorize(Policy = "RequiredAdminRole")]
    public class SliderController : ControllerBase
    {
        private readonly IUnitOfWork<CoreTempDbContext> _db;
        private readonly IMapper _mapper;
        private readonly ILogger<SliderController> _logger;
        private readonly IUploadService _uploadService;
        private readonly IWebHostEnvironment _env;
        private ApiReturn<string> errorModel;

        public SliderController(IUnitOfWork<CoreTempDbContext> dbContext,
            IMapper mapper, ILogger<SliderController> logger, 
            IUploadService uploadService, IWebHostEnvironment env)
        {
            _db = dbContext;
            _mapper = mapper;
            _logger = logger;
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
        [ProducesResponseType(typeof(ApiReturn<PagedList<Slider>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> List([FromQuery] PaginationDto paginationDto)
        {
            ApiReturn<PagedList<Slider>> model = new ApiReturn<PagedList<Slider>> { Status = true };

            var pg = await _db._SliderRepository.GetAllPagedListAsync(paginationDto, p => !p.IsDeleted);
            if (pg != null)
            {
                Response.AddPagination(pg.CurrentPage, pg.PageSize,
                pg.TotalCount, pg.TotalPage);

                model.Message = "Success";
                model.Result = pg;

                return Ok(model);
            }

            errorModel.Message = "Error";
            return BadRequest(errorModel);
        }

        [HttpGet("get")]
        [ProducesResponseType(typeof(ApiReturn<SliderDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(int id)
        {
            ApiReturn<SliderDto> model = new ApiReturn<SliderDto> { Status = true };

            var pg = await _db._SliderRepository.GetAsync(p => p.SliderId == id && !p.IsDeleted);

            if (pg != null)
            {
                var pgForreturn = _mapper.Map<SliderDto>(pg);
                model.Message = "Success";
                model.Result = pgForreturn;

                return Ok(model);
            }

            errorModel.Message = "Error";
            return BadRequest(errorModel);
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromForm] SliderDto sliderDto)
        {
            ApiReturn<string> model = new ApiReturn<string> { Status = true };
            if (sliderDto.File != null)
            {
                if (sliderDto.File.Length > 0)
                {
                    var uploadRes = await _uploadService.UploadFileToLocal(
                        sliderDto.File,
                        Guid.NewGuid().ToString(),
                         _env.WebRootPath,
                        $"{Request.Scheme ?? ""}://{Request.Host.Value ?? ""}{Request.PathBase.Value ?? ""}",
                        "Files\\Slider"
                    );
                    if (uploadRes.Status)
                    {
                        sliderDto.ImageUrl = uploadRes.Url;
                    }
                    else
                    {
                        return BadRequest(uploadRes.Message);
                    }
                }
                else
                {
                    sliderDto.ImageUrl = string.Format("{0}://{1}{2}/{3}",
                        Request.Scheme,
                        Request.Host.Value ?? "",
                        Request.PathBase.Value ?? "",
                        "wwwroot/Files/Slider/Logo.png");
                }
            }

            var pg = _mapper.Map<Slider>(sliderDto);
            await _db._SliderRepository.InsertAsync(pg);

            if (await _db.SaveAsync() > 0)
            {
                model.Message = "Success";
                model.Result = "با موفقیت افزوده شد";

                return Ok(model);
            }

            errorModel.Message = "Error";
            return BadRequest(errorModel);
        }


        [HttpPut("update")]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromForm] SliderDto sliderDto)
        {
            ApiReturn<string> model = new ApiReturn<string> { Status = true };
            if (sliderDto.File != null)
            {
                if (sliderDto.File.Length > 0)
                {
                    var uploadRes = await _uploadService.UploadFileToLocal(
                        sliderDto.File,
                        Guid.NewGuid().ToString(),
                         _env.WebRootPath,
                        $"{Request.Scheme ?? ""}://{Request.Host.Value ?? ""}{Request.PathBase.Value ?? ""}",
                        "Files\\Slider"
                    );
                    if (uploadRes.Status)
                    {
                        sliderDto.ImageUrl = uploadRes.Url;
                    }
                    else
                    {
                        return BadRequest(uploadRes.Message);
                    }
                }
                else
                {
                    sliderDto.ImageUrl = string.Format("{0}://{1}{2}/{3}",
                        Request.Scheme,
                        Request.Host.Value ?? "",
                        Request.PathBase.Value ?? "",
                        "wwwroot/Files/Slider/Logo.png");
                }
            }

            var pg = _mapper.Map<Slider>(sliderDto);
            _db._SliderRepository.Update(pg);

            if (await _db.SaveAsync() > 0)
            {
                model.Message = "Success";
                model.Result = "با موفقیت ویرایش شد";

                return Ok(model);
            }

            errorModel.Message = "Error";
            return BadRequest(errorModel);
        }

        [HttpDelete("delete")]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            ApiReturn<string> model = new ApiReturn<string> { Status = true };
            var pg = await _db._SliderRepository.GetByIdAsync(id);
            if (pg != null)
            {
                pg.IsDeleted = true;
                _db._SliderRepository.Update(pg);
                if (await _db.SaveAsync() > 0)
                {
                    model.Message = "Success";
                    model.Result = "با موفقیت حذف شد";

                    return Ok(model);
                }
            }

            errorModel.Message = "Error";
            return BadRequest(errorModel);
        }

    }
}
