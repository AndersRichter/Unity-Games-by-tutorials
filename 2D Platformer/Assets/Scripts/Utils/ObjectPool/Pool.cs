using System.Collections.Generic;
using UnityEngine;

namespace Utils.ObjectPool
{
    public class Pool : MonoBehaviour
    {
        private readonly Dictionary<int, Queue<PoolItem>> _items = new();

        private static Pool _instance;

        public static Pool Instance
        {
            get
            {
                if (!_instance)
                {
                    var gameObj = new GameObject("### POOL ###");
                    _instance = gameObj.AddComponent<Pool>();
                }

                return _instance;
            }
        }

        public GameObject Get(GameObject gameObj, Vector3 position)
        {
            var id = gameObj.GetInstanceID();
            var queue = RequireQueue(id);

            if (queue.Count > 0)
            {
                var pooledItem = queue.Dequeue();
                pooledItem.transform.position = position;
                pooledItem.gameObject.SetActive(true);
                pooledItem.Restart();
                return pooledItem.gameObject;
            }

            var instance = SpawnUtils.Spawn(gameObj, position, gameObject.name);
            var poolItem = instance.GetComponent<PoolItem>();
            poolItem.Retain(id, this);

            return instance;
        }

        private Queue<PoolItem> RequireQueue(int id)
        {
            if (!_items.TryGetValue(id, out var queue))
            {
                queue = new();
                _items.Add(id, queue);
            }

            return queue;
        }

        public void Release(int id, PoolItem poolItem)
        {
            var queue = RequireQueue(id);
            queue.Enqueue(poolItem);
            poolItem.gameObject.SetActive(false);
        }
    }
}