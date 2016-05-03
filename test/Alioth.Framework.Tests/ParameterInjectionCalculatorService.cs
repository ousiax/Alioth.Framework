/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;

namespace Alioth.Framework.Tests
{
    [ServiceTypeAtrribute(typeof(ICalculatorService))]
    class ParameterInjectionCalculatorService : ICalculatorService
    {
        private IAdditionService addition;
        private ISubtractionService subtraction;

        public ParameterInjectionCalculatorService(
            [DepedencyAtrribute(typeof(IAdditionService))]IAdditionService addition,
            [DepedencyAtrribute(typeof(ISubtractionService))]ISubtractionService subtraction)
        {
            this.addition = addition;
            this.subtraction = subtraction;

        }

        public int X
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int Y
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int Add(int x, int y)
        {
            return addition.Add(x, y);
        }

        public int Subtract(int x, int y)
        {
            return subtraction.Subtract(x, y);
        }
    }
}
