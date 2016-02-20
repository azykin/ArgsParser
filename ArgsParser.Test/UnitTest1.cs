using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArgsParser.Test
{
    public class Option : ArgsOptionsBase
    {
        /* int, double (,.), float (,.), decimal, bool
         * string
         * string[]
         * int[]
         * method 
         */
        [PropertyParamAttribute("i1")]
        public int int1 { get; set; }

        [PropertyParamAttribute("d1", DefaultValue = 9.1)]
        public double double1 { get; set; }
        
        [PropertyParamAttribute("d2", "double2")]
        public double double2 { get; set; }

        [PropertyParamAttribute("f1", "float1")]
        public float float1 { get; set; }

        [PropertyParamAttribute("f2", "float2", DefaultValue=4.1)]
        public float float2 { get; set; }

        [PropertyParamAttribute("d1", "dec")]
        public decimal dec { get; set; }

        [PropertyParamAttribute("b1", "bool")]
        public bool Bool1 { get; set; }
        
        [PropertyParamAttribute("b2", "bool_false")]
        public bool Bool2 { get; set; }

        [PropertyParamAttribute("str", "string")]
        public string str { get; set; }

        [PropertyParamAttribute("arrstr", "string")]
        public string[] arrStr { get; set; }

        [PropertyParamAttribute("arrint", "intArray")]
        public int[] arrInt { get; set; }

        [MethodParamAttribute("m1")]
        public void method1()
        {
            bm1 = true;
        }
        public static bool bm1 = false;

        [MethodParamAttribute("m2")]
        public void method2(int i1, string s1, string s2)
        {
            bm2 = true;
        }
        public static bool bm2 = false;
    }

    public class opt1
    {
        [PropertyParamAttribute("i1")]
        public int int1 { get; set; }
    }

    [TestClass]
    public class UnitTest1
    {
        // определить где в коде могут быть ошибки и разбить методы
        // по классам этих ошибок

        /* 1 (Key, null) name = key
         * 2 Смена KeyPrefix
         * 3 AutoInvoke
         * 4 Приоритет методов
         * 5 Ключ по Key и по Name
         * 6 Сигнатура методов 
         * 7 Подсунуть не поддерживаемый тип
         * 8 Записать дефолтный null в значимый тип
         */

        [TestMethod]
        public void TestMethod1()
        {
            var args = "".Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            var opt = ArgsManager.ParseArgs<opt1>(args);
        }
    }
}
