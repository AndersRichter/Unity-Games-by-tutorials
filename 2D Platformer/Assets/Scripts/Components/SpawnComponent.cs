using UnityEngine;
using Utils;
using Utils.ObjectPool;

namespace Components
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform destination;
        [SerializeField] private GameObject prefab;
        [SerializeField] private bool _usePool;
        [SerializeField] private bool _shouldScale = true;

        [ContextMenu("Spawn")]
        public GameObject Spawn()
        {
            var instantiate = _usePool
                ? Pool.Instance.Get(prefab, destination.position)
                : SpawnUtils.Spawn(prefab, destination.position);

            if (_shouldScale)
            {
                instantiate.transform.localScale = destination.lossyScale;
            }
            return instantiate;
        }

        public void SpawnNoReturn()
        {
            Spawn();
        }

        public GameObject Spawn(GameObject newPrefab)
        {
            prefab = newPrefab;
            return Spawn();
        }
    }
}
