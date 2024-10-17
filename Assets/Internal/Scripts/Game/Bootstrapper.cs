using System;
using Shakhtarsk.Network;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Shakhtarsk.Game
{
    public class Bootstrapper : MonoBehaviour
    {
        private NetworkManager _network;
        
        [Inject]        
        private void Construct(NetworkManager network)
        {
            _network = network;
        }

        private void Awake()
        {
            Init();
        }

        private async void Init()
        {
            var success = await _network.JoinSession();

            if (success)
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                //todo
            }
        }
    }
}