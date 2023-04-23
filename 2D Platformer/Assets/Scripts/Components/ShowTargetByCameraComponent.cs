using Cinemachine;
using UnityEngine;

namespace Components
{
    public class ShowTargetByCameraComponent : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private CameraAnimationComponent cameraAnimation;
        [SerializeField] private float delayToRollback;

        // Works like a fallback and only in the editor
        private void OnValidate()
        {
            if (cameraAnimation == null)
            {
                cameraAnimation = FindObjectOfType<CameraAnimationComponent>();
            }
        }

        public void ShowTarget()
        {
            cameraAnimation.SetInitialPosition(target.position);
            cameraAnimation.SetAnimationState(true);
            Invoke(nameof(MoveBack), delayToRollback);
        }

        private void MoveBack()
        {
            cameraAnimation.SetAnimationState(false);
        }
    }
}