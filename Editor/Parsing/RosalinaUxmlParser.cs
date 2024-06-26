﻿using System.IO;
using System.Xml.Linq;

namespace TactileModules.UIElementsCodeBehind {

    internal static class RosalinaUxmlParser
    {
        private const string NAME_ATTRIBUTE = "name";
        private const string GENERATE_CODE_BEHIND = "tactile-code-behind";
        private const string GENERATED_NAMESPACE = "tactile-namespace";
        private const string RESOURCE_PATH = "tactile-resource-path";
        private const string STYLESHEETS_PATH = "tactile-stylesheet-path";
        private const string EVENT_NAME = "tactile-event-name";

        /// <summary>
        /// Parses the given UI document path.
        /// </summary>
        /// <param name="uiDocumentPath">UI Document path.</param>
        /// <returns>The UXML document.</returns>
        public static UxmlDocument ParseUIDocument(string uiDocumentPath)
        {
            using var documentStream = File.OpenRead(uiDocumentPath);
            var root = XElement.Load(documentStream);
            var rootNode = ParseUxmlNode(root);

            return new UxmlDocument(Path.GetFileName(uiDocumentPath), uiDocumentPath, rootNode);
        }

        private static UxmlNode ParseUxmlNode(XElement xmlNode)
        {
            var type = xmlNode.Name.LocalName;
            var name = xmlNode.Attribute(NAME_ATTRIBUTE)?.Value ?? string.Empty;
            var node = new UxmlNode(UIPropertyTypes.GetUIElementType(type), name, xmlNode.Parent is null);

            if (node.IsRoot) {
                node.GenerateCodeBehind = xmlNode.Attribute(XName.Get(GENERATE_CODE_BEHIND)) != null;
                node.Namespace = xmlNode.Attribute(XName.Get(GENERATED_NAMESPACE))?.Value;
                node.ResourcePath = xmlNode.Attribute(XName.Get(RESOURCE_PATH))?.Value;
                node.StylesheetsPath = xmlNode.Attribute(XName.Get(STYLESHEETS_PATH))?.Value;
            } else {
                node.Namespace = xmlNode.Name.NamespaceName;
                node.EventName = xmlNode. Attribute(XName.Get(EVENT_NAME))?.Value;
            }
        
            if (xmlNode.HasElements)
            {
                foreach (var xmlElement in xmlNode.Elements())
                {
                    node.Children.Add(ParseUxmlNode(xmlElement));
                }
            }

            return node;
        }
    }

}