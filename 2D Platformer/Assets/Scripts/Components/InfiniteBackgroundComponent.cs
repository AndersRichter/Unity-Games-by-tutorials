using UnityEngine;
using Utils;

namespace Components
{
    public class InfiniteBackgroundComponent : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _container;

        // Bounds - special struct for working with bounds - game object borders/frames/sizes
        private Bounds _containerBounds;
        private Bounds _allBounds;
        
        private Vector3 _boundsToTransformDelta;
        private Vector3 _containerDelta;
        private Vector3 _screenSize;

        private void Start()
        {
            var sprites = _container.GetComponentsInChildren<SpriteRenderer>();

            foreach (var sprite in sprites)
            {
                // summarize all children bounds
                // large borders extend the current total border
                _containerBounds.Encapsulate(sprite.bounds);
            }

            _allBounds = _containerBounds;

            _boundsToTransformDelta = transform.position - _allBounds.center;
            _containerDelta = _container.position - _allBounds.center;
        }
        
        private void LateUpdate()
        {
            var min = _camera.ViewportToWorldPoint(Vector3.zero);
            var max = _camera.ViewportToWorldPoint(Vector3.one);
            _screenSize = new Vector3(max.x - min.x, max.y - min.y);

            _allBounds.center = transform.position - _boundsToTransformDelta;
            var cameraPosition = _camera.transform.position.x;
            var screenLeft = new Vector3(cameraPosition - _screenSize.x / 2, _containerBounds.center.y);
            var screenRight = new Vector3(cameraPosition + _screenSize.x / 2, _containerBounds.center.y);

            if (!_allBounds.Contains(screenLeft))
            {
                InstantiateBackground(_allBounds.min.x - _containerBounds.extents.x);
            }
            
            if (!_allBounds.Contains(screenRight))
            {
                InstantiateBackground(_allBounds.max.x + _containerBounds.extents.x);
            }
        }

        private void InstantiateBackground(float boundCenterX)
        {
            var newBounds = new Bounds(new Vector3(boundCenterX, _containerBounds.center.y), _containerBounds.size);
            _allBounds.Encapsulate(newBounds);

            _boundsToTransformDelta = transform.position - _allBounds.center;
            var newContainerXPosition = boundCenterX + _containerDelta.x;
            var newPosition = new Vector3(newContainerXPosition, _container.transform.position.y);

            Instantiate(_container, newPosition, Quaternion.identity, transform);
        }

        private void OnDrawGizmosSelected()
        {
            GizmoUtils.DrawBounds(_allBounds, Color.magenta);
        }
    }
}