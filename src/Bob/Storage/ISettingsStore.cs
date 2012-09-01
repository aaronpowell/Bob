namespace Bob.Storage
{
    public interface ISettingsStore
    {
        T GetValue<T>(string key);
        bool TryGetValue<T>(string key, out T value);
        bool ContainsKey(string key);
        void SetValue<T>(string key, T value);
        object this[string key] { get; set; }
    }
}