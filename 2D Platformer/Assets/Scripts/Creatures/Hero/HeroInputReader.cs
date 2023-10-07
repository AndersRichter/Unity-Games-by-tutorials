using UnityEngine;
using UnityEngine.InputSystem;

namespace Creatures.Hero
{
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
            if (context.performed)
            {
                hero.Attack();
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                hero.Interact();
            }
        }
        
        public void OnThrow(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (context.duration >= 1f)
                {
                    hero.LongThrow();
                }
                else
                {
                    hero.Throw();
                }
            }
        }

        public void OnPlatformDrop(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                hero.DropFromPlatform();
            }
        }
        
        public void OnTakeHeal(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                hero.TakeHeal();
            }
        }
        
        public void OnNextItemQuickInventory(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                hero.NextItemQuickInventory();
            }
        }
        
        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                hero.Sprint();
            }
        }
    }
}
