/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

namespace Alioth.Framework.Tests
{
    interface ICalculatorService : IAdditionService, ISubtractionService
    {
        int X { get; set; }

        int Y { get; set; }
    }
}
