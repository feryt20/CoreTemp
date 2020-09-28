using CoreTemp.Repo.Repositories.Basket.Interface;
using CoreTemp.Repo.Repositories.Main.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreTemp.Repo.Infrastructure
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
    {
        IUserRepository _UserRepository { get; }
        ITokenRepository _TokenRepository { get; }
        IVerificationCodeRepository _VerificationCodeRepository { get; }

        IProductGroupRepository _ProductGroupRepository { get; }
        IProductRepository _ProductRepository { get; }
        IOrderRepository _OrderRepository { get; }
        IOrderDetailRepository _OrderDetailRepository { get; }
        IPaymentLogRepository _PaymentLogRepository { get; }
        ISliderRepository _SliderRepository { get; }

        IMyBasketRepository MyBasketRepository { get; }

        void Save();
        Task<int> SaveAsync();
    }
}
