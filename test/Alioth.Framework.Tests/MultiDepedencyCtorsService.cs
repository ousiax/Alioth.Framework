/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;

namespace Alioth.Framework.Tests
{
    [ServiceTypeAtrribute(typeof(IAdditionService))]
    class MultiDepedencyCtorsService : IAdditionService
    {
        [DepedencyAtrribute]
        public MultiDepedencyCtorsService()
        {
        }

        [DepedencyAtrribute]
        public MultiDepedencyCtorsService(int x)
        {
        }

        public int Add(int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}
