using System.Collections;
using UnityEngine;

namespace Components
{
    public class TeleportComponent : MonoBehaviour
    {
        [SerializeField] private Transform _destination;
        [SerializeField] private float _disappearingTime;
        [SerializeField] private float _teleportingTime;

        public void Teleport(GameObject target)
        {
            StartCoroutine(TeleportingAnimation(target));
        }

        private IEnumerator TeleportingAnimation(GameObject target)
        {
            var sprite = target.GetComponent<SpriteRenderer>();
            var spriteAlpha = sprite.color.a;

            yield return AppearDisappearAnimation(sprite, 0);
            ToggleActiveState(target, false);
            yield return MoveAnimation(target);
            ToggleActiveState(target, true);
            yield return AppearDisappearAnimation(sprite, spriteAlpha);
        }

        private IEnumerator AppearDisappearAnimation(SpriteRenderer sprite, float finalAlpha)
        {
            if (sprite)
            {
                var alphaTime = 0f;
                var spriteAlpha = sprite.color.a;
            
                while (alphaTime < _disappearingTime)
                {
                    alphaTime += Time.deltaTime;
                    var progress = alphaTime / _disappearingTime;
                    var newAlpha = Mathf.Lerp(spriteAlpha, finalAlpha, progress);
                    var color = sprite.color;
                    color.a = newAlpha;
                    sprite.color = color;
            
                    // skip one frame and start next cycle iteration
                    yield return null;
                }
            }
        }
        
        private IEnumerator MoveAnimation(GameObject target)
        {
            var moveTime = 0f;
            var startPosition = target.transform.position;

            while (moveTime < _teleportingTime)
            {
                moveTime += Time.deltaTime;
                var progress = moveTime / _teleportingTime;
                target.transform.position = Vector3.Lerp(startPosition, _destination.position, progress);

                // skip one frame and start next cycle iteration
                yield return null;
            }
        }

        private void ToggleActiveState(GameObject target, bool isActive)
        {
            target.SetActive(isActive);
        }
    }
}
