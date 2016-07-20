using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DoFramework.sysCore
{
    public abstract class ExecutorBase :  IExecutor
    {
        public virtual object Do(params object[] objlist)
        {
            object obj = null;
            if (PrepareAction())
            {
                Console.WriteLine("PrepareAction executed.");
                obj = DoAction();
                Console.WriteLine("DoAction executed.");
                PostAction();
                Console.WriteLine("PostAction executed.");
                return obj;
            }
            else
            { return obj; }
        }

        public abstract void PostAction();
        public abstract object DoAction();
        public abstract bool PrepareAction();
    }

}