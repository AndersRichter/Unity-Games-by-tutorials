using UnityEngine;

namespace Model.Definitions
{
    [CreateAssetMenu(menuName = "Definitions/PlayerCharacteristics", fileName = "PlayerCharacteristics")]

    public class PlayerCharacteristicsDefinition : ScriptableObject
    {
        [SerializeField] private int _inventorySize;
        [SerializeField] private int _maxHealth;

        public int InventorySize => _inventorySize;
        public int MaxHealth => _maxHealth;
    }
}