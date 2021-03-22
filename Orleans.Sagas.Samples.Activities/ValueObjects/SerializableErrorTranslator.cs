using Newtonsoft.Json;
using System;

namespace Orleans.Sagas.Samples.Activities.ValueObjects
{
    public class SerializableErrorTranslator : IErrorTranslator
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        public string Translate(Exception exception)
        {
            if (exception == null)
            {
                return null;
            }

            return JsonConvert.SerializeObject(exception, _jsonSerializerSettings);
        }

        public Exception TranslateBack(string message)
        {
            if (message == null)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<Exception>(message, _jsonSerializerSettings);
        }
    }
}