using System;
using System.Collections.Generic;

namespace Orleans.Sagas
{
    class SagaPropertyBag : ISagaPropertyBag
    {
        private Dictionary<string, object> existingProperties;

        public Dictionary<string, object> ContextProperties { get; }

        public SagaPropertyBag(Dictionary<string, object> existingProperties)
        {
            this.existingProperties = existingProperties;
            ContextProperties = new Dictionary<string, object>();
        }

        public void Add(string key, string value)
        {
            ContextProperties.Add(key, value);
        }

        public void Add(string key, Guid value)
        {
            ContextProperties.Add(key, value);
        }

        public void Add(string key, int value)
        {
            ContextProperties.Add(key, value);
        }

        public void Add(string key, long value)
        {
            ContextProperties.Add(key, value);
        }

        public void Add(string key, bool value)
        {
            ContextProperties.Add(key, value);
        }

        public bool GetBool(string key)
        {
            return (bool)existingProperties[key];
        }

        public Guid GetGuid(string key)
        {
            return (Guid)existingProperties[key];
        }

        public int GetInt(string key)
        {
            return (int)existingProperties[key];
        }

        public long GetLong(string key)
        {
            return (long)existingProperties[key];
        }

        public string GetString(string key)
        {
            return (string)existingProperties[key];
        }
    }
}
