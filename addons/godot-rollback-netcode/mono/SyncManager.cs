using Godot;
using System;
using System.Collections.Generic;

namespace GodotRollbackNetcode
{
    public class SyncManager : GDScriptWrapper
    {
        private static SyncManager instance;
        public static SyncManager Instance
        {
            get
            {
                if (instance == null)
                    GD.PrintErr("Expected C# SyncManager singleton to be initialized. Did you forget to call SyncManager.Init(node)?");
                return instance;
            }
        }
        public static void Init(Node node)
        {
            instance = new SyncManager(node.GetNode("/root/SyncManager"));
        }

        #region Object Variables
        public Godot.Object NetworkAdapter
        {
            get => (Godot.Object)Source.Get("network_adaptor");
            set => Source.Set("network_adaptor", value);
        }

        public Godot.Object MessageSerializer
        {
            get => (Godot.Object)Source.Get("message_serializer");
            set => Source.Set("message_serializer", value);
        }

        public Godot.Object HashSerializer
        {
            get => (Godot.Object)Source.Get("hash_serializer");
            set => Source.Set("hash_serializer", value);
        }
        #endregion

        #region Collection Variables
        public Godot.Collections.Dictionary Peers
        {
            get => (Godot.Collections.Dictionary)Source.Get("peers");
            set => Source.Set("peers", value);
        }

        public Godot.Collections.Array InputBuffer
        {
            get => (Godot.Collections.Array)Source.Get("input_buffer");
            set => Source.Set("input_buffer", value);
        }

        public Godot.Collections.Array StateBuffer
        {
            get => (Godot.Collections.Array)Source.Get("state_buffer");
            set => Source.Set("state_buffer", value);
        }

        public Godot.Collections.Array StateHashes
        {
            get => (Godot.Collections.Array)Source.Get("state_hashes");
            set => Source.Set("state_hashes", value);
        }
        #endregion

        #region Mechanized Variables
        public bool Mechanized
        {
            get => (bool)Source.Get("mechanized");
            set => Source.Set("mechanized", value);
        }

        public Godot.Collections.Dictionary MechanizedInputReceived
        {
            get => (Godot.Collections.Dictionary)Source.Get("mechanized_input_received");
            set => Source.Set("mechanized_input_received", value);
        }

        public int MechanizedRollbackTicks
        {
            get => (int)Source.Get("mechanized_rollback_ticks");
            set => Source.Set("mechanized_rollback_ticks", value);
        }
        #endregion

        #region General Variables
        public int MaxBufferSize
        {
            get => (int)Source.Get("max_buffer_size");
            set => Source.Set("max_buffer_size", value);
        }

        public int TicksToCalculateAdvantage
        {
            get => (int)Source.Get("ticks_to_calculate_advantage");
            set => Source.Set("ticks_to_calculate_advantage", value);
        }

        public int InputDelay
        {
            get => (int)Source.Get("input_delay");
            set => Source.Set("input_delay", value);
        }

        public int MaxInputFramesPerMessage
        {
            get => (int)Source.Get("max_input_frames_per_message");
            set => Source.Set("max_input_frames_per_message", value);
        }

        public int MaxMessagesAtOnce
        {
            get => (int)Source.Get("max_messages_at_once");
            set => Source.Set("max_messages_at_once", value);
        }

        public int MaxTicksToRegainSync
        {
            get => (int)Source.Get("max_ticks_to_regain_sync");
            set => Source.Set("max_ticks_to_regain_sync", value);
        }

        public int MinLagToRegainSync
        {
            get => (int)Source.Get("min_lag_to_regain_sync");
            set => Source.Set("min_lag_to_regain_sync", value);
        }

        public bool Interpolation
        {
            get => (bool)Source.Get("interpolation");
            set => Source.Set("interpolation", value);
        }

        public int MaxStateMismatchCount
        {
            get => (int)Source.Get("max_state_mismatch_count");
            set => Source.Set("max_state_mismatch_count", value);
        }
        #endregion

        #region Debug Variables
        public int DebugrollbackTicks
        {
            get => (int)Source.Get("debug_rollback_ticks");
            set => Source.Set("debug_rollback_ticks", value);
        }

        public int DebugRandomRollbackTicks
        {
            get => (int)Source.Get("debug_random_rollback_ticks");
            set => Source.Set("debug_random_rollback_ticks", value);
        }

        public int DebugMessageBytes
        {
            get => (int)Source.Get("debug_message_bytes");
            set => Source.Set("debug_message_bytes", value);
        }

        public int DebugSkipNthMessage
        {
            get => (int)Source.Get("debug_skip_nth_message");
            set => Source.Set("debug_skip_nth_message", value);
        }

        public float DebugPhysicsProcessMsecs
        {
            get => (float)Source.Get("debug_physics_process_msecs");
            set => Source.Set("debug_physics_process_msecs", value);
        }

