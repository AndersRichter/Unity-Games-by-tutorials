using UnityEngine;

namespace Utils
{
    public class SpawnUtils
    {
        private const string SpawnContainerName = "### SPAWN ###";

        public static GameObject Spawn(GameObject prefab, Vector3 position, string containerName = SpawnContainerName)
        {
            var spawnContainer = GameObject.Find(containerName);

            if (!spawnContainer)
                spawnContainer = new GameObject(containerName);

            return Object.Instantiate(prefab, position, Quaternion.identity, spawnContainer.transform);
        }
    }
}