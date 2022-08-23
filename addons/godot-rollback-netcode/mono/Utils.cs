using Godot;
using System;

namespace GodotRollbackNetcode
{
    public static class Utils
    {
        public static T AsWrapper<T>(this Godot.Object source) where T : GDScriptWrapper
        {
            return (T)Activator.CreateInstance(typeof(T), new object[] { source });
        }

        public static T GetNodeAsWrapper<T>(this Node node, NodePath path) where T : GDScriptWrapper
        {
            return node.GetNode(path).AsWrapper<T>();
        }
    }
}
