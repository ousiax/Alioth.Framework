/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

namespace Alioth.Framework.Tests
{
    [ServiceTypeAttribute(typeof(ICalculatorService))]
    class CalculatorService : ICalculatorService
    {
        public int X { get; set; }

        public int Y { get; set; }

        [DepedencyAttribute(typeof(IAdditionService))]
        public IAdditionService Addition { get; set; }


        [DepedencyAttribute(typeof(ISubtractionService))]
        public ISubtractionService Subtraction { get; set; }

        public int Add(int x, int y)
        {
            return Addition.Add(x, y);
        }

        public int Subtract(int x, int y)
        {
            return Subtraction.Subtract(x, y);
        }
    }
}
