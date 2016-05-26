/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using Alioth.Framework;

namespace ConsoleApp1
{
    [ServiceTypeAtrribute(typeof(ISubService), ReferenceType.Singleton)]
    class SubService : ISubService
    {
        public int Sub(int x, int y)
        {
            return x - y;
        }
    }
}
