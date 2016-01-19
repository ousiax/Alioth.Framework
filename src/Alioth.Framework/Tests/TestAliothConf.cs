/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;
using System.IO;
using System.Reflection;
using Alioth.Framework;
using NUnit.Framework;

namespace Tests {
    [TestFixture]
    class TestAliothConf {
        private IAliothServiceContainer container;

        [SetUp]
        public void Init() {
            container = new AliothServiceContainer();
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(this.GetType(), "servicecontainer.json")) {
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
        public void TestParameterInjection() {
            IMessageService msg = container.GetService<IMessageService>();
            Assert.NotNull(msg.Message);
            Assert.AreEqual("foobar", msg.Message);
        }
    }

    interface IMessageService {
        string Message { get; }
    }

    [ServiceTypeAtrribute(typeof(IMessageService))]
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
