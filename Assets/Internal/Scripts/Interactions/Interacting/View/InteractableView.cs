using UnityEngine;

namespace Shakhtarsk.Interactions.View
{
    public abstract class InteractableView<T> : MonoBehaviour where T : Interactable
    {
        [SerializeField] protected T interactable;
        [SerializeField] private Highlighter highlighter;

        private void Awake()
        {
            interactable.FocusChanged += OnFocusChanged;
            OnFocusChanged(false);
            Initialize();
        }

        private void OnFocusChanged(bool value)
        {
            highlighter.Highlight(value);
        }
        
        protected virtual void Initialize() { }
    }
}