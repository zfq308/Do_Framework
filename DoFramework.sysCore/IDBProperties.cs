using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DoFramework.sysCore
{

    public interface IDBProperties
    {
         string SQLScript { set; get; }
         Enum_SQLType SQLType { get; set; }
         bool SupportOutput { get; set; }
         Enum_DatabaseOutputType DatabaseOutputType { get; set; }
         DatabaseOutputArg databaseOutputArg { get; set; }
    }

    public enum Enum_SQLType
    {
        SP = 0,
        Script = 1,
    }

    public enum Enum_DatabaseOutputType
    {
        Memorycache = 0,
        CSVFile = 1,
        ADONetDataSet = 2,
    }
}