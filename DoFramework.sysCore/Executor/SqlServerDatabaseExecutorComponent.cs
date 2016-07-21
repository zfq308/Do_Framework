using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DoFramework.sysCore
{

    public class SqlServerDatabaseExecutorComponent : ExecutorBase, IComponent, IDBProperties
    {
        #region 建构式

        private SqlConnection _Conn = null;
        private string ConnectionString { get; set; }

        public SqlServerDatabaseExecutorComponent(string connectionString)
        {
            ConnectionString = connectionString;
            _Conn = new SqlConnection(connectionString);
        }

        #endregion

        #region Components Properties
        public string Component_Name
        {
            get
            {
                return "DoFramework.sysCore.SqlServerDatabaseReaderComponent";
            }
        }

        public Version Component_Version
        {
            get
            {
                return new Version(1, 0, 0, 0);
            }
        }

        public string[] Component_DependencyList
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Component_MainCategory
        {
            get
            {
                return "Database Executor";
            }
        }

        public string Component_SubCategory
        {
            get
            {
                return "SQL Server DataScript Executor";
            }
        }

        #endregion

        #region Domain properties

        public string SQLScript
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Enum_SQLType SQLType
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool SupportOutput
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Enum_DatabaseOutputType DatabaseOutputType
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public DatabaseOutputArg databaseOutputArg
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region implement ITask actions

        public override void PostAction()
        {
            throw new NotImplementedException();
        }

        public override object DoAction()
        {
            throw new NotImplementedException();
        }

        public override bool PrepareAction()
        {
            throw new NotImplementedException();
        }

        #endregion

    }

}