using Godot;
using GodotRollbackNetcode;
using System;
using System.Threading.Tasks;

namespace Game
{
    public class CSharpMain : Node2D
    {
        const string LOG_FILE_DIRECTORY = "user://detailed_logs";
        [Export]
        bool loggingEnabled = true;

        Control connectionPanel;
        LineEdit hostField;
        LineEdit portField;
        Label messageLabel;
        Label syncLostLabel;
        Node serverPlayer;
        Node clientPlayer;
        Button resetButton;

        public override void _Ready()
        {
            connectionPanel = GetNode<Control>("CanvasLayer/ConnectionPanel");
            hostField = GetNode<LineEdit>("CanvasLayer/ConnectionPanel/GridContainer/HostField");
            portField = GetNode<LineEdit>("CanvasLayer/ConnectionPanel/GridContainer/PortField");
            messageLabel = GetNode<Label>("CanvasLayer/MessageLabel");
            syncLostLabel = GetNode<Label>("CanvasLayer/SyncLostLabel");
            resetButton = GetNode<Button>("CanvasLayer/ResetButton");
            serverPlayer = GetNode("ServerPlayer");
            clientPlayer = GetNode("ClientPlayer");

            GetTree().Connect("network_peer_connected", this, nameof(OnNetworkPeerConnected));
            GetTree().Connect("network_peer_disconnected", this, nameof(OnNetworkPeerDisconnected));
            GetTree().Connect("server_disconnected", this, nameof(OnServerDisconnected));

            SyncManager.Instance.SyncStarted += OnSyncManagerSyncStarted;
            SyncManager.Instance.SyncStopped += OnSyncManagerSyncStopped;
            SyncManager.Instance.SyncLost += OnSyncManagerSyncLost;
            SyncManager.Instance.SyncRegained += OnSyncManagerSyncRegained;
            SyncManager.Instance.SyncError += OnSyncManagerSyncError;

            syncLostLabel.Visible = false;
        }

        public override void _Notification(int what)
        {
            if (what == NotificationPredelete)
            {
                SyncManager.Instance.SyncStarted -= OnSyncManagerSyncStarted;
                SyncManager.Instance.SyncStopped -= OnSyncManagerSyncStopped;
                SyncManager.Instance.SyncLost -= OnSyncManagerSyncLost;
                SyncManager.Instance.SyncRegained -= OnSyncManagerSyncRegained;
                SyncManager.Instance.SyncError -= OnSyncManagerSyncError;
            }
        }

        private void OnServerButtonPressed()
        {
            var peer = new NetworkedMultiplayerENet();
            if (!int.TryParse(portField.Text, out int port))
                return;
            peer.CreateServer(port, 1);
            GetTree().NetworkPeer = peer;
            messageLabel.Text = "Listening...";
            connectionPanel.Visible = false;
        }

        private void OnClientButtonPressed()
        {
            if (!int.TryParse(portField.Text, out int port))
                return;
            var peer = new NetworkedMultiplayerENet();
            peer.CreateClient(hostField.Text, port);
            GetTree().NetworkPeer = peer;
            messageLabel.Text = "Connecting...";
            connectionPanel.Visible = false;
        }

        private async void OnNetworkPeerConnected(int peerId)
        {
            messageLabel.Text = "Connected with id: " + peerId;
            SyncManager.Instance.AddPeer(peerId);

            serverPlayer.SetNetworkMaster(1);
            if (GetTree().IsNetworkServer())
                clientPlayer.SetNetworkMaster(peerId);
            else
                clientPlayer.SetNetworkMaster(GetTree().GetNetworkUniqueId());

            if (GetTree().IsNetworkServer())
            {
                messageLabel.Text = "Starting...";
                await ToSignal(GetTree().CreateTimer(2.0f), "timeout");
                SyncManager.Instance.Start();
            }
        }

        private void OnNetworkPeerDisconnected(int peerId)
        {
            messageLabel.Text = "Disconnected with id: " + peerId;
            SyncManager.Instance.RemovePeer(peerId);
        }

        private void OnServerDisconnected()
        {
            OnNetworkPeerDisconnected(1);
        }

        private void OnResetButtonPressed()
        {
            SyncManager.Instance.Stop();
            SyncManager.Instance.ClearPeers();
            var peer = GetTree().NetworkPeer;
            if (peer is NetworkedMultiplayerENet enetPeer)
                enetPeer.CloseConnection();
            GetTree().ReloadCurrentScene();
        }

        private void OnSyncManagerSyncStarted()
        {
            messageLabel.Text = "Started!";

            if (loggingEnabled && !SyncReplay.Instance.Active)
            {
                var dir = new Directory();
                if (!dir.DirExists(LOG_FILE_DIRECTORY))
                    dir.MakeDir(LOG_FILE_DIRECTORY);

                var datetime = OS.GetDatetime(true);
                string logFileName = string.Format("{0}-{1}-{2}_{3}-{4}-{5}_peer-{6}.log", datetime["year"], datetime["month"], datetime["day"], datetime["hour"], datetime["minute"], datetime["second"], GetTree().GetNetworkUniqueId());

                SyncManager.Instance.StartLogging(LOG_FILE_DIRECTORY + "/" + logFileName);
            }
        }

        private void OnSyncManagerSyncStopped()
        {
            if (loggingEnabled)
                SyncManager.Instance.StopLogging();
        }

        private void OnSyncManagerSyncLost()
        {
            syncLostLabel.Visible = true;
        }

        private void OnSyncManagerSyncRegained()
        {
            syncLostLabel.Visible = false;
        }

        private void OnSyncManagerSyncError(string msg)
        {
            messageLabel.Text = "Fatal sync error: " + msg;
            syncLostLabel.Visible = false;

            var peer = GetTree().NetworkPeer;
            if (peer is NetworkedMultiplayerENet enetPeer)
                enetPeer.CloseConnection();
            SyncManager.Instance.ClearPeers();
        }

        public void SetupMatchForReplay(int myPeerId, Godot.Collections.Array peerIds, Godot.Collections.Dictionary matchInfo)
        {
            connectionPanel.Visible = false;
            resetButton.Visible = false;
        }
    }
}