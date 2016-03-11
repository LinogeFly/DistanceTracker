using DistanceTracker.Data.Model;
using DistanceTracker.Data.Repository;
using System.Security.Claims;

namespace DistanceTracker.API
{
    public static class UserRepositoryExtensions
    {
        public static User CurrentUser(this IUserRepository userRepository, ClaimsPrincipal principal)
        {
            return userRepository.FindByUserName(principal.Identity.Name);
        }
    }
}
