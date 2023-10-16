using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericDatabaseAccess.UnitOfWorks
{
    public class UnitOfWork<T>: IUnitOfWork where T : DbContext
    {
        private readonly T _ctx;

        public UnitOfWork(T ctx)
        {
            _ctx = ctx;
        }

        public bool SaveChanges()
        {
            using var ctxTransaction = _ctx.Database.BeginTransaction();
            try
            {
                _ctx.SaveChanges();
                ctxTransaction.Commit();
                return true;
            }
            catch (Exception)
            {
                ctxTransaction.Rollback();
                return false;
            }
        }
    }
}
