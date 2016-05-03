/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

namespace Alioth.Framework.Tests
{
    [ServiceTypeAtrribute(typeof(ISubtractionService))]
    class SubtractionService : ISubtractionService
    {
        public int Subtract(int x, int y)
        {
            return x - y;
        }
    }
}
