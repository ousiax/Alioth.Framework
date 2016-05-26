/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using Alioth.Framework;

namespace ConsoleApp1
{
    [ServiceTypeAtrribute(typeof(IXYService))]
    class XYService : IXYService
    {
        private int x;
        private int y;

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
        }

        public XYService()
        {
        }

        [DepedencyAtrribute]
        public XYService(int y)
        {
            this.y = y;
        }
    }
}
