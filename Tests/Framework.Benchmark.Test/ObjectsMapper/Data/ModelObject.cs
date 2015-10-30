
using System;
namespace Framework.Benchmark.Test.ObjectsMapper
{
    public class ModelObject
    {
        public DateTime BaseDate { get; set; }
        public ModelSubObject Sub { get; set; }
        public ModelSubObject Sub2 { get; set; }
        public ModelSubObject SubWithExtraName { get; set; }
    }

    public class ModelSubObject
    {
        public string ProperName { get; set; }
        public ModelSubSubObject SubSub { get; set; }
    }

    public class ModelSubSubObject
    {
        public string IAmACoolProperty { get; set; }
    }

    public class A2
    {
        public string str1;
        public string str2;
        public string str3;
        public string str4;
        public string str5;
        public string str6;
        public string str7;
        public string str8;
        public string str9;
    }

    public class B2
    {
        public string str1 = "str1";
        public string str2 = "str2";
        public string str3 = "str3";
        public string str4 = "str4";
        public string str5 = "str5";
        public string str6 = "str6";
        public string str7 = "str7";
        public string str8 = "str8";
        public string str9 = "str9";
    }
}
