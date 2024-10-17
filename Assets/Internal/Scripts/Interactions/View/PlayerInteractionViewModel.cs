using R3;
using Shakhtarsk.Characters.Player;
using Shakhtarsk.Characters.Player.Network;
using TMPro;
using UnityEngine;
using Zenject;

namespace Shakhtarsk.Interactions.View
{
    public class PlayerInteractionViewModel : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TMP_Text messageTMP;
        
        private PlayerInteraction _interaction;
        private InteractionMessagesMap _messagesMap;
        
        [Inject]
        private void Construct(Spawner playerSpawner, InteractionMessagesMap messagesMap)
        {
            _messagesMap = messagesMap;
            playerSpawner.PlayerSpawned += OnPlayerSpawned;
        }

        private void OnPlayerSpawned(PlayerResolver resolver)
        {
            _interaction = resolver.Resolve<PlayerInteraction>();
            _interaction.FocusStateChanged += OnFocusStateChanged;
            OnVisible(false);
        }

        private void OnFocusStateChanged(bool value)
        {
            OnVisible(value);
            OnMessage(_messagesMap.GetInteractionMessage(_interaction.FocusedInteractable));
        }
        
        private void OnVisible(bool value)
        {
            canvasGroup.alpha = value ? 1 : 0;
            canvasGroup.blocksRaycasts = value;
        }

        private void OnMessage(string text)
        {
            messageTMP.text = text;
        }
    }
}