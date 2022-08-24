using Godot;
using Godot.Collections;
using GodotRollbackNetcode;
using System;

namespace Game
{
    public class CSharpExplosion : Node2D, INetworkSpawn, INetworkDespawn
    {
        NetworkTimer despawnTimer;
        NetworkAnimationPlayer animationPlayer;
        AudioStream explosionSound;

        public void _NetworkDespawn()
        {
            animationPlayer.Stop(true);
        }

        public void _NetworkSpawn(Dictionary data)
        {
            GlobalPosition = (Vector2)data["position"];
            despawnTimer.Start();
            animationPlayer.Play("Explode");
            SyncManager.Global.PlaySound($"{GetPath()}:Explosion", explosionSound);
        }

        public override void _Ready()
        {
            despawnTimer = this.GetNodeAsWrapper<NetworkTimer>("DespawnTimer");
            despawnTimer.Timeout += OnDespawnTimerTimeout;

            animationPlayer = this.GetNodeAsWrapper<NetworkAnimationPlayer>("NetworkAnimationPlayer");

            explosionSound = GD.Load<AudioStream>("res://assets/explosion.wav");
        }

        private void OnDespawnTimerTimeout()
        {
            SyncManager.Global.Despawn(this);
        }
    }
}
