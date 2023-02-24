using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Eventer : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UnityEvent onMouseClicked;

    // works with UI
    public void OnPointerClick(PointerEventData eventData)
    {
        onMouseClicked?.Invoke();
    }
}
