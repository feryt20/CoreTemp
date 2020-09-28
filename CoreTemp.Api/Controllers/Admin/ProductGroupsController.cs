using System;
using System.Threading.Tasks;
using AutoMapper;
using CoreTemp.Common.Common;
using CoreTemp.Common.Helpers;
using CoreTemp.Data.DatabaseContext;
using CoreTemp.Data.DTOs.Common;
using CoreTemp.Data.DTOs.Identity;
using CoreTemp.Data.DTOs.Product;
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
    [Route("api/v{v:apiVersion}/admin/productgroups")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1_Admin")]
    [Authorize(Policy = "RequiredAdminRole")]
    public class ProductGroupsController : ControllerBase
    {
        private readonly IUnitOfWork<CoreTempDbContext> _db;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;
        private readonly IWebHostEnvironment _env;
        private ApiReturn<string> errorModel;

        public ProductGroupsController(IUnitOfWork<CoreTempDbContext> dbContext, IUploadService uploadService,
             IMapper mapper, IWebHostEnvironment env)
        {
            _db = dbContext;
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

        [HttpGet("list")]
        [ProducesResponseType(typeof(ApiReturn<PagedList<ProductGroup>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> List([FromQuery] PaginationDto paginationDto)
        {
            ApiReturn<PagedList<ProductGroup>> model = new ApiReturn<PagedList<ProductGroup>> { Status = true };

            var pg = await _db._ProductGroupRepository.GetAllPagedListAsync(paginationDto,p=>!p.IsDeleted);
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
        [ProducesResponseType(typeof(ApiReturn<ProductGroupDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(int id)
        {
            ApiReturn<ProductGroupDto> model = new ApiReturn<ProductGroupDto> { Status = true };

            var pg = await _db._ProductGroupRepository.GetAsync(p => p.ProductGroupId == id && !p.IsDeleted);

            if (pg != null)
            {
                var pgForreturn = _mapper.Map<ProductGroupDto>(pg);
                model.Message = "Success";
                model.Result = pgForreturn;

                return Ok(model);
            }

            errorModel.Message = "Error";
            errorModel.Result = "خطا در دریافت";
            return BadRequest(errorModel);
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromForm] ProductGroupDto productGroupDto)
        {
            ApiReturn<string> model = new ApiReturn<string> { Status = true };

            if (productGroupDto.File != null)
            {
                if (productGroupDto.File.Length > 0)
                {
                    var uploadRes = await _uploadService.UploadFileToLocal(
                        productGroupDto.File,
                        Guid.NewGuid().ToString(),
                        _env.WebRootPath,
                        $"{Request.Scheme ?? ""}://{Request.Host.Value ?? ""}{Request.PathBase.Value ?? ""}",
                        "Files\\ProductGroup"
                    );
                    if (uploadRes.Status)
                    {
                        productGroupDto.ImageUrl = uploadRes.Url;
                    }
                    else
                    {
                        return BadRequest(uploadRes.Message);
                    }
                }
                else
                {
                    productGroupDto.ImageUrl = string.Format("{0}://{1}{2}/{3}",
                        Request.Scheme,
                        Request.Host.Value ?? "",
                        Request.PathBase.Value ?? "",
                      "wwwroot/Files/ProductGroup/Logo.jpg");
                }
            }

            var pg = _mapper.Map<ProductGroup>(productGroupDto);
            await _db._ProductGroupRepository.InsertAsync(pg);

            if (await _db.SaveAsync()>0)
            {
                model.Message = "Success";
                model.Result = "با موفقیت افزوده شد";

                return Ok(model);
            }

            errorModel.Message = "Error";
            errorModel.Result = "خطا در افزودن";
            return BadRequest(errorModel);
        }


        [HttpPut("update")]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromForm] ProductGroupDto productGroupDto)
        {
            ApiReturn<string> model = new ApiReturn<string> { Status = true };
            if (productGroupDto.File != null)
            {
                if (productGroupDto.File.Length > 0)
                {
                    var uploadRes = await _uploadService.UploadFileToLocal(
                        productGroupDto.File,
                        Guid.NewGuid().ToString(),
                        _env.WebRootPath,
                        $"{Request.Scheme ?? ""}://{Request.Host.Value ?? ""}{Request.PathBase.Value ?? ""}",
                        "Files\\ProductGroup"
                    );
                    if (uploadRes.Status)
                    {
                        productGroupDto.ImageUrl = uploadRes.Url;
                    }
                    else
                    {
                        return BadRequest(uploadRes.Message);
                    }
                }
                else
                {
                    productGroupDto.ImageUrl = string.Format("{0}://{1}{2}/{3}",
                      Request.Scheme,
                      Request.Host.Value ?? "",
                      Request.PathBase.Value ?? "",
                      "wwwroot/Files/ProductGroup/Logo.jpg");
                }
            }

            var pg = _mapper.Map<ProductGroup>(productGroupDto);
            _db._ProductGroupRepository.Update(pg);

            if (await _db.SaveAsync() > 0)
            {
                model.Message = "Success";
                model.Result = "با موفقیت ویرایش شد";

                return Ok(model);
            }

            errorModel.Message = "Error";
            errorModel.Result = "خطا در ویرایش";
            return BadRequest(errorModel);
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            ApiReturn<string> model = new ApiReturn<string> { Status = true };
            var pg = await _db._ProductGroupRepository.GetByIdAsync(id);
            if (pg != null)
            {
                pg.IsDeleted = true;
                _db._ProductGroupRepository.Update(pg);
                if (await _db.SaveAsync() > 0)
                {
                    model.Message = "Success";
                    model.Result = "با موفقیت حذف شد";

                    return Ok(model);
                }
            }

            errorModel.Message = "Error";
            errorModel.Result = "خطا در حذف";
            return BadRequest(errorModel);
        }
    }
}
