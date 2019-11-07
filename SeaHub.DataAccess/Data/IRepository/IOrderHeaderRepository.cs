using Microsoft.AspNetCore.Mvc.Rendering;
using SeaHub.Models;
using System.Collections.Generic;

namespace SeaHub.DataAccess.Data.Repository.IRepository
{
    public interface IOrderHeaderRepository:IRepository<OrderHeader>
    {
        void ChangeOrderStatus(int orderHeaderId, string status);
    }
}
