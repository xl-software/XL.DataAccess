using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace XL.DataAccess
{
    public class DbCtx : DbContext
    {
        public DbCtx()
        {
            Config();
        }

        public DbCtx(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Config();
        }

        public void Config()
        {
            // Forzamos copiar EntityFramework.SqlServer.dll cuando otro proyecto lo referencie
            _ = System.Data.Entity.SqlServer.SqlProviderServices.Instance;

            // Deshabilitamos Lazy Loading
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;

            // Consulta por consola
            #if DEBUG
            this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
            #endif
        }

        public IQueryable<T> GetQuery<T>(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties) where T : class
        {
            IQueryable<T> dbQuery = this.Set<T>();

            foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                dbQuery = dbQuery.Include<T, object>(navigationProperty);

            return dbQuery
                .AsNoTracking()
                .Where(where);
        }

        public void SetEntryState<T>(EntityState state, params T[] items) where T : class
        {
            foreach (T item in items)
                this.Entry(item).State = state;
        }
    }
}
