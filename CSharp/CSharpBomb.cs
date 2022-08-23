using Godot;
using Godot.Collections;
using GodotRollbackNetcode;
using System;

namespace Game
{
    public class CSharpBomb : Node2D, INetworkSpawn
    {
        NetworkedTimer timer;

        public override void _Ready()
        {
            timer = this.GetNodeAsWrapper<NetworkedTimer>("ExplosionTimer");
        }

        public void _NetworkSpawn(Dictionary data)
        {
            GlobalPosition = (Vector2)data["position"];
            //timer.Connect()
            // TODO: Finish
        }
    }
}