using AirportDistanceCalculator.Database.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AirportDistanceCalculator.Business.Repositories
{
    public class Repository<T>: GenericDatabaseAccess.Repositories.Repository<T>, IRepository<T> where T:class
    {
        public Repository(AirportDistanceCalculatorDbContext ctx):base(ctx)
        {
            
        }

        public T? GetFirst(Expression<Func<T, bool>> predicate, bool isReadOnly = false, List<string>? includes = null)
        {
            var query = base._ctx.Set<T>().AsQueryable();

            query = PrepareQuery(query, isReadOnly, includes);

            return query.FirstOrDefault(predicate);
        }

       
    }
}
