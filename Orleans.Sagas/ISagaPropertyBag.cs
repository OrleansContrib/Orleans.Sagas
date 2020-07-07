using System;

namespace Orleans.Sagas
{
    public interface ISagaPropertyBag
    {
        void Add(string key, string value);
        void Add(string key, Guid value);
        void Add(string key, int value);
        void Add(string key, long value);
        void Add(string key, bool value);
        string GetString(string key);
        Guid GetGuid(string key);
        int GetInt(string key);
        long GetLong(string key);
        bool GetBool(string key);
    }
}