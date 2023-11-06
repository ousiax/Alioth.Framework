NOTE: this project is used as personal study, please use others in production, such as [Microsoft.Extensions.DependencyInjection](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection).

![Build Status](https://github.com/ousiax/Alioth.Framework/actions/workflows/ci.yml/badge.svg?branch=main)

# Alioth Framework

A lightweight IoC container that implemented with .NET.

### A simple demo

**examples/demo1**

***Program.cs***

    using System;
    using Alioth.Framework;
    
    namespace Demo1 {
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

***IAddService.cs***

    namespace Demo1 {
        interface IAddService {
            int Add(int x, int y);
        }
    }

***ISubService.cs***

    namespace Demo1 {
        interface ISubService {
            int Sub(int x, int y);
        }
    }

***ICalculatorService.cs***

    namespace Demo1 {
        interface ICalculatorService : IAddService, ISubService {
        }
    }

***IXYService.cs***

    namespace Demo1 {
        interface IXYService {
            int X { get; }
    
            int Y { get; }
        }
    }

***AddService.cs***

using Alioth.Framework;

    namespace Demo1 {
        [ServiceTypeAtrribute(typeof(IAddService), ReferenceType.Singleton)]
        class AddService : IAddService {
            public int Add(int x, int y) {
                return x + y;
            }
        }
    }

***SubService.cs***

using Alioth.Framework;

    namespace Demo1 {
        [ServiceTypeAtrribute(typeof(ISubService), ReferenceType.Singleton)]
        class SubService : ISubService {
            public int Sub(int x, int y) {
                return x - y;
            }
        }
    }

***CalculatorService.cs***

    using Alioth.Framework;

    namespace Demo1 {
        [ServiceTypeAtrribute(typeof(ICalculatorService), ReferenceType.Singleton)]
        class CalculatorService : ICalculatorService {
            private IAddService add;
            private ISubService sub;
    
            // property injection
            [DepedencyAtrribute(typeof(IAddService))]
            private IAddService Adder {
                set { add = value; }
            }
    
            // constructor injection
            public CalculatorService([DepedencyAtrribute(typeof(ISubService))] ISubService sub) {
                this.sub = sub;
            }
    
            public int Add(int x, int y) {
                return add.Add(x, y);
            }
    
            public int Sub(int x, int y) {
                return sub.Sub(x, y);
            }
        }
    }

***XYService.cs***

    using Alioth.Framework;
    
    namespace Demo1 {
        [ServiceTypeAtrribute(typeof(IXYService))]
        class XYService : IXYService {
            private int x;
            private int y;
    
            public int X {
                get { return x; }
                set { x = value; }
            }
    
            public int Y {
                get { return y; }
            }
    
            public XYService() {
            }
    
            [DepedencyAtrribute]
            public XYService(int y) {
                this.y = y;
            }
        }
    }

***servicecontainer.json***

    {
      "Services": [
        {
          "Type": "Demo1.AddService, Demo1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
          "Name": "",
          "Version": "",
          "Description": ""
        },
        {
          "Type": "Demo1.SubService, Demo1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
          "Name": "",
          "Version": "",
          "Description": ""
        },
        {
          "Type": "Demo1.CalculatorService, Demo1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
          "Name": "",
          "Version": "",
          "Description": ""
        },
        {
          "Type": "Demo1.XYService, Demo1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
          "Name": "",
          "Version": "",
          "Description": "",
          "Properties": {
            "X": "3"
          },
          "Parameters": {
            "y": "4"
          }
        }
      ]
    }

### Relation Topics
* [Inversion of Control Containers and the Dependency Injection
    pattern](http://www.martinfowler.com/articles/injection.html)