        public float DebugProcessMsecs
        {
            get => (float)Source.Get("debug_process_msecs");
            set => Source.Set("debug_process_msecs", value);
        }

        public bool DebugCheckMessageSerializerRoundtrip
        {
            get => (bool)Source.Get("debug_check_message_serializer_roundtrip");
            set => Source.Set("debug_check_message_serializer_roundtrip", value);
        }

        public bool DebugCheckLocalStateConsistency
        {
            get => (bool)Source.Get("debug_check_local_state_consistency");
            set => Source.Set("debug_check_local_state_consistency", value);
        }
        #endregion

        /// <summary>
        /// The ping frequency in seconds, because we don't want it to be dependent on the network tick.
        /// </summary>
        public float PingFrequency
        {
            get => (float)Source.Get("ping_frequency");
            set => Source.Set("ping_frequency", value);
        }

        #region Readonly Variables
        public int InputTick => (int)Source.Get("input_tick");
        public int CurrentTick => (int)Source.Get("current_tick");
        public int SkipTicks => (int)Source.Get("skip_ticks");
        public int RollbackTicks => (int)Source.Get("rollback_ticks");
        public int RequestedInputCompleteTick => (int)Source.Get("requested_input_complete_tick");
        public bool Started => (bool)Source.Get("started");
        public float TickTime => (float)Source.Get("tick_time");
        #endregion

        public SyncManager() { }

        public SyncManager(Godot.Object source) : base(source)
        {
            ForwardSignalsToEvents();
        }

        #region Methods
        public void ResetNetworkAdaptor() => Source.Call("reset_network_adaptor");

        public void AddPeer(int peerId) => Source.Call("add_peer", peerId);

        public bool HasPeer(int peerId) => (bool)Source.Call("has_peer", peerId);

        public void RemovePeer(int peerid) => Source.Call("remove_peer", peerid);

        public void ClearPeers() => Source.Call("clear_peers");

        public void StartLogging(string logFilePath) => StartLogging(logFilePath, new Godot.Collections.Dictionary());

        public void StartLogging(string logFilePath, Godot.Collections.Dictionary matchInfo) => Source.Call("start_logging", logFilePath, matchInfo);

        public void StopLogging() => Source.Call("stop_logging");

        public void Start() => Source.Call("start");

        public void Stop() => Source.Call("stop");

        public void ResetMechanizedData() => Source.Call("reset_mechanized_data");


        public void ExecuteMechanizedTick() => Source.Call("execute_mechanized_tick");

        public void ExecuteMechanizedInterpolationFrame(float delta) => Source.Call("execute_mechanized_interpolation_frame", delta);

        public void ExecuteMechanizedInterframe() => Source.Call("execute_mechanized_interframe");

        public Godot.Collections.Dictionary SortDictionaryKeys(Godot.Collections.Dictionary dictionary) => (Godot.Collections.Dictionary)Source.Call("sort_dictionary_keys");

        public Node Spawn(string name, Node parent, PackedScene scene, Godot.Collections.Dictionary data, bool rename = true, string signalName = "") => (Node)Source.Call("spawn", name, parent, scene, data, rename, signalName);

        public void Despawn(Node node) => Source.Call("despawn", node);

        public bool IsInRollback => (bool)Source.Call("is_in_rollback");
        public bool IsRespawning => (bool)Source.Call("is_respawning");

        public void SetDefaultSoundBus(string bus) => Source.Call("set_default_sound_bus", bus);

        public void PlaySound(string identifier, AudioStream sound, Godot.Collections.Dictionary info) => Source.Call("play_sound", identifier, sound, info);

        public bool EnsureCurrentTickInputComplete() => (bool)Source.Call("ensure_current_tick_input_complete");

        public string OrderedDict2Str(Godot.Collections.Dictionary dict) => (string)Source.Call("ordered_dict2str");
        #endregion

        #region Signal Events
        public event Action SyncStarted;
        public event Action SyncStopped;
        public event Action SyncLost;
        public event Action SyncRegained;
        /// <summary>
        /// message
        /// </summary>
        public event Action<string> SyncError;
        /// <summary>
        /// count
        /// </summary>
        public event Action<int> SkipTicksFlagged;
        /// <summary>
        /// tick
        /// </summary>
        public event Action<int> RollbackFlagged;
        /// <summary>
        /// tick, peerId, localInput, remoteInput
        /// </summary>
        public event Action<int, int, Godot.Collections.Dictionary, Godot.Collections.Dictionary> PredictionMissed;
        /// <summary>
        /// tick, peerId, localInput, remoteInput
        /// </summary>
        public event Action<int, int, int, int> RemoteStateMismatch;
        /// <summary>
        /// peerId
        /// </summary>
        public event Action<int> PeerAdded;
        /// <summary>
        /// peerId
        /// </summary>
        public event Action<int> PeerRemoved;
        /// <summary>
        /// peer
        /// </summary>
        public event Action<Godot.Object> PeerPingedBack;
        /// <summary>
        /// rollbackTicks
        /// </summary>
        public event Action<int> StateLoaded;
        /// <summary>
        /// isRollback
        /// </summary>
        public event Action<bool> TickFinished;
        /// <summary>
        /// tick
        /// </summary>
        public event Action<int> TickRetired;
        /// <summary>
        /// tick
        /// </summary>
        public event Action<int> TickInputComplete;
        /// <summary>
        /// name, spawnedNode, scene, data
        /// </summary>
        public event Action<string, Node, PackedScene, Godot.Collections.Dictionary> SceneSpawned;
        /// <summary>
        /// name, node
        /// </summary>
        public event Action<string, Node> SceneDespawned;
        public event Action InterpolationFrame;
        #endregion

