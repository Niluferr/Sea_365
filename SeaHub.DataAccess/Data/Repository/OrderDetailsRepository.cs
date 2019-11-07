using Microsoft.AspNetCore.Mvc.Rendering;
using SeaHub.DataAccess.Data.Repository.IRepository;
using SeaHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeaHub.DataAccess.Data.Repository
{
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

    }

}

