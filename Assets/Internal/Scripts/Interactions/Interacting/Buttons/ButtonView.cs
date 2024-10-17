using DG.Tweening;
using Shakhtarsk.Interactions.View;
using UnityEngine;

namespace Shakhtarsk.Interactions
{
    public class ButtonView : InteractableView<Button>
    {
        [SerializeField] private Transform pressableElement;
        [SerializeField] private float pressStrength;
        
        
        protected override void Initialize()
        {
            interactable.LocalClicked += OnClicked;
        }

        private void OnClicked()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(pressableElement.DOLocalMove(Vector3.down * pressStrength, 0.2f));
            sequence.Append(pressableElement.DOLocalMove(Vector3.zero, 0.2f));
        }
    }
}