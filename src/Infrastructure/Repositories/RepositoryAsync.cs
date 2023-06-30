using BlazorHero.CleanArchitecture.Application.Exceptions;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Contracts;
using BlazorHero.CleanArchitecture.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Infrastructure.Repositories
{
    public class RepositoryAsync<T, TId> : IRepositoryAsync<T, TId> where T : AuditableEntity<TId>, IEntity<TId>
    {
        private readonly BlazorHeroContext _dbContext;
        private DbSet<T> Entity { get; }

        public RepositoryAsync(BlazorHeroContext dbContext)
        {
            _dbContext = dbContext;
            Entity = _dbContext.Set<T>();

        }

        public IQueryable<T> Entities => _dbContext.Set<T>();

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }


        public async Task<T> GetByIdAsync(TId id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetPagedResponseAsync(int pageNumber, int pageSize)
        {
            return await _dbContext
                .Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }



        public int Count()
        {
            return this.Entity.Count();
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return this.Entity.Where(predicate).Count();
        }

        public async Task<int> CountAsync()
        {
            return await this.Entity.CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await this.Entity.Where(predicate).CountAsync();
        }

        public void Delete(T entity)
        {
            this.Entity.Remove(entity);
            this._dbContext.SaveChanges();
        }

        public void Delete(TId id)
        {
            this.Entity.Remove(this.Entity.Find(id));
            this._dbContext.SaveChanges();
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            this.Entity.RemoveRange(this.Entity.Where(predicate));
            this._dbContext.SaveChanges();
        }

        public async Task DeleteAsync(T entity)
        {
            this.Entity.Remove(entity);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TId id)
        {
            this.Entity.Remove(this.Entity.Find(id));
            await this._dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            this.Entity.RemoveRange(this.Entity.Where(predicate));
            await this._dbContext.SaveChangesAsync();
        }

        public T FirstOrDefault(TId id)
        {
            return this.Entity.FirstOrDefault(CreateEqualityExpressionForId(id));
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return this.Entity.FirstOrDefault(predicate);
        }

        public async Task<T> FirstOrDefaultAsync(TId id)
        {
            return await this.Entity.FirstOrDefaultAsync(CreateEqualityExpressionForId(id));
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await this.Entity.FirstOrDefaultAsync(predicate);
        }

        public T Get(TId id)
        {
            return this.Entity.Find(id);
        }

        public IQueryable<T> GetAll()
        {
            return GetAllIncluding();
        }


        public async Task<IQueryable<T>> GetAllAsync()
        {
            return await GetAllIncludingAsync();
        }

        public async Task<IQueryable<T>> GetAllIncludingAsync(
            params Expression<Func<T, object>>[] propertySelectors)
        {
            var query = GetAll();
            //await GetQueryableAsync();

            if (propertySelectors == null || propertySelectors.Length == 0)
            {
                return query;
            }

            foreach (var propertySelector in propertySelectors)
            {
                query = query.Include(propertySelector);
            }

            return query;
        }


        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] propertySelectors)
        {
            var query = this.Entity.AsQueryable();

            if (propertySelectors == null || propertySelectors.Length == 0)
            {
                return query;
            }

            foreach (var propertySelector in propertySelectors)
            {
                query = query.Include(propertySelector);
            }

            return query;
        }

        public List<T> GetAllList()
        {
            return GetAll().ToList();
        }

        public List<T> GetAllList(Expression<Func<T, bool>> predicate)
        {
            return GetAll().Where(predicate).ToList();
        }

        public async Task<List<T>> GetAllListAsync()
        {
            return await GetAll().ToListAsync();
        }

        public async Task<List<T>> GetAllListAsync(Expression<Func<T, bool>> predicate)
        {
            return await GetAll().Where(predicate).ToListAsync();
        }

        public async Task<T> GetAsync(TId id)
        {
            var entity = await FirstOrDefaultAsync(id);
            return entity ?? throw new EntityNotFoundException(typeof(T), id);
        }

        public T Insert(T entity)
        {
            return this.Entity.Add(entity).Entity;
        }

        public TId InsertAndGetId(T entity)
        {
            entity = Insert(entity);
            this._dbContext.SaveChanges();
            return entity.Id;
        }

        public async Task<TId> InsertAndGetIdAsync(T entity)
        {
            entity = await InsertAsync(entity);
            await this._dbContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<T> InsertAsync(T entity)
        {
            var result = (await this.Entity.AddAsync(entity)).Entity;
            await this._dbContext.SaveChangesAsync();
            return result;

        }


        public T Load(TId id)
        {
            return Get(id);
        }

        public long LongCount()
        {
            return GetAll().LongCount();
        }

        public long LongCount(Expression<Func<T, bool>> predicate)
        {
            return GetAll().LongCount(predicate);
        }

        public async Task<long> LongCountAsync()
        {
            return await GetAll().LongCountAsync();
        }

        public async Task<long> LongCountAsync(Expression<Func<T, bool>> predicate)
        {
            return await GetAll().LongCountAsync(predicate);
        }

        //public T Query<T>(Func<IQueryable<T>, T> queryMethod)
        //{
        //    throw new NotImplementedException();
        //}

        public T Single(Expression<Func<T, bool>> predicate)
        {
            return GetAll().Single(predicate);
        }

        public async Task<T> SingleAsync(Expression<Func<T, bool>> predicate)
        {
            return await (await GetAllAsync()).SingleAsync(predicate);
        }

        public T Update(T entity)
        {
            AttachIfNot(entity);
            this._dbContext.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public T Update(TId id, Action<T> updateAction)
        {
            var entity = Get(id);
            updateAction(entity);
            return entity;
        }

        public Task<T> UpdateAsync(T entity)
        {
            return Task.FromResult(Update(entity));
        }

        public async Task<T> UpdateAsync(TId id, Func<T, Task> updateAction)
        {
            var entity = await GetAsync(id);
            await updateAction(entity);
            return entity;
        }


        protected virtual Expression<Func<T, bool>> CreateEqualityExpressionForId(TId id)
        {
            var lambdaParam = Expression.Parameter(typeof(T));

            var leftExpression = Expression.PropertyOrField(lambdaParam, "Id");

            var idValue = Convert.ChangeType(id, typeof(TId));

            Expression<Func<object>> closure = () => idValue;
            var rightExpression = Expression.Convert(closure.Body, leftExpression.Type);

            var lambdaBody = Expression.Equal(leftExpression, rightExpression);

            return Expression.Lambda<Func<T, bool>>(lambdaBody, lambdaParam);
        }


        protected virtual void AttachIfNot(T entity)
        {
            var entry = this._dbContext.ChangeTracker.Entries().FirstOrDefault(ent => ent.Entity == entity);
            if (entry != null)
            {
                return;
            }

            this.Entity.Attach(entity);
        }

    }
}