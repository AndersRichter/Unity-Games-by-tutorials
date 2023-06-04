using UnityEngine;

// The idea of Inventory is to divide definitions of stored objects and stored objects
// Here we define DefinitionsFacade class as ScriptableObject - so we can create it as a resource in Unity inspector
// It is an entry point for all definitions, using Singleton pattern
namespace Model.Definitions
{
    [CreateAssetMenu(menuName = "Definitions/DefinitionsFacade", fileName = "DefinitionsFacade")]
    public class DefinitionsFacade : ScriptableObject
    {
        [SerializeField] private InventoryItemsDefinition _itemsDefinition;

        // Public field, but it is not accessible through the DefinitionsFacade.ItemsDefinition because it is not static.
        // So the right way is DefinitionsFacade.Instance.ItemsDefinition - supporting of Singleton pattern
        public InventoryItemsDefinition ItemsDefinition => _itemsDefinition;

        private static DefinitionsFacade _instance;

        // Singleton pattern - when we always return the same class instance when it is requested
        public static DefinitionsFacade Instance => _instance == null ? LoadDefinitions() : _instance;

        private static DefinitionsFacade LoadDefinitions()
        {
            // Loading facade as a resource
            _instance = Resources.Load<DefinitionsFacade>("DefinitionsFacade");
            return _instance;
        }

        public static bool IsItemDefinitionExists(string id)
        {
            var itemDefinition = Instance.ItemsDefinition.GetFirstOrDefault(id);
            return !itemDefinition.IsVoid;
        }
        
        public static int GetMaxInStack(string id)
        {
            return Instance.ItemsDefinition.GetFirstOrDefault(id).MaxInStack;
        }
    }
}