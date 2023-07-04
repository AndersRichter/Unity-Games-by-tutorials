using UnityEngine;
using UnityEngine.UI;

namespace UI.Widgets
{
    public class CustomButton : Button
    {
        [SerializeField] private GameObject _normalText;
        [SerializeField] private GameObject _pressedText;

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);
            
            _normalText.SetActive(state != SelectionState.Pressed);
            _pressedText.SetActive(state == SelectionState.Pressed);
        }
    }
}