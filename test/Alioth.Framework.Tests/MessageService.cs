/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Roy Xu
 *
*/

namespace Alioth.Framework.Tests
{
    [ServiceTypeAttribute(typeof(IMessageService), ReferenceType.Singleton)]
    class MessageService : IMessageService
    {
        private string message;

        public string Message
        {
            get { return this.message; }
        }

        public MessageService(string message)
        {
            this.message = message;
        }
    }
}
