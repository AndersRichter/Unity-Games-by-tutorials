using UnityEngine;
using UnityEngine.InputSystem;

namespace Creatures.Hero
{
    public class HeroInputEnableDisable : MonoBehaviour
    {
        private PlayerInput _input;
        
        private void Start()
        {
            _input = FindObjectOfType<PlayerInput>();
        }
        
        public void SetInput(bool isEnabled)
        {
            _input.enabled = isEnabled;
        }
    }
}