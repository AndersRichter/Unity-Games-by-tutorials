using UnityEngine;

namespace Components
{
    public class FollowTargetComponent : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float damping;
        [SerializeField] private Vector2 positionCorrection;

        // using LateUpdate because we need to move object after target and whole scene were updated
        private void LateUpdate()
        {
            if (target)
            {
                var destination = new Vector3(target.position.x + positionCorrection.x, target.position.y + positionCorrection.y, transform.position.z);
                // interpolation - adds more smooth movement
                transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * damping);
            }
        }

        public void StartStopFollowing()
        {
            enabled = !enabled;
        }
    }
}
