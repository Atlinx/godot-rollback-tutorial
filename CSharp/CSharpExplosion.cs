using Godot;
using Godot.Collections;
using GodotRollbackNetcode;
using System;

namespace Game
{
    public class CSharpExplosion : Node2D, INetworkSpawn
    {
        NetworkedTimer despawnTimer;

        public void _NetworkSpawn(Dictionary data)
        {
            GlobalPosition = (Vector2)data["position"];
            despawnTimer.Start();
        }

        public override void _Ready()
        {
            despawnTimer = this.GetNodeAsWrapper<NetworkedTimer>("DespawnTimer");
            despawnTimer.Timeout += OnDespawnTimerTimeout;
        }

        private void OnDespawnTimerTimeout()
        {
            SyncManager.Instance.Despawn(this);
        }
    }
}
