using Microsoft.AspNetCore.Mvc.Rendering;
using SeaHub.DataAccess.Data.Repository.IRepository;
using SeaHub.Models;
using System.Collections.Generic;
using System.Linq;

namespace SeaHub.DataAccess.Data.Repository
{
    public class FrequencyRepository : Repository<Frequency>, IFrequencyRepository
    {
        private readonly ApplicationDbContext _db;
        public FrequencyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetFrequencyListDropDown()
        {
            return _db.Frequency.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }

        public void Update(Frequency frequency)
        {
            var objFromDb = _db.Frequency.FirstOrDefault(s => s.Id == frequency.Id);
            objFromDb.Name = frequency.Name;
            objFromDb.FrequencyCount = frequency.FrequencyCount;
            _db.SaveChanges();
        }
    }
}

