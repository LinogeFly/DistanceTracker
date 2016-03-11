using DistanceTracker.Data.UnitOfWork;
using DistanceTracker.Data.Repository;

namespace DistanceTracker.Data.EntityFramework
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        public UnitOfWork(DataContext context)
        {
            _context = context;

            Users = new UserRepository(_context);
            Routes = new RouteRepository(_context);
            History = new HistoryRepository(_context);
        }

        public IUserRepository Users { get; private set; }
        public IRouteRepository Routes { get; private set; }
        public IHistoryRepository History { get; private set; }

        public void Complete()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
