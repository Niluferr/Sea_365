using Microsoft.AspNetCore.Mvc.Rendering;
using SeaHub.Models;
using System.Collections.Generic;

namespace SeaHub.DataAccess.Data.Repository.IRepository
{
    public interface ICategoryRepository:IRepository<Category>
    {
        IEnumerable<SelectListItem> GetCategoryListDropDown();
        void Update(Category category);

    }
}
