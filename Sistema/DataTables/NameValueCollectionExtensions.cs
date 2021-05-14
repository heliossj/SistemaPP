using System;
using System.Collections.Specialized;
using System.Linq;

namespace Sistema.DataTables
{
    public static class NameValueCollectionExtensions
    {
        public static T G<T>(this NameValueCollection collection, string key)
        {
            return collection.G<T>(key, default(T));
        }

        public static T G<T>(this NameValueCollection collection, string key, object defaultValue)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection", "The provided collection cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("The provided key cannot be null or empty.", "key");
            }
            string text = collection[key];
            if (text == null)
            {
                return (T)defaultValue;
            }
            return (T)Convert.ChangeType(text, typeof(T));
        }
        public static void S(this NameValueCollection collection, string key, object value)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection", "The provided collection cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("The provided key cannot be null or empty.", "key");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value", "The provided value cannot be null.");
            }
            if (collection.Keys.Cast<string>().Any((string _k) => _k.Equals(key)))
            {
                collection[key] = value.ToString();
                return;
            }
            collection.Add(key, value.ToString());
        }
    }
}
