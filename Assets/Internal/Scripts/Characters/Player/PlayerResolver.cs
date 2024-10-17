using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

namespace Shakhtarsk.Characters.Player
{
    public class PlayerResolver : NetworkBehaviour
    {
        private Dictionary<Type, IPlayerComponent> _playerComponents;
        
        private void Awake()
        {
            var list = new List<IPlayerComponent>();
            list.AddRange(GetComponentsInChildren<IPlayerComponent>());
            _playerComponents = list.ToDictionary(item => item.GetType());
        }

        public T Resolve<T>() where T : class, IPlayerComponent
        {
            var type = typeof(T);
            return _playerComponents.GetValueOrDefault(type, null) as T;
        }
    }
}