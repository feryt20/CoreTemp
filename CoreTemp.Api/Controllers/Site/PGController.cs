using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoreTemp.Common.Helpers;
using CoreTemp.Data.DatabaseContext;
using CoreTemp.Data.DTOs.Identity;
using CoreTemp.Data.Models.Site;
using CoreTemp.Repo.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Parbad;

namespace CoreTemp.Api.Controllers.Site
{
    [AllowAnonymous]
    public class PGController : Controller
    {
        private readonly IUnitOfWork<CoreTempDbContext> _dbMain;
        private readonly IUnitOfWork<BasketDbContext> _dbBasket;
        private readonly ILogger<PGController> _logger;
        private readonly IOnlinePayment _onlinePayment;
        private ApiReturn<string> errorModel;
        public PGController(IUnitOfWork<CoreTempDbContext> dbMain, IUnitOfWork<BasketDbContext> dbBasket,
            ILogger<PGController> logger, IOnlinePayment onlinePayment)
        {
            _dbMain = dbMain;
            _dbBasket = dbBasket;
            _logger = logger;
            _onlinePayment = onlinePayment;

            errorModel = new ApiReturn<string>
            {
                Status = false,
                Message = "",
                Result = null
            };
        }

        [HttpGet]
        public async Task<IActionResult> Start(long id)
        {

            var factorFromRepo = await _dbMain._OrderRepository.GetByIdAsync(id);

            if (factorFromRepo == null)
            {
                errorModel.Message = "Error";
                errorModel.Result = "سفارش یافت نشد";
                return View("Pay", errorModel);
            }
            //
            //
            if (factorFromRepo.OrderExpireDate.AddMinutes(10) < DateTime.Now)
            {
                errorModel.Message = "Error";
                errorModel.Result = "زمان تکمیل عملیات پرداخت تمام شده است" ;
                return View("Pay", errorModel);
            }
            if (factorFromRepo.IsFinalized)
            {
                errorModel.Message = "Error";
                errorModel.Result = "پرداخت قبلا به صورت موفق انجام شده است" ;
                return View("Pay", errorModel);
            }

            errorModel.Message = factorFromRepo.FinalPrice.ToPrice();
            errorModel.Result = id.ToString();
            return View("Pay", errorModel);
        }

        [HttpGet]
        public async Task<IActionResult> Pay(long id)
        {

            var factorFromRepo = await _dbMain._OrderRepository.GetByIdAsync(id);

            if (factorFromRepo == null)
            {
                errorModel.Message = "Error";
                errorModel.Result = "سفارش یافت نشد";
                return View(errorModel);
            }
            //
            //
            if (factorFromRepo.OrderExpireDate.AddMinutes(10) < DateTime.Now)
            {
                errorModel.Message = "Error";
                errorModel.Result = "زمان تکمیل عملیات پرداخت تمام شده است";
                return View(errorModel);
            }
            if (factorFromRepo.IsFinalized)
            {
                errorModel.Message = "Error";
                errorModel.Result = "پرداخت قبلا به صورت موفق انجام شده است";
                return View(errorModel);
            }

            var callbackUrl = Url.Action("Verify", "PG", new { id = id }, Request.Scheme);
            var result = await _onlinePayment.RequestAsync(invoice =>
            {
                invoice
                .UseAutoIncrementTrackingNumber()
                .SetAmount(factorFromRepo.FinalPrice)
                .SetCallbackUrl(callbackUrl)
                .UseParbadVirtual();
                //.UseZarinPal("پرداخت از سایت مادپی", "info@madpay724.ir", "09361234567");
            });
            if (result.IsSucceed)
            {
                await result.GatewayTransporter.TransportAsync();
                return new EmptyResult();

            }

            errorModel.Message = "Error";
            errorModel.Result = "پرداخت ناموفق";
            return View(errorModel);
        }
        public async Task<IActionResult> Verify(long id)
        {
            var invoice = await _onlinePayment.FetchAsync();

            var order = await _dbMain._OrderRepository.GetByIdAsync(id);

            var verifyResult = await _onlinePayment.VerifyAsync(invoice);
            if (verifyResult.IsSucceed)
            {
                order.IsFinalized = true;
                PaymentLog paymentLog = new PaymentLog()
                {
                    IsSuccessful = verifyResult.IsSucceed,
                    OrderId = id,
                    PaymentDate = DateTime.Now,
                    PaymentResponseCode = invoice.GatewayName,
                    TrackingCode = invoice.TrackingNumber.ToString(),
                    PaymentResponseMessage = invoice.Message

                };
                _dbMain._OrderRepository.Update(order);
                await _dbMain._PaymentLogRepository.InsertAsync(paymentLog);
                await _dbMain.SaveAsync();

                //clear Basket
                var basketItems = await _dbBasket.MyBasketRepository.GetManyAsync(p => p.UserId == order.UserId);
                _dbBasket.MyBasketRepository.DeleteRange(basketItems);
                await _dbBasket.SaveAsync();

                return Redirect("https://google.com/");
            }
            else
            {
                PaymentLog paymentLog = new PaymentLog()
                {
                    IsSuccessful = verifyResult.IsSucceed,
                    OrderId = id,
                    PaymentDate = DateTime.Now,
                    PaymentResponseCode = invoice.GatewayName,
                    TrackingCode = invoice.TrackingNumber.ToString(),
                    PaymentResponseMessage = invoice.Message

                };
                await _dbMain._PaymentLogRepository.InsertAsync(paymentLog);
                await _dbMain.SaveAsync();
                return Redirect("https://google.com/");
            }
        }
    }
}
