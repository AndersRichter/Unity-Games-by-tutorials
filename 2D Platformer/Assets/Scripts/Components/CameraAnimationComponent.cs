using Cinemachine;
using UnityEngine;

namespace Components
{
    public class CameraAnimationComponent : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;

        private static readonly int AnimatorIsShowing = Animator.StringToHash("isShowing");

        public void SetInitialPosition(Vector3 position)
        {
            virtualCamera.transform.position = new Vector3(position.x, position.y, virtualCamera.transform.position.z);
        }

        public void SetAnimationState(bool value)
        {
            animator.SetBool(AnimatorIsShowing, value);
        }
    }
}