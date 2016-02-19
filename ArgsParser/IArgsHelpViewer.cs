using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgsParser
{
    /// <summary>
    /// Interface to display the description and examples
    /// </summary>
    public interface IArgsHelpViewer
    {
        string GetParamHelp();
        string GetParamExample();
    }
}
