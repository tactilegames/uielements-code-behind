#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;

internal static class UIPropertyTypes
{
    private static readonly List<Type> uiElementTypes = TypeCache.GetTypesDerivedFrom<ITransform>().ToList();

    public static Type GetUIElementType(string uiElementName)
    {
        return uiElementTypes.First(type => type.Name == uiElementName);
    }
}
#endif