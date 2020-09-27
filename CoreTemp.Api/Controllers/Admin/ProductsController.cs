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
using CoreTemp.Data.DTOs.Product;
using CoreTemp.Data.Models.Site;
using CoreTemp.Repo.Infrastructure;
using CoreTemp.Services.Upload;
using CoreTemp.Services.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreTemp.Api.Controllers.Admin
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/products")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1_Api")]
    [Authorize(Policy = "RequiredAdminRole")]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork<CoreTempDbContext> _db;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;
        private readonly IUtilities _utilities;
        private readonly IUploadService _uploadService;
        private readonly IWebHostEnvironment _env;
        private ApiReturn<string> errorModel;

        public ProductsController(IUnitOfWork<CoreTempDbContext> dbContext, IUploadService uploadService,
             IMapper mapper, ILogger<ProductsController> logger, IUtilities utilities, IWebHostEnvironment env)
        {
            _db = dbContext;
            _mapper = mapper;
            _logger = logger;
            _utilities = utilities;
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
        [ProducesResponseType(typeof(ApiReturn<PagedList<Product>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> List([FromQuery] PaginationDto paginationDto)
        {
            ApiReturn<PagedList<Product>> model = new ApiReturn<PagedList<Product>> { Status = true };

            var pg = await _db._ProductRepository.GetAllPagedListAsync(paginationDto, p => !p.IsDeleted);
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
        [ProducesResponseType(typeof(ApiReturn<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(int id)
        {
            ApiReturn<ProductDto> model = new ApiReturn<ProductDto> { Status = true };

            var pg = await _db._ProductRepository.GetAsync(p => p.ProductId == id && !p.IsDeleted);

            if (pg != null)
            {
                var pgForreturn = _mapper.Map<ProductDto>(pg);
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
        public async Task<IActionResult> Create([FromForm] ProductDto productDto)
        {
            ApiReturn<string> model = new ApiReturn<string> { Status = true };

            if (productDto.File != null)
            {
                if (productDto.File.Length > 0)
                {
                    var uploadRes = await _uploadService.UploadFileToLocal(
                        productDto.File,
                        Guid.NewGuid().ToString(),
                        "",
                        $"{Request.Scheme ?? ""}://{Request.Host.Value ?? ""}{""}",
                        "Files\\Product"
                    );
                    if (uploadRes.Status)
                    {
                        productDto.ImageUrl = uploadRes.Url;
                    }
                    else
                    {
                        return BadRequest(uploadRes.Message);
                    }
                }
                else
                {
                    productDto.ImageUrl = string.Format("{0}://{1}{2}/{3}",
                      Request.Scheme,
                      Request.Host.Value ?? "",
                      "",
                      "Files/Product/Logo.jpg");
                }
            }

            var pg = _mapper.Map<Product>(productDto);
            await _db._ProductRepository.InsertAsync(pg);

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
        public async Task<IActionResult> Update([FromForm] ProductDto productDto)
        {
            ApiReturn<string> model = new ApiReturn<string> { Status = true };
            if (productDto.File != null)
            {
                if (productDto.File.Length > 0)
                {
                    var uploadRes = await _uploadService.UploadFileToLocal(
                        productDto.File,
                        Guid.NewGuid().ToString(),
                        "",
                        $"{Request.Scheme ?? ""}://{Request.Host.Value ?? ""}{""}",
                        "Files\\Product"
                    );
                    if (uploadRes.Status)
                    {
                        productDto.ImageUrl = uploadRes.Url;
                    }
                    else
                    {
                        return BadRequest(uploadRes.Message);
                    }
                }
                else
                {
                    productDto.ImageUrl = string.Format("{0}://{1}{2}/{3}",
                      Request.Scheme,
                      Request.Host.Value ?? "",
                      "",
                      "Files/Product/Logo.jpg");
                }
            }

            var product = _mapper.Map<Product>(productDto);
            _db._ProductRepository.Update(product);

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
        public async Task<IActionResult> Delete(long id)
        {
            ApiReturn<string> model = new ApiReturn<string> { Status = true };
            var product = await _db._ProductRepository.GetByIdAsync(id);
            if (product != null)
            {
                product.IsDeleted = true;
                _db._ProductRepository.Update(product);
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
