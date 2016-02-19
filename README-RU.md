# ArgsParser
Парсер входных параметров приложения

Основано на *[CommandLineArgs](https://github.com/azykin/commandlineargs)*

**NuGet:** 

PM> Install-Package ArgsParser

**Использование:** yourapp.exe -f -single singleValue -col val1 val2 val3 

##### Простое использование

Создать класс, который будет хранить параметры. Например:

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
В главном методе приложения нужно вызвать парсер, передав ему тип класса и входные аргументы. Например:
```c#
static void Main(string[] args)
{
    Options options = ArgsParser.ArgsManager.ParseArgs<Options>(args);
}
```
Для доступа к параметра нужно использовать объект, полученный на предыдущем шаге:
```c#
var fileInfo = new FileInfo(options.FilePath);

for (int i = 0; i < options.MaxValue; i++)
{ ... }
```

### Дополнительные возможности

В параметрах можно использовать: 
- Стандартные типы (string, int, double, ...) 
- Массивы ([] - именно массивы а не листы и другие типы коллекций)
- Методы (количество и тип параметров после ключа метода определяется его сигнатурой)

Так же, нужно создать класс, содержащий параметры. Испульзуйте дополнительные поля для описания параметров. Например:
```c#
public class Options : ArgsOptionsBase
{
	[PropertyParamAttribute("path", "FilePath", DefaultValue="data.txt", Description="Файл исходных данных", 
	                                            Example="appname -path datafile.txt")]
	public string FilePath { get; set; }
	
	[PropertyParamAttribute("types", "ResolveFileTypes", DefaultValue = new string[] { ".txt", ".log" }, 
	 Description = "Доступные типы файлов", Example = "appname -types .txt .doc .rtf")]
	public string[] Types { get; set; }
	
	[PropertyParamAttribute("maxval", "MaxValue", DefaultValue=100, Description="Максимальное количество строк",
	                                              Example="appname -maxval 1000")]
	public int MaxValue { get; set; }
	
	[PropertyParamAttribute("log", "UseLoging", DefaultValue = false, Description = "Использовать лог для ошибок",
	                                              Example = "appname -log")]
	public bool UseLog { get; set; }
	
	[MethodParamAttribute("init", "InitDir", Priority=0, Description = "Инициализировать рабочую дирректорию",
	                                              Example = "appname -init appname_data")]
	public void Init(string dirName)
	{
	    Directory.CreateDirectory(dirName);
	}
}
```        
Вы можете использовать базовый класс `ArgsOptionsBase` или интерфейс `IArgsHelpViewer` для использования из коробки или реализации собственных методов отображения справки и примеров. Например:
```c#
Console.WriteLine(options.GetParamHelp());
Console.WriteLine(options.GetParamExample());
```
