
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
