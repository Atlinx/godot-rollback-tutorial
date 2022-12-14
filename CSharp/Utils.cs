using Godot;
using Godot.Collections;
using System.Reflection;

namespace Game
{
    public static class Utils
    {
        public static T Get<T>(this Dictionary dictionary, object key, T defaultReturn = default)
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

        public static Vector2 Lerp(this Vector2 start, Vector2 end, float weight)
        {
            return new Vector2(Mathf.Lerp(start.x, end.x, weight), Mathf.Lerp(start.y, end.y, weight));
        }
    }
}