using UnityEngine;

namespace Components
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform destination;
        [SerializeField] private GameObject prefab;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            var instantiate = Instantiate(prefab, destination.position, Quaternion.identity);
            instantiate.transform.localScale = destination.lossyScale;
        }
    }
}
