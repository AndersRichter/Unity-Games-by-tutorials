using UnityEngine;

namespace Components
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform destination;
        [SerializeField] private GameObject prefab;

        private const string SpawnContainerName = "### SPAWN ###";

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            var spawnContainer = GameObject.Find(SpawnContainerName);

            if (!spawnContainer)
                spawnContainer = new GameObject(SpawnContainerName);

            var instantiate = Instantiate(prefab, destination.position, Quaternion.identity, spawnContainer.transform);
            instantiate.transform.localScale = destination.lossyScale;
        }

        public void Spawn(GameObject newPrefab)
        {
            prefab = newPrefab;
            Spawn();
        }
    }
}
