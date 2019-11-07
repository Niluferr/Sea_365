using SeaHub.Models;

namespace SeaHub.DataAccess.Data.Repository.IRepository
{
    public interface IUserRepository:IRepository<ApplicationUser>
    {
        void LockUser(string userId);
        void UnLockUser(string userId);

    }
}
