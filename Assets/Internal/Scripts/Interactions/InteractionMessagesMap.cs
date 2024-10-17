using System;
using System.Collections.Generic;
using System.Linq;
using Shakhtarsk.Runtime.SerializableTypes;
using UnityEngine;

namespace Shakhtarsk.Interactions
{
    [CreateAssetMenu(menuName = "Shakhtarsk/ScriptableObject/Interaction/Messages Map", fileName = "Messages Map")]
    public class InteractionMessagesMap : ScriptableObject
    {
        [SerializeField] private string fallbackMessage;
        [SerializeField] private MessagePair[] messages;

        private Dictionary<Type, string> _messageMap;
        
        private void Init()
        {
            _messageMap = messages.ToDictionary(item => item.Type, item => item.Message);
        }
        
        public string GetInteractionMessage(IInteractable interactable)
        {
            if (interactable == null) return string.Empty;
            if (_messageMap == null) Init();
            
            var type = interactable.GetType();

            return _messageMap.GetValueOrDefault(type, fallbackMessage);
        }

        [Serializable]
        public struct MessagePair
        {
            [SerializeField, TypeFilter(typeof(IInteractable))] private SerializableType typeReference;
            [SerializeField] private string message;


            public Type Type => typeReference.Type;
            public string Message => message;
        }
    }
   

    






}