using SeaHub.DataAccess.Data.IRepository;
using System;

namespace SeaHub.DataAccess.Data.Repository.IRepository
{
    public interface IUnitOfWork: IDisposable
    {
        IUserRepository User { get; }
        ICategoryRepository Category { get; }
        IFrequencyRepository Frequency { get; }
        IServiceRepository Service { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IOrderDetailsRepository OrderDetails { get; }

        ISP_Call SP_Call { get; }

        void Save();
    }
}
