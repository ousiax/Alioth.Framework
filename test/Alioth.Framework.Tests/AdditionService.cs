/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

namespace Alioth.Framework.Tests
{
    [ServiceTypeAttribute(typeof(IAdditionService))]
    class AdditionService : IAdditionService
    {
        public int Add(int x, int y)
        {
            return x + y;
        }
    }
}
