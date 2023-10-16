using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GenericDatabaseAccess.Repositories
{
    public interface IRepository<T> where T : class
    {
        void Insert(T item);

        void Insert(List<T> items);

        void Insert(params T[] items);

        IQueryable<T> GetAllQueryable(Expression<Func<T, bool>> predicate, bool isReadOnly = false, List<string>? includes = null);

        List<T> GetAll(Expression<Func<T, bool>> predicate, bool isReadOnly = false, List<string>? includes = null);

        T? GetSingle(Expression<Func<T, bool>> predicate, bool isReadOnly = false, List<string>? includes = null);

        bool Exists(Expression<Func<T, bool>> predicate, bool isReadOnly = false, List<string>? includes = null);

        void Delete(T item);

        void Delete(List<T> items);
    }
}
