/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using System;

namespace Alioth.Framework.Tests
{
    [ServiceTypeAttribute(typeof(IAdditionService))]
    class MultiDepedencyCtorsService : IAdditionService
    {
        [DepedencyAttribute]
        public MultiDepedencyCtorsService()
        {
        }

        [DepedencyAttribute]
        public MultiDepedencyCtorsService(int x)
        {
        }

        public int Add(int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}
