using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// all code in Editor folder will be removed in prod build
namespace Model.Definitions.Editor
{
    // it will affect only fields marked with special attribute
    [CustomPropertyDrawer(typeof(InventoryIdAttribute))]
    public class InventoryIdAttributeDrawer : PropertyDrawer
    {
        // we want to create dropdown with all possible values for inventory ids fields (example - InventoryUpdateComponent)
        // all ids are collected dynamically from DefinitionsFacade -> InventoryItemsDefinition (ItemsDefinition)
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var definitions = DefinitionsFacade.Instance.ItemsDefinition.ItemsDefinitionsForEditor;
            var ids = new List<string>();

            foreach (var definition in definitions)
            {
                ids.Add(definition.Id);
            }

            var selectedIndex = Mathf.Max(ids.IndexOf(property.stringValue), 0);
            selectedIndex = EditorGUI.Popup(position, property.displayName, selectedIndex, ids.ToArray());
            property.stringValue = ids[selectedIndex];
        }
    }
}