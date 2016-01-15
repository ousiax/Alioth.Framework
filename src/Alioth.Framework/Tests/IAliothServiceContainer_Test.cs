// @author T005667
// @since 01/15/2016 19:21:01
//-----------------------------------------------------------------------

using System;
using Alioth.Framework;
using NUnit.Framework;

namespace Tests {
    [TestFixture]
    class IAliothServiceContainer_Test {
        private IAliothServiceContainer container;

        [SetUp]
        public void Setup() {
            container = new AliothServiceContainer(null);
            container.Apply(typeof(AdditionService));
            container.Apply(typeof(SubtractionService));
            container.Apply(typeof(CalculatorService));
        }

        [TestCase]
        public void TestGetService() {
            IAdditionService addition = container.GetService(typeof(IAdditionService)) as IAdditionService;
            Assert.IsNotNull(addition, "TestGetService did not work properly.");

            IAdditionService addition2 = container.GetService<IAdditionService>();
            Assert.IsNotNull(addition2, "TestGetService did not work properly.");
        }

        [TestCase]
        public void TestPropertyInjection() {
            ICalculatorService calc = container.GetService(typeof(ICalculatorService)) as ICalculatorService;
            Assert.IsNotNull(calc, "TestPropertyInjection: service found failure.");

            Assert.AreEqual(2, calc.Add(1, 1), "TestPropertyInjection did not work properly.");

            Assert.AreEqual(2, calc.Subtract(3, 1), "TestPropertyInjection did not work properly.");
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
}
