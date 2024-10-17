using DG.Tweening;
using R3;
using Shakhtarsk.Characters.Player.Features.Processors;
using Shakhtarsk.Characters.Player.Network;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Shakhtarsk.Characters.Player.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class StaminaRenderer : MonoBehaviour
    {
        [SerializeField] private Image staminaBar;
        [SerializeField] private CanvasGroup canvasGroup;
        
        private PlayerMechanism _playerMechanism;
        private Tween _currentTween;
        
        [Inject]
        private void Construct(Spawner playerSpawner)
        {
            playerSpawner.PlayerSpawned += OnPlayerSpawned;
        }

        private void OnPlayerSpawned(PlayerResolver resolver)
        {
            _playerMechanism = resolver.Resolve<PlayerMechanism>();
            var staminaProcessor = _playerMechanism.Kcc.GetProcessor<StaminaProcessor>();
            staminaProcessor.InProgress.Subscribe(AnimateShow).AddTo(this);
            staminaProcessor.StaminaNormalized.Subscribe(UpdateStamina).AddTo(this);
            
        }

        private void UpdateStamina(float value)
        {
            staminaBar.fillAmount = value;
        }

        private void AnimateShow(bool state)
        {
            if (_currentTween != null)
            {
                _currentTween.Pause();
            }
            
            var endValue = state ? 1f : 0f;
            _currentTween = canvasGroup.DOFade(endValue, 0.5f);
        }
    }
}