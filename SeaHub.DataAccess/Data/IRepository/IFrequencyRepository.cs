using Microsoft.AspNetCore.Mvc.Rendering;
using SeaHub.Models;
using System.Collections.Generic;

namespace SeaHub.DataAccess.Data.Repository.IRepository
{
    public interface IFrequencyRepository : IRepository<Frequency>
    {
        IEnumerable<SelectListItem> GetFrequencyListDropDown();
        void Update(Frequency frequency);
    }
}
