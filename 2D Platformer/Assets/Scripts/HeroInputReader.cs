using UnityEngine;
using UnityEngine.InputSystem;

public class HeroInputReader : MonoBehaviour
{
    [SerializeField] private Hero hero;

    public void OnMovement(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        hero.SetDirection(direction);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            hero.Attack();
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            hero.Interact();
        }
    }
}
