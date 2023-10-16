using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GenericDatabaseAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbContext _ctx;

        public Repository(DbContext ctx)
        {
            _ctx = ctx;
        }

        #region Create
        public void Insert(T item)
        {
            _ctx.Set<T>().Add(item);
        }

        public void Insert(List<T> items)
        {
            _ctx.Set<T>().AddRange(items);
        }

        public void Insert(params T[] items)
        {
            _ctx.Set<T>().AddRange(items.ToList());
        }
        #endregion

        #region Read
        public IQueryable<T> GetAllQueryable(Expression<Func<T, bool>> predicate, bool isReadOnly = false, List<string>? includes = null)
        {
            var query = _ctx.Set<T>().AsQueryable();

            query = PrepareQuery(query);

            return query.Where(predicate);
        }

        public List<T> GetAll(Expression<Func<T, bool>> predicate, bool isReadOnly = false, List<string>? includes = null)
        {
            var query = _ctx.Set<T>().AsQueryable();

            query = PrepareQuery(query);

            return query.Where(predicate).ToList();
        }

        public T? GetSingle(Expression<Func<T, bool>> predicate, bool isReadOnly = false, List<string>? includes = null)
        {
            var query = _ctx.Set<T>().AsQueryable();

            query = PrepareQuery(query);

            return query.SingleOrDefault(predicate);
        }

        public bool Exists(Expression<Func<T, bool>> predicate, bool isReadOnly = false, List<string>? includes = null)
        {
            var query = _ctx.Set<T>().AsQueryable();

            query = PrepareQuery(query, isReadOnly, includes);

            return query.Any(predicate);
        }

        #endregion

        #region Delete
        public void Delete(T item)
        {
            _ctx.Remove(item);
        }

        public void Delete(List<T> items)
        {
            _ctx.RemoveRange(items);
        }
        #endregion


        protected IQueryable<T> PrepareQuery(IQueryable<T> query, bool isReadOnly = false, List<string>? includes = null)
        {
            if (isReadOnly)
                query = query.AsNoTracking();

            if (includes != null && includes.Any())
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query;
        }
    }
}
