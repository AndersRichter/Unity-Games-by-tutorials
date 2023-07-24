using System;
using UnityEditor;

namespace Utils.Editor
{
    public static class SerializedPropertyExtensions
    {
        public static bool GetEnum<TEnum>(this SerializedProperty property, out TEnum returnedValue) where TEnum : Enum
        {
            returnedValue = default;
            var names = property.enumNames;

            if (names == null || names.Length == 0)
                return false;

            var enumName = names[property.enumValueIndex];
            returnedValue = (TEnum) Enum.Parse(typeof(TEnum), enumName);

            return true;
        }
    }
}