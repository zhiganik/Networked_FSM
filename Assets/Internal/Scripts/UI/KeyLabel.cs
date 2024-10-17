using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Internal.Scripts.UI
{
    public class KeyLabel : MonoBehaviour
    {
        [SerializeField] private InputActionReference inputActionReference;
        [SerializeField] private TMP_Text label;
        [SerializeField] private Vector2 labelSpacing;

        private RectTransform _rectTransform;
        
        private void OnEnable()
        {
            _rectTransform ??= transform as RectTransform;
            
            label.SetText(inputActionReference.action.GetBindingDisplayString());
            
            var preferredValues = label.GetPreferredValues();
            
            _rectTransform.sizeDelta = preferredValues + labelSpacing;
            label.rectTransform.sizeDelta = preferredValues;
            label.rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}