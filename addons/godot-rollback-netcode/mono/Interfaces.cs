namespace GodotRollbackNetcode
{
    public interface INetworkSync { }
    public interface IGetLocalInput : INetworkSync
    {
        Godot.Collections.Dictionary _GetLocalInput();
    }

    public interface INetworkProcess : INetworkSync
    {
        void _NetworkProcess(Godot.Collections.Dictionary input);
    }

    public interface INetworkPreProcess : INetworkSync
    {
        void _NetworkPreProcess(Godot.Collections.Dictionary input);
    }

    public interface INetworkPostProcess : INetworkSync
    {
        void _NetworkPostProcess(Godot.Collections.Dictionary input);
    }

    public interface IInterpolateState : INetworkSync
    {
        void _InterpolateState(Godot.Collections.Array states);
    }

    public interface IPredictRemoteInput : INetworkSync
    {
        Godot.Collections.Dictionary _PredictRemoteInput(Godot.Collections.Dictionary previousInput, int ticksSinceRealInput);
    }

    public interface INetworkSerializable : INetworkSync
    {
        Godot.Collections.Dictionary _SaveState();
        void _LoadState(Godot.Collections.Dictionary state);
    }

    public interface INetworkSpawnPreProcess
    {
        void _NetworkSpawnPreProcess(Godot.Collections.Dictionary data);
    }

    public interface INetworkSpawn
    {
        void _NetworkSpawn(Godot.Collections.Dictionary data);
    }

    public interface INetworkDespawn
    {
        void _NetworkDespawn();
    }
}
