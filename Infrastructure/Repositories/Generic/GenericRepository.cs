using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Generic
{
    public interface IGenericRepository<TEntity>
    {
        IQueryable<TEntity> GetAll();

        //Task<TEntity> GetById(int id);
        Task<TEntity> Single(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);

        Task Create(TEntity entity);

        Task Update(int id, TEntity entity);

        //Task Delete(int id);
    }
    public class GenericRepository<TEntity, TContext> : IGenericRepository<TEntity> where TEntity : class where TContext : DbContext
    {
        private readonly TContext _context;

        public GenericRepository(TContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<TEntity> Single(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>()
                        .AsNoTracking()
                        .FirstOrDefaultAsync(predicate);
        }
        public async Task Create(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int id, TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().AsNoTracking();
        }

        public IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>()
                        .AsNoTracking().Where(predicate).ToList();
        }

        //public async Task Delete(int id)
        //{
        //    var entity = await GetById(id);
        //    _context.Set<TEntity>().Remove(entity);
        //    await _context.SaveChangesAsync();
        //}


    }
}
