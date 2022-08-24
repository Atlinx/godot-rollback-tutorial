using Godot;
using Godot.Collections;
using GodotRollbackNetcode;
using System;

namespace Game
{
    public class CSharpPlayer : Node2D, IGetLocalInput, INetworkProcess, INetworkSerializable, IPredictRemoteInput
    {
        PackedScene bombPrefab;
        PackedScene explosionPrefab;

        float speed = 0;

        public override void _Ready()
        {
            SyncManager.Instance.SceneSpawned += OnSyncManagerSceneSpawned;
            SyncManager.Instance.SceneDespawned += OnSyncManagerSceneDespawned;

            bombPrefab = GD.Load<PackedScene>("res://CSharp/CSharpBomb.tscn");
            explosionPrefab = GD.Load<PackedScene>("res://CSharp/CSharpExplosion.tscn");
        }

        private void OnSyncManagerSceneDespawned(string name, Node node)
        {
            if (node is CSharpBomb bomb && bomb.IsOwnedBy(this))
            {
                bomb.Exploded -= OnBombExploded;
            }
        }

        private void OnSyncManagerSceneSpawned(string name, Node spawnedNode, PackedScene scene, Dictionary data)
        {
            if (spawnedNode is CSharpBomb bomb && bomb.IsOwnedBy(this))
            {
                bomb.Exploded += OnBombExploded;
            }
        }

        private void OnBombExploded()
        {
            SyncManager.Instance.Spawn("Explosion", GetParent(), explosionPrefab, new { position = GlobalPosition }.ToGodotDict());
        }

        public Dictionary _GetLocalInput()
        {
            var inputVector = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

            var input = new Dictionary();
            if (inputVector != Vector2.Zero)
                input["input_vector"] = inputVector;
            if (Input.IsActionJustPressed("ui_accept"))
                input["drop_bomb"] = true;

            return input;
        }

        public void _NetworkProcess(Dictionary input)
        {
            var inputVector = input.Get("input_vector", Vector2.Zero);
            if (inputVector != Vector2.Zero)
            {
                if (speed < 8)
                    speed += 0.2f;
                Position += inputVector * speed;
            }
            else
            {
                if (speed > 0)
                    speed -= 0.2f;
                else
                    speed = 0;
            }
            if (input.Get("drop_bomb", false))
                CSharpBomb.Spawn(GetParent(), GlobalPosition, this);
        }

        public Dictionary _SaveState()
        {
            var state = new Dictionary();

            state["position"] = Position;
            state["speed"] = speed;

            return state;
        }

        public void _LoadState(Dictionary state)
        {
            Position = (Vector2)state["position"];
            speed = (float)state["speed"];
        }

        public Dictionary _PredictRemoteInput(Dictionary previousInput, int ticksSinceRealInput)
        {
            var input = previousInput.Duplicate();
            input.Remove("drop_bomb");
            return input;
        }
    }
}