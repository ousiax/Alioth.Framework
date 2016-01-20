/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

using Alioth.Framework;

namespace Demo1 {
    [ServiceTypeAtrribute(typeof(ICalculatorService), ReferenceType.Singleton)]
    class CalculatorService : ICalculatorService {
        private IAddService add;
        private ISubService sub;

        // property injection
        [DepedencyAtrribute(typeof(IAddService))]
        private IAddService Adder {
            set { add = value; }
        }

        // constructor injection
        public CalculatorService([DepedencyAtrribute(typeof(ISubService))] ISubService sub) {
            this.sub = sub;
        }

        public int Add(int x, int y) {
            return add.Add(x, y);
        }

        public int Sub(int x, int y) {
            return sub.Sub(x, y);
        }
    }
}
