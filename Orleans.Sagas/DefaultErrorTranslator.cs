using System;

namespace Orleans.Sagas
{
    public class DefaultErrorTranslator : IErrorTranslator
    {
        public string Translate(Exception exception)
        {
            return exception?.Message;
        }
    }
}