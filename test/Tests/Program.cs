/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;
using System.Reflection;
using NUnitLite;

namespace Tests {
    public class Program {
        public static void Main(string[] args) {
#if DNX451
            new AutoRun().Execute(args);
#elif DNXCORE50
        new AutoRun().Execute(typeof(Program).GetTypeInfo().Assembly, Console.Out, Console.In, args);
#endif
        }
    }
}
