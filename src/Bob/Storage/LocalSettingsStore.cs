using Windows.Storage;

namespace Bob.Storage
{
    public class LocalSettingsStore : ISettingsStore
    {
        private readonly ApplicationDataContainer localStorage;

        public LocalSettingsStore()
        {
            localStorage = ApplicationData.Current.LocalSettings;
        }

        public T GetValue<T>(string key)
        {
            return (T)localStorage.Values[key];
        }

        public bool TryGetValue<T>(string key, out T value)
        {
            object val;
            value = default(T);

            var result = localStorage.Values.TryGetValue(key, out val);

            if (result)
                value = (T) val;

            return result;
        }

        public bool ContainsKey(string key)
        {
            return localStorage.Values.ContainsKey(key);
        }

        public void SetValue<T>(string key, T value)
        {
            localStorage.Values.Add(key, value);
        }

        public object this[string key]
        {
            get { return localStorage.Values[key]; }
            set { localStorage.Values[key] = value; }
        }
    }
}
