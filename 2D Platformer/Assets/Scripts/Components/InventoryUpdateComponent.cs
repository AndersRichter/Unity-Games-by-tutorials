using Model.Data;
using Model.Definitions;
using UnityEngine;
using Utils;

namespace Components
{
    public class InventoryUpdateComponent : MonoBehaviour
    {
        [InventoryId] [SerializeField] private string _inventoryId;
        [SerializeField] private int _value;

        public void Add(GameObject gameObj)
        {
            var hero = gameObj.GetInterfaceExtension<ICanAddInInventory>();
            hero?.AddToInventory(_inventoryId, _value);
        }
    }
}