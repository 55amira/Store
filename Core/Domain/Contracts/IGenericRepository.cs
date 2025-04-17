using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IGenericRepository <TEntity,TKey> where TEntity : BaseEntity<TKey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync( bool trakChanges = false);
        Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity,TKey> spec,bool trakChanges = false);
        Task<TEntity> GetAsync(TKey Id);
        Task<TEntity> GetAsync(ISpecifications<TEntity, TKey> spec, TKey Id);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
