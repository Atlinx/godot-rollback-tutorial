using Godot;
using System;

namespace Game
{
    public class CSharpInterop : Node2D
    {
        Node2D csharpInterop;

        public override void _Ready()
        {
            csharpInterop = GetNode<Node2D>("../GDScriptInterop");
        }

        public uint CastMeFromBuffer(int number)
        {
            var buffer = new StreamPeerBuffer();
            buffer.Put32(number);
            buffer.Seek(0);
            return buffer.GetU32();
        }

        public Godot.Collections.Dictionary CastMeFromBufferToDict(int number)
        {
            var buffer = new StreamPeerBuffer();
            buffer.Put32(number);
            buffer.Seek(0);
            var unsignedNumber = buffer.GetU32();
            var dict = new Godot.Collections.Dictionary();
            dict["number"] = unsignedNumber;
            return dict;
        }
    }
}
