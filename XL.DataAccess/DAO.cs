using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace XL.DataAccess
{
    public class DAO : IDAO
    {
        #region Properties

        private readonly string _dbContextName;

        #endregion

        #region Ctor

        public DAO() : this(ConfigurationManager.ConnectionStrings.Count - 1) { }

        public DAO(int i)
        {
            _dbContextName = ConfigurationManager.ConnectionStrings[i].Name;
        }

        public DAO(string nameOrConnectionString)
        {
            _dbContextName = nameOrConnectionString;
        }

        #endregion

        #region Get

        public virtual T Get<T>(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties) where T : class
        {
            T item;

            using (var db = new DbCtx(_dbContextName))
            {
                item = Get<T>(db, where, navigationProperties);
            }

            return item;
        }

        private T Get<T>(DbCtx db, Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties) where T : class
        {
            IQueryable<T> query = db.GetQuery<T>(where, navigationProperties);

            return query.FirstOrDefault();
        }

        #endregion

        #region GetList

        public virtual IList<T> GetList<T>(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties) where T : class
        {
            IList<T> list;

            using (var db = new DbCtx(_dbContextName))
            {
                list = GetList<T>(db, where, navigationProperties);
            }

            return list;
        }

        private IList<T> GetList<T>(DbCtx db, Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties) where T : class
        {
            IQueryable<T> query = db.GetQuery<T>(where, navigationProperties);

            return query.ToList<T>();
        }

        #endregion

        #region GetPaginated

        public virtual PaginatedResult<T> GetPaginated<T>(int page, int limit, Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties) where T : class
        {
            PaginatedResult<T> result = new PaginatedResult<T>();

            using (var db = new DbCtx(_dbContextName))
            {
                result.Total = Count<T>(db, where, navigationProperties);
                result.Items = GetPaginated<T>(db, page, limit, where, navigationProperties);
                result.Page = page;
                result.Limit = limit;
            }

            return result;
        }

        private IList<T> GetPaginated<T>(DbCtx db, int page, int limit, Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties) where T : class
        {
            IQueryable<T> query = db.GetQuery<T>(where, navigationProperties);

            return query.Skip(page * limit).Take(limit).ToList<T>();
        }

        #endregion

        #region Search

        public virtual IList<T> Search<T>(string term, params Expression<Func<T, object>>[] navigationProperties) where T : class
        {
            IList<T> list;

            using (var db = new DbCtx(_dbContextName))
            {
                var objectSet = ((IObjectContextAdapter)db).ObjectContext.CreateObjectSet<T>();

                    foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    objectSet = objectSet.Include<T, object>(navigationProperty);

                    .in
                    .ToTraceString();
                list = null;

               // list = Search<T>(db, term, navigationProperties);
            }

            return list;
        }

        #endregion

        #region Add

        public virtual void Add<T>(params T[] items) where T : class
        {
            using (var db = new DbCtx(_dbContextName))
            {
                Add<T>(db, items);
            }
        }

        private void Add<T>(DbCtx db, params T[] items) where T : class
        {
            db.SetEntryState<T>(EntityState.Added, items);

            db.SaveChanges();
        }

        #endregion

        #region Update

        public virtual void Update<T>(params T[] items) where T : class
        {
            using (var db = new DbCtx(_dbContextName))
            {
                Update<T>(db, items);
            }
        }

        private void Update<T>(DbCtx db, params T[] items) where T : class
        {
            db.SetEntryState<T>(EntityState.Modified, items);

            db.SaveChanges();
        }

        #endregion

        #region Save

        public virtual void Save<T>(Expression<Func<T, object>> where, params T[] items) where T : class
        {
            using (var db = new DbCtx(_dbContextName))
            {
                Save<T>(db, where, items);
            }
        }

        private void Save<T>(DbCtx db, Expression<Func<T, object>> where, params T[] items) where T : class
        {
            DbSet<T> dbSet = db.Set<T>();

            dbSet.AddOrUpdate(where, items);

            db.SaveChanges();
        }

        #endregion

        #region Remove

        public virtual void Remove<T>(params T[] items) where T : class
        {
            using (var db = new DbCtx(_dbContextName))
            {
                Remove<T>(db, items);
            }
        }

        private void Remove<T>(DbCtx db, params T[] items) where T : class
        {
            db.SetEntryState<T>(EntityState.Deleted, items);

            db.SaveChanges();
        }

        #endregion

        #region Aggregate functions

        #region Exists

        public virtual bool Exists<T>(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties) where T : class
        {
            bool exists;

            using (var db = new DbCtx(_dbContextName))
            {
                exists = Exists<T>(db, where, navigationProperties);
            }

            return exists;
        }

        private bool Exists<T>(DbCtx db, Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties) where T : class
        {
            IQueryable<T> query = db.GetQuery<T>(where, navigationProperties);

            return query.Any();
        }

        #endregion

        #region Count

        public virtual long LongCount<T>(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties) where T : class
        {
            long count = 0;
            using (var db = new DbCtx(_dbContextName))
            {
                count = LongCount<T>(db, where, navigationProperties);
            }
            return count;
        }

        private long LongCount<T>(DbCtx db, Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties) where T : class
        {
            IQueryable<T> query = db.GetQuery<T>(where, navigationProperties);

            return query.LongCount<T>();
        }

        public virtual int Count<T>(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties) where T : class
        {
            int count = 0;
            using (var db = new DbCtx(_dbContextName))
            {
                count = Count<T>(db, where, navigationProperties);
            }
            return count;
        }

        private int Count<T>(DbCtx db, Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties) where T : class
        {
            IQueryable<T> query = db.GetQuery<T>(where, navigationProperties);

            return query.Count<T>();
        }

        #endregion

        #region Avg

        public virtual decimal Avg<T>(Expression<Func<T, bool>> where, Expression<Func<T, decimal>> selector, params Expression<Func<T, object>>[] navigationProperties) where T : class
        {
            decimal avg = 0;
            using (var db = new DbCtx(_dbContextName))
            {
                avg = Avg<T>(db, where, selector, navigationProperties);
            }
            return avg;
        }

        private decimal Avg<T>(DbCtx db, Expression<Func<T, bool>> where, Expression<Func<T, decimal>> selector, params Expression<Func<T, object>>[] navigationProperties) where T : class
        {
            IQueryable<T> query = db.GetQuery<T>(where, navigationProperties);

            return query.Average(selector);
        }

        #endregion

        #region Max

        public virtual double Max<T>(Expression<Func<T, bool>> where, Expression<Func<T, int>> selector, params Expression<Func<T, object>>[] navigationProperties) where T : class
        {
            double max = 0;
            using (var db = new DbCtx(_dbContextName))
            {
                max = Max<T>(db, where, selector, navigationProperties);
            }
            return max;
        }

        private int Max<T>(DbCtx db, Expression<Func<T, bool>> where, Expression<Func<T, int>> selector, params Expression<Func<T, object>>[] navigationProperties) where T : class
        {
            IQueryable<T> query = db.GetQuery<T>(where, navigationProperties);

            return query.Max(selector);
        }

        #endregion

        #region Min

        #endregion

        #region Sum

        #endregion

        #endregion

        #region Execute

        public virtual IList<T> Execute<T>(string query, params SqlParameter[] parameters) where T : class
        {
            using (var db = new DbCtx(_dbContextName))
            {
                return Execute<T>(db, query);
            }
        }

        private IList<T> Execute<T>(DbCtx db, string query, params SqlParameter[] parameters) where T : class
        {
            return db.Database
                .SqlQuery<T>(query, parameters ?? new SqlParameter[] { })
                .ToList();
        }

        public virtual QueryResult Execute(string query, params SqlParameter[] parameters)
        {
            QueryResult queryResult = new QueryResult();

            using (var db = new DbCtx(_dbContextName))
            {
                var connection = (SqlConnection)db.Database.Connection;

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters
                        .AddRange(parameters ?? new SqlParameter[] { });

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable schemaTable = reader.GetSchemaTable();

                        var columns = new List<Column>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            columns.Add(new Column()
                            {
                                Value = reader.GetName(i),
                                SqlType = reader.GetDataTypeName(i),
                                Type = reader.GetFieldType(i)
                            });
                        }

                        List<List<Column>> rows = new List<List<Column>>();

                        while (reader.Read())
                        {
                            object[] tempRow = new object[reader.FieldCount];

                            List<Column> row = new List<Column>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row.Add(new Column()
                                {
                                    Value = reader.GetValue(i),
                                    SqlType = reader.GetDataTypeName(i),
                                    Type = reader.GetFieldType(i)
                                });
                            }

                            rows.Add(row);
                        }

                        queryResult.Columns = columns;
                        queryResult.Rows = rows;
                        queryResult.SQLQuery = query;
                    }
                }
            }

            return queryResult;
        }

        #endregion
    }
}
