// @author T005667
// @since 01/15/2016 19:21:01
//-----------------------------------------------------------------------

using Alioth.Framework;
using NUnit.Framework;

namespace Tests {
    [TestFixture]
    class IAliothServiceContainer_Test {
        private IAliothServiceContainer container;

        [SetUp]
        public void Setup() {
            container = new AliothServiceContainer(null);
            container.Apply(typeof(CalculatorService));
        }

        [TestCase]
        public void TestGetService() {
            ICalculatorService calc = container.GetService(typeof(ICalculatorService)) as ICalculatorService;
            Assert.IsNotNull(calc, "TestGetService: service found failure.");

            ICalculatorService calc2 = container.GetService<ICalculatorService>();
            Assert.IsNotNull(calc2, "TestGetService: service found failure.");
        }
    }

    interface ICalculatorService {
        int Add(int x, int y);

        int Sub(int x, int y);
    }

    [ServiceTypeAtrribute(typeof(ICalculatorService))]
    class CalculatorService : ICalculatorService {
        public int Add(int x, int y) {
            return x + y;
        }

        public int Sub(int x, int y) {
            return x - y;
        }
    }
}
