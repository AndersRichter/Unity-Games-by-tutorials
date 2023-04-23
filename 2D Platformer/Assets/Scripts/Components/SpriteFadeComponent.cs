using UnityEngine;

namespace Components
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteFadeComponent : MonoBehaviour
    {
        [SerializeField] private float speed;

        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            var currentAlpha = _spriteRenderer.color.a;
            var newAlpha = Mathf.Lerp(currentAlpha, 0f, Time.deltaTime * speed);
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, newAlpha);
            enabled = false;
        }
    }
}
