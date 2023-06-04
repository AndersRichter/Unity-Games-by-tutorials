using System.Collections;
using UnityEngine;

namespace Components
{
    [RequireComponent(typeof(Collider2D))]
    public class DisableColliderForTimeComponent : MonoBehaviour
    {
        [SerializeField] private float _disableTime = 0.3f;

        private Collider2D _collider;
        private Coroutine _coroutine;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        public void DisableCollider()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _coroutine = StartCoroutine(ToggleCollider());
        }

        private IEnumerator ToggleCollider()
        {
            _collider.enabled = false;
            yield return new WaitForSeconds(_disableTime);
            _collider.enabled = true;
        }
    }
}