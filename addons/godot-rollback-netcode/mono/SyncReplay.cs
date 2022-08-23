using Godot;

namespace GodotRollbackNetcode
{
    public class SyncReplay : GDScriptWrapper
    {
        private static SyncReplay instance;
        public static SyncReplay Instance
        {
            get
            {
                if (instance == null)
                    GD.PrintErr("Expected C# SyncReplay singleton to be initialized. Did you forget to call SyncReplay.Init(node)?");
                return instance;
            }
        }
        public static void Init(Node node)
        {
            instance = new SyncReplay(node.GetNode("/root/SyncReplay"));
        }

        public SyncReplay() { }

        public SyncReplay(Godot.Object source) : base(source) { }

        public bool Active
        {
            get => (bool)Source.Get("active");
            set => Source.Set("active", value);
        }

        public StreamPeerTCP Connection
        {
            get => (StreamPeerTCP)Source.Get("connection");
            set => Source.Set("connection", value);
        }

        public string MatchScenePath
        {
            get => (string)Source.Get("match_scene_path");
            set => Source.Set("match_scene_path", value);
        }

        public string MatchSceneMethod
        {
            get => (string)Source.Get("match_scene_method");
            set => Source.Set("match_scene_method", value);
        }
    }
}
