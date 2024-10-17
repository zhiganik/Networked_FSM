using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Shakhtarsk.Network.Events
{
    public class SessionListEventSender : NetworkedEventSender<SessionList>
    {
        public override void Init(NetworkEvents events)
        {
            base.Init(events);
            
            events.OnSessionListUpdate.AddListener(Call);
            
        }

        private void Call(NetworkRunner runner, List<SessionInfo> sessionInfos)
        {
            Invoke(new SessionList {SessionInfos = sessionInfos});
        }
    }

    public struct SessionList
    {
        public List<SessionInfo> SessionInfos;
    }
}