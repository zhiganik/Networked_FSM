using Fusion;

namespace Shakhtarsk.Network.Events
{
    public class PlayerLeftSender : NetworkedEventSender<PlayerLeftEvent>
    {
        public override void Init(NetworkEvents events)
        {
            base.Init(events);
            
            events.PlayerLeft.AddListener(Call);
            
        }

        private void Call(NetworkRunner runner, PlayerRef playerRef)
        {
            Invoke(new PlayerLeftEvent {Runner = runner, PlayerRef = playerRef});
        }
    }
    
    public struct PlayerLeftEvent
    {
        public NetworkRunner Runner;
        public PlayerRef PlayerRef;
    }
}