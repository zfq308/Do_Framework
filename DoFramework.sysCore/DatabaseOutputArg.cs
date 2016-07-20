using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DoFramework.sysCore
{

    public class DatabaseOutputArg
    {
        public string ArgName { set; get; }
        public Enum_DatabaseOutputType DatabaseOutputType { get; set; }
        public string OutputPath { get; set; }
    }

}