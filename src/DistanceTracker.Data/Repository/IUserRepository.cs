using DistanceTracker.Data.Model;

namespace DistanceTracker.Data.Repository
{
    public interface IUserRepository: IRepository<User>
    {
        User FindByUserName(string userName);
    }
}
