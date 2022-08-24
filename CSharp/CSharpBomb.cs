using Godot;
using Godot.Collections;
using GodotRollbackNetcode;
using System;

namespace Game
{
    public class CSharpBomb : Node2D, INetworkSpawn, INetworkDespawn
    {
        public static Node Spawn(Node parent, Vector2 position, Node owner)
        {
            return SyncManager.Global.Spawn(nameof(CSharpBomb), parent, GD.Load<PackedScene>("res://CSharp/CSharpBomb.tscn"), new { position, ownerPath = owner.GetPath().ToString() }.ToGodotDict());
        }

        public event Action Exploded;
        public NodePath BombOwnerPath { get; private set; }

        PackedScene explosionPrefab;
        NetworkTimer explosionTimer;
        NetworkAnimationPlayer animationPlayer;

        public override void _Ready()
        {
            explosionTimer = this.GetNodeAsWrapper<NetworkTimer>("ExplosionTimer");
            explosionTimer.Connect("timeout", this, nameof(OnExplosionTimerTimeout));

            animationPlayer = this.GetNodeAsWrapper<NetworkAnimationPlayer>("NetworkAnimationPlayer");

            explosionPrefab = GD.Load<PackedScene>("res://CSharp/CSharpExplosion.tscn");
        }

        public bool IsOwnedBy(Node node)
        {
            if (!IsInstanceValid(node)) return false;
            return node.GetPath().ToString() == BombOwnerPath;
        }

        public void _NetworkSpawn(Dictionary data)
        {
            GlobalPosition = (Vector2)data["position"];
            BombOwnerPath = (string)data["ownerPath"];
            explosionTimer.Start();
            animationPlayer.Play("Tick");
        }

        private void OnExplosionTimerTimeout()
        {
            Exploded?.Invoke();
            SyncManager.Global.Spawn("Explosion", GetParent(), explosionPrefab, new { position = GlobalPosition }.ToGodotDict());
            SyncManager.Global.Despawn(this);
        }

        public void _NetworkDespawn()
        {
            animationPlayer.Stop(true);
            Exploded = null;
        }
    }
}