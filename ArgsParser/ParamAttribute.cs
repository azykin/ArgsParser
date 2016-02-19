using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgsParser
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ParamAttribute : Attribute
    {
        public ParamAttribute(string Key)
        {
            this.Key = this.FullName = Key;
        }

        public ParamAttribute(string Key, string FullName)
        {
            this.Key = Key;
            this.FullName = FullName;
        }

        /// <summary>
        /// Key of argument
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Full name of argument
        /// </summary>
        public string FullName
        {
            get { return _fullName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException("FullName");
                if (value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Count() > 1)
                    throw new ArgumentException("FullName attribute must be a single word!");
                _fullName = value;
            }
        }
        private string _fullName;

        /// <summary>
        /// The argument description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// An example of using the argument
        /// </summary>
        public string Example { get; set; }
    }
}
