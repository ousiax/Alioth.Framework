/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Xunit;

namespace Alioth.Framework.Tests

{
    public class Test_Alioth_Framework

    {
        private IAliothServiceContainer container;

        public Test_Alioth_Framework()

        {
            container = new AliothServiceContainer();
            using (Stream stream = GetType().GetTypeInfo().Assembly.GetManifestResourceStream($"{this.GetType().GetTypeInfo().Namespace}.servicecontainer.json"))

            {
                container.Apply(stream);
            }
        }

        [Fact]
        public void TestGetService()

        {
            IAdditionService addition = container.GetService(typeof(IAdditionService)) as IAdditionService;
            Assert.NotNull(addition);

            IAdditionService addition2 = container.GetService<IAdditionService>();
            Assert.NotNull(addition2);
        }

        [Fact]
        public void TestPropertyInjection()

        {
            ICalculatorService calc = container.GetService(typeof(ICalculatorService)) as ICalculatorService;
            Assert.NotNull(calc);

            Assert.Equal(100, calc.X);
            Assert.Equal(200, calc.Y);

            Assert.Equal(2, calc.Add(1, 1));

            Assert.Equal(2, calc.Subtract(3, 1));

            Assert.Throws(typeof(KeyNotFoundException), () =>

                {
                    container.GetService<ICalculatorService>("KeyNotFoundException");
                });
            Assert.Throws(typeof(KeyNotFoundException), () =>

                {
                    container.GetService<ICalculatorService>("PropertyKeyNotFound");
                });

        }

        [Fact]
        public void TestMultiCtors()

        {
            Assert.Throws(typeof(InvalidOperationException), () =>

                {
                    this.container.GetService<IAdditionService>("MultiCtors");
                });
            Assert.Throws(typeof(InvalidOperationException), () =>

                {
                    this.container.GetService<IAdditionService>("MultiDepedencyCtors");
                });
        }

        [Fact]
        public void TestRepeatApply()

        {
            Assert.Throws(typeof(ArgumentException), () =>

                {
                    container.Apply(typeof(AdditionService));
                });
        }

        [Fact]
        public void TestParameterInjection()

        {
            IMessageService msg = container.GetService<IMessageService>();
            Assert.NotNull(msg.Message);
            Assert.Equal("foobar", msg.Message);

            ICalculatorService calc = container.GetService(typeof(ICalculatorService), "ParameterInjection", null) as ICalculatorService;
            Assert.NotNull(calc);

            Assert.Equal(2, calc.Add(1, 1));

            Assert.Equal(2, calc.Subtract(3, 1));
        }

        [Fact]
        public void TestServiceKeyIgnoreCase()

        {
            var s1 = container.GetService<IMessageService>("foobar", "foobar");
            Assert.NotNull(s1);
            var s3 = container.GetService<IMessageService>("fooBAR", "FOObar");
            Assert.NotNull(s1);
        }

        [Fact]
        public void TestSingletonLifeCycle()

        {
            var s1 = container.GetService<IMessageService>("foobar", "foobar");
            var s3 = container.GetService<IMessageService>("fooBAR", "FOObar");
            Assert.Same(s1, s3);
        }
    }
}
