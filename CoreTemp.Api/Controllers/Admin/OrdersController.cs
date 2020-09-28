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
using CoreTemp.Data.DTOs.Order;
using CoreTemp.Data.Models.Site;
using CoreTemp.Repo.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreTemp.Api.Controllers.Admin
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/admin/orders")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1_Admin")]
    [Authorize(Policy = "RequiredAdminRole")]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork<CoreTempDbContext> _db;
        private readonly IMapper _mapper;
        private readonly ILogger<OrdersController> _logger;
        private ApiReturn<string> errorModel;

        public OrdersController(IUnitOfWork<CoreTempDbContext> dbContext,
            IMapper mapper, ILogger<OrdersController> logger)
        {
            _db = dbContext;
            _mapper = mapper;
            _logger = logger;

            errorModel = new ApiReturn<string>
            {
                Status = false,
                Message = "",
                Result = null
            };
        }


        [HttpGet("list")]
        [ProducesResponseType(typeof(ApiReturn<PagedList<Order>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> List([FromQuery] PaginationDto paginationDto)
        {
            ApiReturn<PagedList<Order>> model = new ApiReturn<PagedList<Order>> { Status = true };

            var order = await _db._OrderRepository.GetAllPagedListAsync(paginationDto, p => !p.IsDeleted);
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

        [HttpGet("get/{id}")]
        [ProducesResponseType(typeof(ApiReturn<OrderDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(int id)
        {
            ApiReturn<OrderDto> model = new ApiReturn<OrderDto> { Status = true };

            var order = await _db._OrderRepository.GetAllAsync(p => p.OrderId == id && !p.IsDeleted,orderBy: o=>o.OrderBy(p=>p.OrderId),"OrderDetails,User,PaymentLogs");

            if (order != null)
            {
                var pgForreturn = _mapper.Map<OrderDto>(order.FirstOrDefault());
                model.Message = "Success";
                model.Result = pgForreturn;

                return Ok(model);
            }

            errorModel.Message = "Error";
            errorModel.Result = "خطا در دریافت";
            return BadRequest(errorModel);
        }

       
    }
}
