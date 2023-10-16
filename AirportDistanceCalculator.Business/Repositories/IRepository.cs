using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AirportDistanceCalculator.Business.Repositories
{
    public interface IRepository<T>: GenericDatabaseAccess.Repositories.IRepository<T> where T : class
    {
        T? GetFirst(Expression<Func<T, bool>> predicate, bool isReadOnly = false, List<string>? includes = null);
    }
}
