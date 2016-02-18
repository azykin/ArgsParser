﻿using System;
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
    /// Базовый класс для хранения входных параметров. Позволяет отображать справку и примеры
    /// </summary>
    public class ArgsOptionsBase : IArgsHelpViewer
    {
        /// <summary>
        /// Displays help on all the above parameters
        /// </summary>
        /// <returns></returns>
        public virtual string GetParamHelp()
        {
            var props = this.GetType().GetProperties().Where(x => x.GetCustomAttribute<PropertyParamAttribute>() != null).ToArray();

            var methods = this.GetType().GetMethods().Where(x => x.GetCustomAttribute<MethodParamAttribute>() != null).ToArray();

            var showList = new List<Tuple<string, string, string>>();

            foreach (var p in props)
            {
                var atr = p.GetCustomAttribute<PropertyParamAttribute>(); 
                if  (atr == null) continue;

                showList.Add(new Tuple<string, string, string>(atr.Key, atr.Description, atr.Example));
            }

            foreach (var m in methods)
            {
                var atr = m.GetCustomAttribute<MethodParamAttribute>(); 
                if (atr == null) continue;

                showList.Add(new Tuple<string, string, string>(atr.Key, atr.Description, atr.Example));
            }

            StringBuilder paramsSB = new StringBuilder();

            paramsSB.AppendLine(" Parameters:");

            foreach (var k in showList.OrderBy(x => x.Item1))
            {
                paramsSB.AppendLine(String.Format("\t{0}{1, -12}\t{2}", ArgsManager.KeyPrefix, k.Item1, k.Item2));
            }

            return paramsSB.ToString();
        }

        /// <summary>
        /// Shows the examples on all the above parameters
        /// </summary>
        /// <returns></returns>
        public string GetParamExample()
        {
            var props = this.GetType().GetProperties().Where(x => x.GetCustomAttribute<ParamAttribute>() != null);

            var methods = this.GetType().GetMethods().Where(x => x.GetCustomAttribute<ParamAttribute>() != null);

            var showList = new List<Tuple<string, string, string>>();

            foreach (var p in props)
            {
                var atr = p.GetCustomAttribute<PropertyParamAttribute>();
                if (atr == null) continue;

                showList.Add(new Tuple<string, string, string>(atr.Key, atr.Description, atr.Example));
            }

            foreach (var m in methods)
            {
                var atr = m.GetCustomAttribute<MethodParamAttribute>();
                if (atr == null) continue;

                showList.Add(new Tuple<string, string, string>(atr.Key, atr.Description, atr.Example));
            }

            StringBuilder exampSB = new StringBuilder();

            exampSB.AppendLine(" Examples:");

            foreach (var k in showList.OrderBy(x => x.Item1))
            {
                //exampSB.AppendLine(String.Format("> {0} {1}", Process.GetCurrentProcess().ProcessName, k.Item3));
                exampSB.AppendLine(String.Format("\t> {0}", k.Item3));
            }

            return exampSB.ToString();
        }
    }
}
