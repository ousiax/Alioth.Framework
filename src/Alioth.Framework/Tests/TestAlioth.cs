/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;
using Alioth.Framework;
using NUnit.Framework;

namespace Tests {
    [TestFixture]
    class TestAlioth {
        private IAliothServiceContainer container;

        [SetUp]
        public void Setup() {
            container = new AliothServiceContainer(null);
            container.Apply(typeof(AdditionService));
            container.Apply(typeof(SubtractionService));
            container.Apply(typeof(CalculatorService));
            container.Apply(typeof(CalculatorService2), "calculatorservice2");
            container.Apply(typeof(MultiCtorsService), "MultiCtors");
            container.Apply(typeof(MultiDepedencyCtorsService), "MultiDepedencyCtors");
        }

        [TestCase]
        public void TestGetService() {
            IAdditionService addition = container.GetService(typeof(IAdditionService)) as IAdditionService;
            Assert.IsNotNull(addition, "TestGetService did not work properly.");

            IAdditionService addition2 = container.GetService<IAdditionService>();
            Assert.IsNotNull(addition2, "TestGetService did not work properly.");
        }

        [TestCase]
        public void TestConstructorInjection() {
            ICalculatorService calc = container.GetService(typeof(ICalculatorService), "calculatorservice2", null) as ICalculatorService;
            Assert.IsNotNull(calc, "TestConstructorInjection: service found failure.");

            Assert.AreEqual(2, calc.Add(1, 1), "TestConstructorInjection did not work properly.");

            Assert.AreEqual(2, calc.Subtract(3, 1), "TestConstructorInjection did not work properly.");
        }

        [TestCase]
        public void TestPropertyInjection() {
            ICalculatorService calc = container.GetService(typeof(ICalculatorService)) as ICalculatorService;
            Assert.IsNotNull(calc, "TestPropertyInjection: service found failure.");

            Assert.AreEqual(2, calc.Add(1, 1), "TestPropertyInjection did not work properly.");

            Assert.AreEqual(2, calc.Subtract(3, 1), "TestPropertyInjection did not work properly.");
        }

        [TestCase]
        public void TestMultiCtors() {
            Assert.Throws(typeof(InvalidOperationException), () => {
                this.container.GetService<IAdditionService>("MultiCtors");
            });
            Assert.Throws(typeof(InvalidOperationException), () => {
                this.container.GetService<IAdditionService>("MultiDepedencyCtors");
            });
        }

        [TestCase]
        public void TestRepeatApply() {
            Assert.Throws(typeof(ArgumentException), () => {
                container.Apply(typeof(AdditionService));
            });
        }
    }

    interface IAdditionService {
        int Add(int x, int y);
    }

    interface ISubtractionService {
        int Subtract(int x, int y);
    }

    interface ICalculatorService : IAdditionService, ISubtractionService { }

    [ServiceTypeAtrribute(typeof(IAdditionService))]
    class AdditionService : IAdditionService {
        public int Add(int x, int y) {
            return x + y;
        }
    }

    [ServiceTypeAtrribute(typeof(ISubtractionService))]
    class SubtractionService : ISubtractionService {
        public int Subtract(int x, int y) {
            return x - y;
        }
    }

    [ServiceTypeAtrribute(typeof(ICalculatorService))]
    class CalculatorService : ICalculatorService {
        [DepedencyAtrribute(typeof(IAdditionService))]
        public IAdditionService Addition { get; set; }


        [DepedencyAtrribute(typeof(ISubtractionService))]
        public ISubtractionService Subtraction { get; set; }

        public int Add(int x, int y) {
            return Addition.Add(x, y);
        }

        public int Subtract(int x, int y) {
            return Subtraction.Subtract(x, y);
        }
    }

    [ServiceTypeAtrribute(typeof(ICalculatorService))]
    class CalculatorService2 : ICalculatorService {
        private IAdditionService addition;
        private ISubtractionService subtraction;

        public CalculatorService2(
            [DepedencyAtrribute(typeof(IAdditionService))]IAdditionService addition,
            [DepedencyAtrribute(typeof(ISubtractionService))]ISubtractionService subtraction) {
            this.addition = addition;
            this.subtraction = subtraction;

        }

        public int Add(int x, int y) {
            return addition.Add(x, y);
        }

        public int Subtract(int x, int y) {
            return subtraction.Subtract(x, y);
        }
    }

    [ServiceTypeAtrribute(typeof(IAdditionService))]
    class MultiCtorsService : IAdditionService {
        public MultiCtorsService() {
        }

        public MultiCtorsService(int x) {
        }

        public int Add(int x, int y) {
            throw new NotImplementedException();
        }
    }

    [ServiceTypeAtrribute(typeof(IAdditionService))]
    class MultiDepedencyCtorsService : IAdditionService {
        [DepedencyAtrribute]
        public MultiDepedencyCtorsService() {
        }

        [DepedencyAtrribute]
        public MultiDepedencyCtorsService(int x) {
        }

        public int Add(int x, int y) {
            throw new NotImplementedException();
        }
    }
}