        #region Signal Forwarding
        private void ForwardSignalsToEvents()
        {
            Source.Connect("sync_started", this, nameof(OnSyncStarted));
            Source.Connect("sync_stopped", this, nameof(OnSyncStopped));
            Source.Connect("sync_lost", this, nameof(OnSyncLost));
            Source.Connect("sync_regained", this, nameof(OnSyncRegained));
            Source.Connect("sync_error", this, nameof(OnSyncError));

            Source.Connect("skip_ticks_flagged", this, nameof(OnSkipTicksFlagged));
            Source.Connect("rollback_flagged", this, nameof(OnRollbackFlagged));
            Source.Connect("prediction_missed", this, nameof(OnPredictionMissed));
            Source.Connect("remote_state_mismatch", this, nameof(OnRemoteStateMismatch));

            Source.Connect("peer_added", this, nameof(OnPeerAdded));
            Source.Connect("peer_removed", this, nameof(OnPeerRemoved));
            Source.Connect("peer_pinged_back", this, nameof(OnPeerPingedBack));

            Source.Connect("state_loaded", this, nameof(OnStateLoaded));
            Source.Connect("tick_finished", this, nameof(OnTickFinished));
            Source.Connect("tick_retired", this, nameof(OnTickRetired));
            Source.Connect("tick_input_complete", this, nameof(OnTickInputComplete));
            Source.Connect("scene_spawned", this, nameof(OnSceneSpawned));
            Source.Connect("scene_despawned", this, nameof(OnSceneDespawned));
            Source.Connect("interpolation_frame", this, nameof(OnInterpolationFrame));
        }

        private void OnInterpolationFrame()
        {
            InterpolationFrame?.Invoke();
        }

        private void OnSceneDespawned(string name, Node node)
        {
            SceneDespawned?.Invoke(name, node);
        }

        private void OnSceneSpawned(string name, Node spawnedNode, PackedScene scene, Godot.Collections.Dictionary data)
        {
            SceneSpawned?.Invoke(name, spawnedNode, scene, data);
        }

        private void OnTickInputComplete(int tick)
        {
            TickInputComplete?.Invoke(tick);
        }

        private void OnTickRetired(int tick)
        {
            TickRetired?.Invoke(tick);
        }

        private void OnTickFinished(bool isRollback)
        {
            TickFinished?.Invoke(isRollback);
        }

        private void OnStateLoaded(int rollbackTicks)
        {
            StateLoaded?.Invoke(rollbackTicks);
        }

        private void OnPeerPingedBack(Godot.Object peer)
        {
            PeerPingedBack?.Invoke(peer);
        }

        private void OnPeerRemoved(int peerId)
        {
            PeerRemoved?.Invoke(peerId);
        }

        private void OnPeerAdded(int peerId)
        {
            PeerAdded?.Invoke(peerId);
        }

        private void OnRemoteStateMismatch(int tick, int peerId, int localHash, int remoteHash)
        {
            RemoteStateMismatch?.Invoke(tick, peerId, localHash, remoteHash);
        }

        private void OnPredictionMissed(int tick, int peerId, Godot.Collections.Dictionary localInput, Godot.Collections.Dictionary remoteInput)
        {
            PredictionMissed?.Invoke(tick, peerId, localInput, remoteInput);
        }

        private void OnRollbackFlagged(int tick)
        {
            RollbackFlagged?.Invoke(tick);
        }

        private void OnSkipTicksFlagged(int count)
        {
            SkipTicksFlagged?.Invoke(count);
        }

        private void OnSyncError(string msg)
        {
            SyncError?.Invoke(msg);
        }

        private void OnSyncRegained()
        {
            SyncRegained?.Invoke();
        }

        private void OnSyncStopped()
        {
            SyncStopped?.Invoke();
        }

        private void OnSyncLost()
        {
            SyncLost?.Invoke();
        }

        private void OnSyncStarted()
        {
            SyncStarted?.Invoke();
        }
        #endregion
    }
}
