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

}