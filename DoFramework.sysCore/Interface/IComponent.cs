using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DoFramework.sysCore
{
    public interface IComponent
    {
        string Component_Name { get;  }
        Version Component_Version { get;}
        string[] Component_DependencyList { get;}
        string Component_MainCategory { get; }
        string Component_SubCategory { get;  }
    }
}