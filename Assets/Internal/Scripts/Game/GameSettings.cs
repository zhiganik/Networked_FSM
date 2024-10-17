using UnityEngine;
using Zenject;

namespace Shakhtarsk.Game
{
    public class GameSettings : IInitializable
    {
        public void Initialize()
        {
            var maxResolution = Screen.resolutions[^1];
            Screen.SetResolution(maxResolution.width / 2, maxResolution.height / 2, false);
        }
    }
}