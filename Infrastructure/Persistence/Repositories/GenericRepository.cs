using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class GenericRepository<TEntity, TKay> : IGenericRepository<TEntity, TKay> where TEntity : BaseEntity<TKay>
    {
        private readonly StoreDbContext _context;

        public GenericRepository(StoreDbContext context )
        {
            _context = context;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool trakChanges = false)
        {
            return trakChanges ?
                   await _context.Set<TEntity>().ToListAsync()
                  :await _context.Set<TEntity>().AsNoTracking().ToListAsync();

            //if (trakChanges) return await _context.Set<TEntity>().ToListAsync();
            //return await _context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<TEntity> GetAsync(TKay Id)
        {
            return await _context.Set<TEntity>().FindAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
             await _context.AddAsync(entity);  
        }
        public  void Update(TEntity entity)
        {
             _context.Update(entity);
        }
        public void Delete(TEntity entity)
        {
            _context.Remove(entity); 
        }

    }
}
