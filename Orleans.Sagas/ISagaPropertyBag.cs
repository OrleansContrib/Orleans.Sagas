namespace Orleans.Sagas
{
    public interface ISagaPropertyBag
    {
        void Add<T>(string key, T value);
        T Get<T>(string key);
    }
}