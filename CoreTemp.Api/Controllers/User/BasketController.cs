﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CoreTemp.Data.DatabaseContext;
using CoreTemp.Data.DTOs.Identity;
using CoreTemp.Data.Models.Basket;
using CoreTemp.Repo.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreTemp.Api.Controllers.User
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/user/basket")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1_User")]
    [Authorize(Policy = "RequiredUserRole")]
    public class BasketController : ControllerBase
    {
        private readonly IUnitOfWork<CoreTempDbContext> _dbMain;
        private readonly IUnitOfWork<BasketDbContext> _dbBasket;
        private readonly IMapper _mapper;
        private ApiReturn<string> errorModel;

        public BasketController(IUnitOfWork<CoreTempDbContext> dbMain, IUnitOfWork<BasketDbContext> dbBasket,
            IMapper mapper)
        {
            _dbMain = dbMain;
            _dbBasket = dbBasket;
            _mapper = mapper;

            errorModel = new ApiReturn<string>
            {
                Status = false,
                Message = "",
                Result = null
            };
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(ApiReturn<IEnumerable<MyBasket>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> List()
        {
            ApiReturn<IEnumerable<MyBasket>> model = new ApiReturn<IEnumerable<MyBasket>> { Status = true };

            var basketItems = await _dbBasket.MyBasketRepository.GetAllAsync(p => p.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier),null);

            if (basketItems != null)
            {
                model.Message = "Success";
                model.Result = basketItems;

                return Ok(model);
            }

            errorModel.Message = "Error";
            errorModel.Result = "خطا در دریافت";
            return BadRequest(errorModel);
        }

        [HttpPut("add/{pid}")]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(long pid)
        {
            ApiReturn<string> model = new ApiReturn<string> { Status = true };
            var product = await _dbMain._ProductRepository.GetByIdAsync(pid);

            var basketIsExist = await _dbBasket.MyBasketRepository.GetFirstOrDefaultAsync(p => p.ProductId == pid && p.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (basketIsExist != null)
            {
                basketIsExist.Count++;
                _dbBasket.MyBasketRepository.Update(basketIsExist);
            }
            else
            {
                if (product == null)
                {
                    errorModel.Message = "ProductError";
                    return BadRequest(errorModel);
                }
                if (product.HaveDiscount && product.DiscountTime > DateTime.Now)
                {
                    MyBasket basket = new MyBasket()
                    {
                        Count = 1,
                        ProductId = pid,
                        ProductPrice = product.ProductPrice,
                        ProductDiscount = product.ProductDiscount,
                        TotalPrice = product.ProductPrice - product.ProductDiscount,
                        UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                    };
                    await _dbBasket.MyBasketRepository.AddAsync(basket);
                }
                else
                {
                    MyBasket basket = new MyBasket()
                    {
                        Count = 1,
                        ProductId = pid,
                        ProductPrice = product.ProductPrice,
                        ProductDiscount = 0,
                        TotalPrice = product.ProductPrice,
                        UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                    };
                    await _dbBasket.MyBasketRepository.AddAsync(basket);
                }

            }


            if (await _dbBasket.SaveAsync() > 0)
            {
                model.Message = "Success";
                model.Result = "سبد خرید به روز رسانی شد";

                return Ok(model);
            }

            errorModel.Message = "Error";
            errorModel.Result = "خطا در به روز رسانی";
            return BadRequest(errorModel);
        }


        [HttpDelete("delete/{pid}")]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(long pid)
        {
            ApiReturn<string> model = new ApiReturn<string> { Status = true };
            var pg = await _dbBasket.MyBasketRepository.GetByIdAsync(pid);
            if (pg != null)
            {
                _dbBasket.MyBasketRepository.Delete(pg);
                if (await _dbBasket.SaveAsync() > 0)
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
