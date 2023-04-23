using UnityEngine;

namespace Components
{
    public class SwitchAnimationComponent : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private bool state;
        [SerializeField] private string _animationKey;

        public void Switch()
        {
            state = !state;
            animator.SetBool(_animationKey, state);
        }

        [ContextMenu("Switch")]
        public void SwitchIt()
        {
            Switch();
        }
    }
}
