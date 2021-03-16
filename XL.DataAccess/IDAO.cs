using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace XL.DataAccess
{
    public interface IDAO
    {
        T Get<T>(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties) where T : class;
        IList<T> GetList<T>(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties) where T : class;
        bool Exists<T>(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties) where T : class;
        long Count<T>(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties) where T : class;
        void Add<T>(params T[] items) where T : class;
        void Update<T>(params T[] items) where T : class;
        void Save<T>(Expression<Func<T, object>> where, params T[] items) where T : class;
        void Remove<T>(params T[] items) where T : class;
    }
}
