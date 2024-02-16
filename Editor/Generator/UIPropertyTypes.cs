using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;

namespace TactileModules.UIElementsCodeBehind {

    internal static class UIPropertyTypes
    {
        private static readonly List<Type> uiElementTypes = TypeCache.GetTypesDerivedFrom<ITransform>().ToList();
        
        public static Type GetUIElementType(string uiElementName) {
            if (uiElementName == "Template" || uiElementName == "Instance") {
                return typeof(VisualElement);
            }
            
            var uiElementType = uiElementTypes.FirstOrDefault(type => type.Name == uiElementName);
            return uiElementType ?? uiElementTypes.First(type => type.FullName == uiElementName);
        }
    }

}