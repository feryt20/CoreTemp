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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreTemp.Api.Controllers.Site
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/index")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1_Site")]
    [AllowAnonymous]
    public class IndexController : ControllerBase
    {
        //Sliders

        private readonly IUnitOfWork<CoreTempDbContext> _db;
        private readonly IMapper _mapper;
        private ApiReturn<string> errorModel;

        public IndexController(IUnitOfWork<CoreTempDbContext> dbContext,
            IMapper mapper)
        {
            _db = dbContext;
            _mapper = mapper;

            errorModel = new ApiReturn<string>
            {
                Status = false,
                Message = "",
                Result = null
            };
        }

        [HttpGet("products")]
        //[ResponseCache(Duration = 600)]
        [ProducesResponseType(typeof(ApiReturn<PagedList<Product>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Products([FromQuery] PaginationDto paginationDto)
        {
            ApiReturn<PagedList<Product>> model = new ApiReturn<PagedList<Product>> { Status = true };

            var order = await _db._ProductRepository.GetAllPagedListAsync(paginationDto, p => !p.IsDeleted);
            if (order != null)
            {
                Response.AddPagination(order.CurrentPage, order.PageSize,
                order.TotalCount, order.TotalPage);

                model.Message = "Success";
                model.Result = order;

                return Ok(model);
            }

            errorModel.Message = "Error";
            errorModel.Result = "خطا در دریافت";
            return BadRequest(errorModel);
        }

        [HttpGet("{group}/products")]
        //[ResponseCache(Duration = 600)]
        [ProducesResponseType(typeof(ApiReturn<PagedList<Product>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Products(int group,[FromQuery] PaginationDto paginationDto)
        {
            ApiReturn<PagedList<Product>> model = new ApiReturn<PagedList<Product>> { Status = true };

            var order = await _db._ProductRepository.GetAllPagedListAsync(paginationDto, p => p.ProductGroupId == group && !p.IsDeleted);
            if (order != null)
            {
                Response.AddPagination(order.CurrentPage, order.PageSize,
                order.TotalCount, order.TotalPage);

                model.Message = "Success";
                model.Result = order;

                return Ok(model);
            }

            errorModel.Message = "Error";
            errorModel.Result = "خطا در دریافت";
            return BadRequest(errorModel);
        }

        [HttpGet("product/{id}")]
        //[ResponseCache(Duration = 600)]
        [ProducesResponseType(typeof(ApiReturn<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Product(int id)
        {
            ApiReturn<ProductDto> model = new ApiReturn<ProductDto> { Status = true };

            var pg = await _db._ProductRepository.GetFirstOrDefaultAsync(p => p.ProductId == id && !p.IsDeleted);

            if (pg != null)
            {
                var pgForreturn = _mapper.Map<ProductDto>(pg);
                model.Message = "Success";
                model.Result = pgForreturn;

                return Ok(model);
            }

            errorModel.Message = "Error";
            errorModel.Result = "خطا در دریافت";
            return BadRequest(errorModel);
        }

        [HttpGet("groups")]
        [ResponseCache(Duration = 600)]
        [ProducesResponseType(typeof(ApiReturn<IEnumerable<ProductGroup>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Groups()
        {
            ApiReturn<IEnumerable<ProductGroup>> model = new ApiReturn<IEnumerable<ProductGroup>> { Status = true };

            var pg = await _db._ProductGroupRepository.GetAllAsync(p => !p.IsDeleted,orderBy: p=>p.OrderBy(i=>i.ProductGroupId),null);
            if (pg != null)
            {
                model.Message = "Success";
                model.Result = pg;

                return Ok(model);
            }

            errorModel.Message = "Error";
            errorModel.Result = "خطا در دریافت";
            return BadRequest(errorModel);
        }

    }
}
