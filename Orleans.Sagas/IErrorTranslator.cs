using System;

namespace Orleans.Sagas
{
    public interface IErrorTranslator
    {
       string Translate(Exception exception);
    }
}