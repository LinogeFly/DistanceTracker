using DistanceTracker.Data.Repository;
using System;

namespace DistanceTracker.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IRouteRepository Routes { get; }
        IHistoryRepository History { get; }
        void Complete();
    }
}
