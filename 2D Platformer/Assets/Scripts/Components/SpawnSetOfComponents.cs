using System;
using System.Linq;
using UnityEngine;

namespace Components
{
    public class SpawnSetOfComponents : MonoBehaviour
    {
        [SerializeField] private SpawnItem[] spawnItems;

        public void SpawnAll()
        {
            foreach (var item in spawnItems)
            {
                item.ComponentToSpawn.Spawn();
            }
        }

        public void Spawn(string itemName)
        {
            spawnItems.FirstOrDefault(item => item.Name == itemName)?.ComponentToSpawn.Spawn();
        }

        [Serializable]
        public class SpawnItem
        {
            public string Name;
            public SpawnComponent ComponentToSpawn;
        }
    }
}