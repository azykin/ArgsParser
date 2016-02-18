using ArgsParser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ArgsParserConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = ArgsManager.ParseArgs<Options>(args, true);
            Console.WriteLine(options.GetParamHelp());
            Console.WriteLine(options.GetParamExample());

            var param = ArgsManager.GetParamValue("d", args);

            Console.ReadLine();
        }
    }

    public class Options : ArgsOptionsBase
    {
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        // -path file1.txt -f -int 12 -d 99.9 -fr .ppt .pptx .pdf -ppp

        [PropertyParamAttribute("path", "file_path", DefaultValue = "def", Description = "path of file", Example = "f file.txt")]
        public string File { get; set; }

        [PropertyParamAttribute("f", "Flag", DefaultValue = false, Description = "Проверка типа флага", Example = "fl")]
        public bool Flag { get; set; }

        [PropertyParamAttribute("int", "Integer", DefaultValue = "10", Description = "Проверка числа", Example = "t 1234")]
        public int IntValue { get; set; }

        [PropertyParamAttribute("d", "Double", DefaultValue = "10,4", Description = "Число с запятой", Example = "d 9.5 or d 9,5")]
        public double Dub { get; set; }

        [PropertyParamAttribute("bf", "BadFormats", Description = "Используемые форматы (выдаст ошибку)", Example = "bf .pptx .ppt .doc .pdf")]
        public List<string> Formats { get; set; }

        [PropertyParamAttribute("fr", "Formats", Description = "Правильные используемые форматы", Example = "fr .pptx .ppt .doc .pdf")]
        public string[] Formats2 { get; set; }

        [MethodParamAttribute(FullName: "Help", Key: "?", Description = "help for this programm", Priority = 0, Example = "? 100 asdf")]
        public void help(int aaa, string bbb)
        {
            Console.WriteLine("Help: {0} _ {1}", aaa, bbb);
            base.GetParamHelp();
        }

        [MethodParamAttribute(FullName: "hl", Key: "hl", Description = "help for this programm", Priority = 1, Example = "hl")]
        public void examp()
        {
            base.GetParamExample();
        }

        [MethodParamAttribute("h", "Hide", Description = "Hide console window", Priority = 0, Example = "h - console window not will be showed")]
        public void hide()
        {
            IntPtr h = Process.GetCurrentProcess().MainWindowHandle;
            ShowWindow(h, 0);
        }

    }


}
