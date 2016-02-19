using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArgsParser
{
    /// <summary>
    /// Allows to parse input parameters
    /// </summary>
    public class ArgsManager
    {
        /// <summary>
        /// Prefix for argument key
        /// </summary>
        public static string KeyPrefix
        {
            get { return _keyPrefix; }
            set
            {
                if (value != null && !string.IsNullOrEmpty(value.Trim()))
                {
                    _keyPrefix = value;
                    return;
                }
                throw new ArgumentNullException("KeyPrefix");
            }
        }
        private static string _keyPrefix = "-";

        public ArgsManager()
        {

        }

        /// <summary>
        /// Parse input arguments and invoke methods
        /// </summary>
        /// <typeparam name="T">The type of object that stores the parameters</typeparam>
        /// <param name="args">Array of arguments</param>
        /// <returns></returns>
        public static T ParseArgs<T>(string[] args)
        {
            return ParseArgs<T>(args, true);
        }

        /// <summary>
        /// Parse input arguments
        /// </summary>
        /// <typeparam name="T">The type of object that stores the parameters</typeparam>
        /// <param name="args">Array of arguments</param>
        /// <param name="autoInvokeMethods">Invoke method </param>
        /// <returns>Object with parsed arguments</returns>
        public static T ParseArgs<T>(string[] args, bool autoInvokeMethods)
        {
            var option = Activator.CreateInstance<T>();

            var dArgs = ArgsToDictionary(args);

            // Get and pars properties
            var props = typeof(T).GetProperties();              //.Where(x => x.GetCustomAttributes<ParamAttribute>().Count() != 0);
            ParseProperties<T>(option, dArgs);

            // Get and parse methods
            var methods = typeof(T).GetMethods();               //.Where(x => x.GetCustomAttributes<ParamAttribute>().Count() != 0);
            var invokeMethods = ParseMethods<T>(option, methods, dArgs);

            // Invoke parsed methods
            if (autoInvokeMethods)
            {
                foreach (var m in invokeMethods.OrderBy(x => x.Key))
                {
                    m.Value.Invoke();
                }
            }

            return option;
        }
        
        private static Dictionary<int, Action> ParseMethods<T>(T option, IEnumerable<MethodInfo> methods, Dictionary<string, string> dArgs)
        {
            var invokeMethods = new Dictionary<int, Action>();
            foreach (var m in methods)
            {
                var atr = m.GetCustomAttribute<MethodParamAttribute>();
                if (atr == null) continue;

                if (dArgs.ContainsKey(atr.Key) || dArgs.ContainsKey(atr.FullName))
                {
                    var mParams = m.GetParameters();
                    var mParamVal = (dArgs[atr.Key] ?? dArgs[atr.FullName]).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (mParams.Count() != mParamVal.Count())
                        throw new Exception("The count of input parameters does not match the method signature!");

                    var resParams = new List<object>(mParams.Count());
                    for (int i = 0; i < mParams.Count(); i++)
                    {
                        resParams.Add(Convert.ChangeType(mParamVal[i], mParams[i].ParameterType));
                    }

                    invokeMethods.Add(atr.Priority, new Action(() => { m.Invoke(option, resParams.ToArray()); }));
                }
            }
            return invokeMethods;
        }

        private static void ParseProperties<T>(T option, Dictionary<string, string> dArgs)
        {
            foreach (var p in option.GetType().GetProperties())
            {
                var atr = p.GetCustomAttribute<PropertyParamAttribute>();

                if (atr == null) continue;

                if (dArgs.ContainsKey(atr.Key) || dArgs.ContainsKey(atr.FullName))
                {
                    if (p.PropertyType == typeof(bool)) // bool
                    {
                        SetPropVal(option, true, p);
                        continue;
                    }
                    else
                    {
                        var key = dArgs.ContainsKey(atr.Key) && !string.IsNullOrEmpty(dArgs[atr.Key])
                                      ? dArgs[atr.Key] : null;
                        var name = dArgs.ContainsKey(atr.FullName) && !string.IsNullOrEmpty(dArgs[atr.FullName])
                                        ? dArgs[atr.FullName] : null;

                        if (p.PropertyType.IsArray) // Collection
                        {
                            var val = key ?? name;

                            SetPropVal(option, val.Split(' ') ?? atr.DefaultValue, p);
                            continue;
                        }
                        else // other types
                        {
                            SetPropVal(option, key ?? name ?? atr.DefaultValue, p);
                        }
                    }
                }
                else
                {
                    if (p.PropertyType == typeof(bool))
                    {
                        SetPropVal(option, atr.DefaultValue ?? false, p);
                    }
                }
            }
        }

        // controlled set value
        private static void SetPropVal(object option, object val, PropertyInfo p)
        {
            if (p.CanWrite && (val) != null)
            {
                p.SetValue(option, Convert.ChangeType(val, p.PropertyType, CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Get input argument value by key
        /// </summary>
        /// <param name="argKey">Key of argument</param>
        /// <param name="args">Array of arguments</param>
        /// <returns>String value of argument</returns>
        public static string GetParamValue(string argKey, string[] args)
        {
            var dArgs = ArgsToDictionary(args);
            return dArgs != null && dArgs.ContainsKey(argKey) ? dArgs[argKey] : null;
        }

        /// <summary>
        /// Get input arguments as a dictionary [key - value]
        /// </summary>
        /// <param name="args">Array of arguments</param>
        /// <returns>Dictionary<argKey, argValue></returns>
        public static Dictionary<string, string> ArgsToDictionary(string[] args)
        {
            var checkList = args.Where(x => x.StartsWith(ArgsManager.KeyPrefix)).ToList();
            if (checkList.Distinct().Count() != checkList.Count)
                throw new Exception("Args list contains non unique keys");

            Dictionary<string, string> _args = new Dictionary<string, string>();

            if (args == null) return _args;

            string argKey = String.Empty;

            foreach (string arg in args)
            {
                //if argument is a key
                if (arg.StartsWith(ArgsManager.KeyPrefix))
                {
                    //push key
                    argKey = arg.Replace(ArgsManager.KeyPrefix, String.Empty);
                    _args.Add(argKey, String.Empty);
                }
                //push value
                else
                {
                    _args[argKey] = string.IsNullOrEmpty(_args[argKey]) ? arg : _args[argKey] + " " + arg;
                }
            }

            return _args;
        }
    }
}
