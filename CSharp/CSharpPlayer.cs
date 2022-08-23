using Godot;
using Godot.Collections;
using GodotRollbackNetcode;

namespace Game
{
    public class CSharpPlayer : Node2D, IGetLocalInput, INetworkProcess, INetworkSerializable
    {
        public Dictionary _GetLocalInput()
        {
            var inputVector = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

            var input = new Dictionary();
            if (inputVector != Vector2.Zero)
                input["input_vector"] = inputVector;

            return input;
        }

        public void _NetworkProcess(Dictionary input)
        {
            Position += input.Get("input_vector", Vector2.Zero) * 8;
        }

        public Dictionary _SaveState()
        {
            var state = new Dictionary();

            state["position"] = Position;

            return state;
        }

        public void _LoadState(Dictionary state)
        {
            Position = (Vector2)state["position"];
        }
    }
}