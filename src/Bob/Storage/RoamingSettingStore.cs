using Windows.Storage;

namespace Bob.Storage
{
    public class RoamingSettingStore : ISettingsStore
    {
        private readonly ApplicationDataContainer roamingStorage;

        public RoamingSettingStore()
        {
            roamingStorage = ApplicationData.Current.RoamingSettings;
        }

        public T GetValue<T>(string key)
        {
            return (T)roamingStorage.Values[key];
        }

        public bool TryGetValue<T>(string key, out T value)
        {
            object val;
            value = default(T);

            var result = roamingStorage.Values.TryGetValue(key, out val);

            if (result)
                value = (T) val;

            return result;
        }

        public bool ContainsKey(string key)
        {
            return roamingStorage.Values.ContainsKey(key);
        }

        public void SetValue<T>(string key, T value)
        {
            roamingStorage.Values.Add(key, value);
        }

        public object this[string key]
        {
            get { return roamingStorage.Values[key]; }
            set { roamingStorage.Values[key] = value; }
        }
    }
}