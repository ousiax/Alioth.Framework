/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using Alioth.Framework;

namespace ConsoleApp1
{
    [ServiceTypeAtrribute(typeof(IAddService), ReferenceType.Singleton)]
    class AddService : IAddService
    {
        public int Add(int x, int y)
        {
            return x + y;
        }
    }
}
