﻿using Domain.Contracts;
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
    public class GenericRepository<TEntity, Tkey> : IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        private readonly StoreDbContext _context;

        public GenericRepository(StoreDbContext context )
        {
            _context = context;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool trakChanges = false)
        {
            if (typeof(TEntity) == typeof(Product))
            {
                return trakChanges ?
                   await _context.Products.Include(P => P.ProductType).Include(P => P.ProductBrand).ToListAsync() as IEnumerable<TEntity>
                  : await _context.Products.Include(P => P.ProductType).Include(P => P.ProductBrand).AsNoTracking().ToListAsync() as IEnumerable<TEntity>;
            }
        
            
            return trakChanges ?
                   await _context.Set<TEntity>().ToListAsync()
                  :await _context.Set<TEntity>().AsNoTracking().ToListAsync();

            //if (trakChanges) return await _context.Set<TEntity>().ToListAsync();
            //return await _context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<TEntity> GetAsync(Tkey Id)
        {
            if (typeof(TEntity) == typeof(Product))
            {
                // return await _context.Products.Include(P => P.ProductType).Include(P => P.ProductBrand).FirstOrDefaultAsync( P => P.Id ==Id as int?) as TEntity;
                return await _context.Products.Where(P => P.Id == Id as int?).Include(P => P.ProductType).Include(P => P.ProductBrand).FirstOrDefaultAsync () as TEntity;

            }
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

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity, Tkey> spec, bool trakChanges = false)
        {
           return await ApplySpecifications(spec).ToListAsync();
        }

        public async Task<TEntity?> GetAsync(ISpecifications<TEntity, Tkey> spec)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();
        }

        public async Task<int> CountAsync(ISpecifications<TEntity, Tkey> spec)
        {
            return await ApplySpecifications(spec).CountAsync();
        }
        private IQueryable<TEntity> ApplySpecifications (ISpecifications<TEntity,Tkey> spec)
        {
            return  SpecificationEvaluator.GetQuery(_context.Set<TEntity>(), spec);
        }

        
    }
}
