/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;
using Alioth.Framework;

namespace ConsoleApp1 {
    class Program {
        static void Main(string[] args) {
            IAliothServiceContainer container = new AliothServiceContainer(null) {
                Description = "demo1"
            }.Apply("servicecontainer.json");
            ICalculatorService calc = container.GetService<ICalculatorService>();
            IXYService xy = container.GetService<IXYService>();
            Console.WriteLine("{0} + {1} = {2}", xy.X, xy.Y, calc.Add(xy.X, xy.Y));
            Console.WriteLine("{0} - {1} = {2}", xy.X, xy.Y, calc.Sub(xy.X, xy.Y));
        }
    }
}
