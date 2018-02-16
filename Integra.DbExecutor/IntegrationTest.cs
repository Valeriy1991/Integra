using System;
using DbConn.DbExecutor.Abstract;
using DbConn.DbExecutor.Dapper;

namespace Integra.DbExecutor
{
    /// <summary>
    /// Base class for integration test that will be used IDbExecutor for work with database
    /// </summary>
    public abstract class IntegrationTest
    {
        public IDbExecutorFactory DbExecutorFactory { get; private set; }
        
        public virtual void Init()
        {
            SetDbExecutorFactory();
        }

        /// <summary>
        /// Set DapperDbExecutorFactory as default
        /// </summary>
        protected void SetDbExecutorFactory()
        {
            DbExecutorFactory = new DapperDbExecutorFactory();
        }
        /// <summary>
        /// Set custom IDbExecutorFactory
        /// </summary>
        /// <param name="dbExecutorFactory"></param>
        protected void SetDbExecutorFactory(IDbExecutorFactory dbExecutorFactory)
        {
            DbExecutorFactory = dbExecutorFactory;
        }

        /// <summary>
        /// Create IDbExecutor without transaction open
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        protected IDbExecutor CreateDbExecutor(string connectionString)
        {
            return DbExecutorFactory.Create(connectionString);
        }
        /// <summary>
        /// Create IDbExecutor with transaction open
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        protected IDbExecutor CreateTransactionalDbExecutor(string connectionString)
        {
            return DbExecutorFactory.CreateTransactional(connectionString);
        }
    }
}
