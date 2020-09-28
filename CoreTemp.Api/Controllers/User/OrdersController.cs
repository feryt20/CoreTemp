using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

namespace CoreTemp.Api.Controllers.User
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/user/orders")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1_User")]
    [Authorize(Policy = "RequiredUserRole")]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork<CoreTempDbContext> _dbMain;
        private readonly IUnitOfWork<BasketDbContext> _dbBasket;
        private readonly IMapper _mapper;
        private ApiReturn<string> errorModel;

        public OrdersController(IUnitOfWork<CoreTempDbContext> dbMain, IUnitOfWork<BasketDbContext> dbBasket,
            IMapper mapper)
        {
            _dbMain = dbMain;
            _mapper = mapper;
            _dbBasket = dbBasket;

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

            var order = await _dbMain._OrderRepository.GetAllPagedListAsync(paginationDto, p => !p.IsDeleted && p.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
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

        [HttpGet("get/{oid}")]
        [ProducesResponseType(typeof(ApiReturn<OrderDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(int oid)
        {
            ApiReturn<OrderDto> model = new ApiReturn<OrderDto> { Status = true };

            var order = await _dbMain._OrderRepository.GetAllAsync(p => p.OrderId == oid && p.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier) && !p.IsDeleted, orderBy: o => o.OrderBy(p => p.OrderId), "OrderDetails,User,PaymentLogs");

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


        [HttpPost("finalize")]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Finalize()
        {
            ApiReturn<string> model = new ApiReturn<string> { Status = true };
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var basketItems = await _dbBasket.MyBasketRepository.GetManyAsync(p => p.UserId == userId);

            List<OrderDetail> oddList = new List<OrderDetail>();
            long totalPrice = 0;
            foreach (var item in basketItems)
            {
                totalPrice += item.TotalPrice * item.Count;

                OrderDetail odd = new OrderDetail()
                {
                    ProductId = item.ProductId,
                    OrderedCount = item.Count,
                    Price = item.ProductPrice,
                    Discount = item.ProductDiscount,
                    TotalPrice = item.TotalPrice * item.Count
                };
                oddList.Add(odd);
            }
            Order order = new Order()
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                Price = totalPrice,
                DiscVar = 0,
                DiscCode = "",
                IPAddress = HttpContext.Connection?.RemoteIpAddress.ToString(),
                IsFinalized = false,
                DeliveryDate = DateTime.Now.AddDays(5)
            };
            await _dbMain._OrderRepository.InsertAsync(order);
            if (await _dbMain.SaveAsync() > 0)
            {
                oddList.Select(c => { c.OrderId = order.OrderId; return c; }).ToList();

                await _dbMain._OrderDetailRepository.InsertRangeAsync(oddList);

                if (await _dbMain.SaveAsync() > 0)
                {
                    model.Message = "Success";
                    model.Result = "MyOrders/Get/"+ order.OrderId;

                    return Ok(model);
                }
            }
            

            errorModel.Message = "Error";
            errorModel.Result = "خطا در ثبت سفارش";
            return BadRequest(errorModel);
        }


        
    }
}
