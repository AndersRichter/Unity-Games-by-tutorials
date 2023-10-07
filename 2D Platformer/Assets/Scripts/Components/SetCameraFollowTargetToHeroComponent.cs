using Cinemachine;
using Creatures.Hero;
using UnityEngine;

namespace Components
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class SetCameraFollowTargetToHeroComponent : MonoBehaviour
    {
        private void Start()
        {
            var vCamera = GetComponent<CinemachineVirtualCamera>();
            vCamera.Follow = FindObjectOfType<Hero>().transform;
        }
    }
}