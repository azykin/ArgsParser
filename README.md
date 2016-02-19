# ArgsParser
Command line arguments parser for C# applications

Based on *[CommandLineArgs](https://github.com/azykin/commandlineargs)*

**NuGet:** 

PM> Install-Package ArgsParser

**Usage:** yourapp.exe -f -single singleValue -col val1 val2 val3 

## Basic

##### Simple usage

Create a class containing the parameters. For example:

```c#

using ArgsParser;

public class Options
{
	[PropertyParamAttribute("path")]
	public string FilePath { get; set; }
	
	[PropertyParamAttribute("maxval")]
	public int MaxValue { get; set; }
}

```
In main method, call the parser with your class and pass it the arguments. For example:
```c#
static void Main(string[] args)
{
    Options options = ArgsParser.ArgsManager.ParseArgs<Options>(args);
}
```
To access the parameters use the object obtained in the previous step
```c#
var fileInfo = new FileInfo(options.FilePath);

for (int i = 0; i < options.MaxValue; i++)
{ ... }
```

### Advanced usage

In parameters you can use: 
- Standard types 
- Arrays 
- Methods

Also, create a class containing the parameters. Use additional fields for the description of the parameter. For example:

```c#
public class Options : ArgsOptionsBase
{
	[PropertyParamAttribute("path", "FilePath", DefaultValue="data.txt", Description="Source data file", 
	                                            Example="appname -path datafile.txt")]
	public string FilePath { get; set; }
	
	[PropertyParamAttribute("types", "ResolveFileTypes", DefaultValue = new string[] { ".txt", ".log" }, 
	 Description = "Resolves file types", Example = "appname -types .txt .doc .rtf")]
	public string[] Types { get; set; }
	
	[PropertyParamAttribute("maxval", "MaxValue", DefaultValue=100, Description="Max items count",
	                                              Example="appname -maxval 1000")]
	public int MaxValue { get; set; }
	
	[PropertyParamAttribute("log", "UseLoging", DefaultValue = false, Description = "Using log for exceptions",
	                                              Example = "appname -log")]
	public bool UseLog { get; set; }
	
	[MethodParamAttribute("init", "InitDir", Priority=0, Description = "Initialize work directory",
	                                              Example = "appname -init appname_data")]
	public void Init(string dirName)
	{
	    Directory.CreateDirectory(dirName);
	}
}
```        
You can use the base class `ArgsOptionsBase` or the interface `IArgsHelpViewer` to use or implement the methods of display help and examples specified in the attributes. For example:
```c#
Console.WriteLine(options.GetParamHelp());
Console.WriteLine(options.GetParamExample());
```
