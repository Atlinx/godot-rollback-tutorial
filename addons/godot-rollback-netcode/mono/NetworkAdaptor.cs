using Godot;
using System.Collections.Generic;

namespace GodotRollbackNetcode
{
    public abstract class NetworkAdaptor : Godot.Object
    {
        private void attach_network_adaptor(Godot.Object sync_manager) => AttachNetworkAdaptor(sync_manager.AsWrapper<SyncManager>());

        public virtual void AttachNetworkAdaptor(SyncManager syncManager) { }

        private void detach_network_adaptor(Godot.Object sync_manager) => DetachNetworkAdaptor(sync_manager.AsWrapper<SyncManager>());

        public virtual void DetachNetworkAdaptor(SyncManager syncManager) { }

        private void stop_network_adaptor(Godot.Object sync_manager) => StopNetworkAdaptor(sync_manager.AsWrapper<SyncManager>());

        public virtual void StopNetworkAdaptor(SyncManager syncManager) { }

        private void poll() => Poll();

        public virtual void Poll() { }

        private void send_ping(int peer_id, Godot.Collections.Dictionary msg) => SendPing(peer_id, msg);

        public abstract void SendPing(int peerId, Godot.Collections.Dictionary msg);

        private void send_ping_back(int peer_id, Godot.Collections.Dictionary msg) => SendPingBack(peer_id, msg);

        public abstract void SendPingBack(int peerId, Godot.Collections.Dictionary msg);

        private void send_remote_start(int peer_id) => SendRemoteStart(peer_id);

        public abstract void SendRemoteStart(int peerId);

        private void send_remote_stop(int peer_id) => SendRemoteStop(peer_id);

        public abstract void SendRemoteStop(int peerId);

        private void send_input_tick(int peer_id, byte[] msg) => SendInputTick(peer_id, msg);

        public abstract void SendInputTick(int peerId, byte[] msg);

        private bool is_network_host() => IsNetworkHost();

        public abstract bool IsNetworkHost();

        private bool is_network_master_for_node(Node node) => IsNetworkMasterForNode(node);

        protected abstract bool IsNetworkMasterForNode(Node node);

        private int get_network_unique_id() => GetNetworkUniqueId();

        public abstract int GetNetworkUniqueId();
    }
}
