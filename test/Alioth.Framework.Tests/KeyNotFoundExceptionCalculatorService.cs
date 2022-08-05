/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;

namespace Alioth.Framework.Tests
{
    [ServiceTypeAttribute(typeof(ICalculatorService))]
    class KeyNotFoundExceptionCalculatorService : ICalculatorService
    {
        [DepedencyAttribute(typeof(IAdditionService), ServiceName = "fooooobar")]
        public IAdditionService Addition { get; set; }


        [DepedencyAttribute(typeof(ISubtractionService))]
        public ISubtractionService Subtraction { get; set; }

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
            return Addition.Add(x, y);
        }

        public int Subtract(int x, int y)
        {
            return Subtraction.Subtract(x, y);
        }
    }
}
