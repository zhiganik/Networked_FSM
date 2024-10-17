using Fusion;

namespace Shakhtarsk.Network.Events
{
    public class NetworkInputEventSender : NetworkedEventSender<NetworkPlayerInputArgs>
    {
        public override void Init(NetworkEvents events)
        {
            base.Init(events);
            
            events.OnInput.AddListener(Call);
            
        }

        private void Call(NetworkRunner runner, NetworkInput input)
        {
            Invoke(new NetworkPlayerInputArgs {Runner = runner, Input = input});
        }
    }
    
    public struct NetworkPlayerInputArgs
    {
        public NetworkRunner Runner;
        public NetworkInput Input;
    }
}