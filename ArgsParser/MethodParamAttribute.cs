using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgsParser
{
    /// <summary>
    /// Indicates that this method executes if the key is encountered in the input parameters
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class MethodParamAttribute : ParamAttribute
    {
        /// <summary>
        /// Execution priority. 0 - max priority
        /// </summary>
        public int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }
        protected int _priority = int.MaxValue;

        public MethodParamAttribute(string Key) :base(Key)
        {

        }

        public MethodParamAttribute(string Key, string FullName)
            : base(Key, FullName)
        {

        }
    }
}
