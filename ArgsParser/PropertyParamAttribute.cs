using System;

namespace ArgsParser
{
    /// <summary>
    /// Specifies that this property can be set from the input parameters
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class PropertyParamAttribute : ParamAttribute
    {
        /// <summary>
        /// The value that is set if not specified
        /// </summary>
        public object DefaultValue { get; set; }

        public PropertyParamAttribute(string Key) : base(Key)
        {
          
        }
        public PropertyParamAttribute(string Key, string FullName)
            : base(Key, FullName)
        {

        }
    }
}
