using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DoFramework.sysCore
{
    public interface IExecutor
    {
        object Do(params object[] objlist);
    }




}