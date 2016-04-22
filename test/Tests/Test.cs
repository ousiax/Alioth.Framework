using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Alioth.Framework;
using NUnit.Framework;

namespace Tests {
    [TestFixture]
    class TestAlioth {
        private IAliothServiceContainer container;

        [SetUp]
        public void Setup() {
            container = new AliothServiceContainer();
            using (Stream stream = GetType().GetTypeInfo().Assembly.GetManifestResourceStream("Tests.servicecontainer.json")) {
                container.Apply(stream);
            }
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

            Assert.AreEqual(100, calc.X);
            Assert.AreEqual(200, calc.Y);

            Assert.AreEqual(2, calc.Add(1, 1), "TestPropertyInjection did not work properly.");

            Assert.AreEqual(2, calc.Subtract(3, 1), "TestPropertyInjection did not work properly.");

            Assert.Throws(typeof(KeyNotFoundException), () => {
                container.GetService<ICalculatorService>("KeyNotFoundException");
            });
            Assert.Throws(typeof(KeyNotFoundException), () => {
                container.GetService<ICalculatorService>("PropertyKeyNotFound");
            });

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

        [TestCase]
        public void TestParameterInjection() {
            IMessageService msg = container.GetService<IMessageService>();
            Assert.NotNull(msg.Message);
            Assert.AreEqual("foobar", msg.Message);

            ICalculatorService calc = container.GetService(typeof(ICalculatorService), "ParameterInjection", null) as ICalculatorService;
            Assert.IsNotNull(calc, "TestConstructorInjection: service found failure.");

            Assert.AreEqual(2, calc.Add(1, 1), "TestConstructorInjection did not work properly.");

            Assert.AreEqual(2, calc.Subtract(3, 1), "TestConstructorInjection did not work properly.");
        }

        [TestCase]
        public void TestServiceKeyIgnoreCase() {
            var s1 = container.GetService<IMessageService>("foobar", "foobar");
            Assert.NotNull(s1, "TestServiceKeyIgnoreCase failure.");
            var s3 = container.GetService<IMessageService>("fooBAR", "FOObar");
            Assert.NotNull(s1, "TestServiceKeyIgnoreCase failure.");
        }

        [TestCase]
        public void TestSingletonLifeCycle() {
            var s1 = container.GetService<IMessageService>("foobar", "foobar");
            var s3 = container.GetService<IMessageService>("fooBAR", "FOObar");
            Assert.AreSame(s1, s3, "TestSingletonLifeCycle failure.");
        }
    }

    interface IAdditionService {
        int Add(int x, int y);
    }

    interface ISubtractionService {
        int Subtract(int x, int y);
    }

    interface ICalculatorService : IAdditionService, ISubtractionService {
        int X { get; set; }

        int Y { get; set; }
    }

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
        public int X { get; set; }

        public int Y { get; set; }

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
    class ParameterInjectionCalculatorService : ICalculatorService {
        private IAdditionService addition;
        private ISubtractionService subtraction;

        public ParameterInjectionCalculatorService(
            [DepedencyAtrribute(typeof(IAdditionService))]IAdditionService addition,
            [DepedencyAtrribute(typeof(ISubtractionService))]ISubtractionService subtraction) {
            this.addition = addition;
            this.subtraction = subtraction;

        }

        public int X {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public int Y {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
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

    [ServiceTypeAtrribute(typeof(ICalculatorService))]
    class KeyNotFoundExceptionCalculatorService : ICalculatorService {
        [DepedencyAtrribute(typeof(IAdditionService), ServiceName = "fooooobar")]
        public IAdditionService Addition { get; set; }


        [DepedencyAtrribute(typeof(ISubtractionService))]
        public ISubtractionService Subtraction { get; set; }

        public int X {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public int Y {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public int Add(int x, int y) {
            return Addition.Add(x, y);
        }

        public int Subtract(int x, int y) {
            return Subtraction.Subtract(x, y);
        }
    }

    interface IMessageService {
        string Message { get; }
    }

    [ServiceTypeAtrribute(typeof(IMessageService), ReferenceType.Singleton)]
    class MessageService : IMessageService {
        private string message;

        public string Message {
            get { return this.message; }
        }

        public MessageService(string message) {
            this.message = message;
        }
    }
}
