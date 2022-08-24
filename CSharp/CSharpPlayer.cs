using Godot;
using Godot.Collections;
using GodotRollbackNetcode;
using System;

namespace Game
{
    public class CSharpPlayer : Node2D, IGetLocalInput, INetworkProcess, INetworkSerializable, IPredictRemoteInput, IInterpolateState
    {
        PackedScene bombPrefab;
        PackedScene explosionPrefab;
        NetworkRandomNumberGenerator rng;

        [Export]
        public string inputPrefix = "player1_";

        float speed = 0;
        bool teleporting = false;

        public override void _Ready()
        {
            SyncManager.Global.SceneSpawned += OnSyncManagerSceneSpawned;
            SyncManager.Global.SceneDespawned += OnSyncManagerSceneDespawned;

            rng = this.GetNodeAsWrapper<NetworkRandomNumberGenerator>("NetworkRandomNumberGenerator");

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
            SyncManager.Global.Spawn("Explosion", GetParent(), explosionPrefab, new { position = GlobalPosition }.ToGodotDict());
        }

        public Dictionary _GetLocalInput()
        {
            var inputVector = Input.GetVector(inputPrefix + "left", inputPrefix + "right", inputPrefix + "up", inputPrefix + "down");

            var input = new Dictionary();
            if (inputVector != Vector2.Zero)
                input["input_vector"] = inputVector;
            if (Input.IsActionJustPressed(inputPrefix + "bomb"))
                input["drop_bomb"] = true;
            if (Input.IsActionJustPressed(inputPrefix + "teleport"))
                input["teleport"] = true;

            return input;
        }

        public void _NetworkProcess(Dictionary input)
        {
            var inputVector = input.Get("input_vector", Vector2.Zero);
            if (inputVector != Vector2.Zero)
            {
                if (speed < 20)
                    speed += 2;
                Position += inputVector * speed;
            }
            else
            {
                speed = 0;
            }
            if (input.Get("drop_bomb", false))
                CSharpBomb.Spawn(GetParent(), GlobalPosition, this);
            if (input.Get("teleport", false))
            {
                var position = new Vector2(rng.Randi() % 1024, rng.Randi() % 600);
                if (position.x < 0)
                    position.x *= -1;
                if (position.y < 0)
                    position.y *= -1;
                Position = position;
                teleporting = true;
            }
            else
                teleporting = false;
        }

        public Dictionary _SaveState()
        {
            var state = new Dictionary();

            state["position"] = Position;
            state["speed"] = speed;
            state["teleporting"] = teleporting;

            return state;
        }

        public void _LoadState(Dictionary state)
        {
            Position = (Vector2)state["position"];
            speed = (float)state["speed"];
            teleporting = (bool)state["teleporting"];
        }

        public Dictionary _PredictRemoteInput(Dictionary previousInput, int ticksSinceRealInput)
        {
            var input = previousInput.Duplicate();
            input.Remove("drop_bomb");
            if (ticksSinceRealInput > 2)
                input.Remove("input_vector");
            return input;
        }

        public void _InterpolateState(Dictionary oldState, Dictionary newState, float weight)
        {
            if (oldState.Get("teleporting", false) || newState.Get("teleporting", false))
                return;
            Position = Utils.Lerp((Vector2)oldState["position"], (Vector2)newState["position"], weight);
        }

        public void Seed(NetworkRandomNumberGenerator johnny)
        {
            rng.Seed = johnny.Randi();
        }
    }
}