using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbContext;

        private Dictionary<string,object> _repositories;

        public UnitOfWork(StoreContext dbContext)//ask CLR to create object from Dbcontext implicitly
        {
            _dbContext = dbContext;
            _repositories = new();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var key = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(key))
            {
                var repository = new GenericRepository<TEntity>(_dbContext);

                _repositories.Add(key, repository) ;
            }
            return (IGenericRepository<TEntity>)_repositories[key];
        }

        public async Task<int> CompleteAsync()
            => await _dbContext.SaveChangesAsync();
        

        public async ValueTask DisposeAsync()
            => await _dbContext.DisposeAsync();

       
    }
}
