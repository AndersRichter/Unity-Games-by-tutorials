using Creatures.Hero;
using UnityEngine;

namespace Components
{
    public class FollowHeroComponent : MonoBehaviour
    {
        [SerializeField] private float damping;
        [SerializeField] private Vector2 positionCorrection;
        
        private Transform _target;

        private void Start() {
            _target = FindObjectOfType<Hero>().transform;
        }

        // using LateUpdate because we need to move object after target and whole scene were updated
        private void LateUpdate()
        {
            if (_target)
            {
                var destination = new Vector3(_target.position.x + positionCorrection.x, _target.position.y + positionCorrection.y, transform.position.z);
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
