using DistanceTracker.Data.EntityFramework;
using DistanceTracker.Data.UnitOfWork;
using Microsoft.Data.Entity;
using System;
using System.IO;

namespace DistanceTracker.Web.Tests.API
{
    public interface IDbFixture
    {
        IUnitOfWork GetUnitOfWork();
    }

    public class SQLiteDbFixture : IDbFixture, IDisposable
    {
        private bool disposed = false;
        private readonly DataContext DataContext;
        private readonly string DbFileName;

        public SQLiteDbFixture()
        {
            DbFileName = Guid.NewGuid().ToString() + ".dbfixture.sqlite";

            EnsureDbFolderExists();

            var options = new DbContextOptionsBuilder();
            options.UseSqlite(string.Format("Data Source={0}", DbFilePath));

            DataContext = new DataContext(options.Options);
            DataContext.Database.EnsureDeleted();
            DataContext.Database.EnsureCreated();
        }

        public IUnitOfWork GetUnitOfWork()
        {
            return new UnitOfWork(DataContext);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                DataContext.Dispose();
                File.Delete(DbFilePath);
            }

            disposed = true;
        }

        private string DbFolderPath
        {
            get
            {
                return Path.Combine(Path.GetTempPath(), Constants.TestsTempFolderPath);
            }
        }

        private string DbFilePath
        {
            get
            {
                return Path.Combine(DbFolderPath, DbFileName);
            }
        }

        private void EnsureDbFolderExists()
        {
            Directory.CreateDirectory(DbFolderPath);
        }
    }
}
