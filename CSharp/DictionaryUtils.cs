using Godot.Collections;
using System.Reflection;

namespace Game
{
    public static class DictionaryUtils
    {
        public static T Get<T>(this Dictionary dictionary, string key, T defaultReturn = default)
        {
            if (dictionary.Contains(key))
                return (T)dictionary[key];
            return defaultReturn;
        }

        public static Dictionary ToGodotDict(this object obj)
        {
            Dictionary dict = new Dictionary();
            foreach (var prop in obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                dict[prop.Name] = prop.GetValue(obj, null);
            }
            return dict;
        }
    }
}