using Microsoft.AspNetCore.Mvc.Rendering;
using SeaHub.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaHub.DataAccess.Data.Repository.IRepository
{
    public interface IServiceRepository : IRepository<Service>
    {
        void Update(Service service);
    }
}
