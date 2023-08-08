using Contracts.Common.Interfaces;
using Contracts.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace Infrastructure.Common
{
    public class RepositoryBase<T, K, TContext> : RepositoryQueryBase<T, K, TContext> ,IRepositoryBase<T, K, TContext>
        where T : EntityBase<K>
        where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly IUnitOfWork<TContext> _unitOfWork;

        public RepositoryBase(TContext context, IUnitOfWork<TContext> unitOfWork): base(context)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return _context.Database.BeginTransactionAsync();
        }

        public K Create(T entity)
        {
            _context.Set<T>().Add(entity);
            return entity.Id;
        }

        public async Task<K> CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await SaveChangesAsync();
            return entity.Id;
        }

        public IList<K> CreateList(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
            return entities.Select(x => x.Id).ToList();
        }

        public async Task<IList<K>> CreateListAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            await SaveChangesAsync();
            return entities.Select(x => x.Id).ToList();
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await SaveChangesAsync();
        }

        public void DeleteList(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public async Task DeleteListAsync(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            await SaveChangesAsync();
        }

        public async Task EndTransactionAsync()
        {
            await SaveChangesAsync();
            await _context.Database.CommitTransactionAsync();
        }



        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync()
        {
            return _unitOfWork.CommitAsync();
        }

        public void Update(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Unchanged) return;
            T? exist = _context.Set<T>().Find(entity.Id);
            if(exist != null)
            _context.Entry(exist).CurrentValues.SetValues(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Unchanged)
                return;
            T? exist = _context.Set<T>().Find(entity.Id);
            if(exist != null)
            _context.Entry(exist).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }

        public void UpdateList(IEnumerable<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
        }

        public async Task UpdateListAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            await SaveChangesAsync();
        }
    }
}
