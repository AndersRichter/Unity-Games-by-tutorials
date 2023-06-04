using Creatures.Hero;
using Model.Definitions;
using UnityEngine;

namespace Components
{
    public class InventoryUpdateComponent : MonoBehaviour
    {
        [InventoryId] [SerializeField] private string _inventoryId;
        [SerializeField] private int _value;

        public void Add(GameObject gameObj)
        {
            var hero = gameObj.GetComponent<Hero>();

            if (hero != null)
            {
                hero.AddToInventory(_inventoryId, _value);
            }
        }
    }
}