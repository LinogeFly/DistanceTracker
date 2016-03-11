using DistanceTracker.Data.Model;
using DistanceTracker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistanceTracker.Data.EntityFramework
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {
        }

        public User FindByUserName(string userName)
        {
            return ((DataContext)Context).Users.FirstOrDefault(x => x.UserName.ToLower() == userName.ToLower());
        }
    }
}
