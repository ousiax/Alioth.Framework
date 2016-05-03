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
    class MultiCtorsService : IAdditionService
    {
        public MultiCtorsService()
        {
        }

        public MultiCtorsService(int x)
        {
        }

        public int Add(int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}
