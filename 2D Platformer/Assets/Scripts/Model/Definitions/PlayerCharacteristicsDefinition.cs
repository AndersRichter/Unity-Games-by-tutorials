using UnityEngine;

namespace Model.Definitions
{
    [CreateAssetMenu(menuName = "Definitions/PlayerCharacteristics", fileName = "PlayerCharacteristics")]

    public class PlayerCharacteristicsDefinition : ScriptableObject
    {
        [SerializeField] private int _inventorySize;

        public int InventorySize => _inventorySize;
    }
}