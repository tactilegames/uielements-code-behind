#if UNITY_EDITOR
using System.IO;
using System.Xml.Linq;

internal class RosalinaUXMLParser
{
    private const string NameAttribute = "name";
    private const string GENERATE_CODE_BEHIND = "tactile-code-behind";
    private const string GENERATED_NAMESPACE = "tactile-namespace";

    /// <summary>
    /// Parses the given UI document path.
    /// </summary>
    /// <param name="uiDocumentPath">UI Document path.</param>
    /// <returns>The UXML document.</returns>
    public static UxmlDocument ParseUIDocument(string uiDocumentPath)
    {
        using FileStream documentStream = File.OpenRead(uiDocumentPath);
        XElement root = XElement.Load(documentStream);
        UxmlNode rootNode = ParseUxmlNode(root);

        return new UxmlDocument(Path.GetFileName(uiDocumentPath), uiDocumentPath, rootNode);
    }

    private static UxmlNode ParseUxmlNode(XElement xmlNode)
    {
        string type = xmlNode.Name.LocalName;
        string name = xmlNode.Attribute(NameAttribute)?.Value ?? string.Empty;
        var node = new UxmlNode(type, name, xmlNode.Parent is null);

        if (node.IsRoot) {
            node.GenerateCodeBehind = xmlNode.Attribute(XName.Get(GENERATE_CODE_BEHIND)) != null;
            node.Namespace = xmlNode.Attribute(XName.Get(GENERATED_NAMESPACE))?.Value;
        } else {
            node.Namespace = xmlNode.Name.NamespaceName;
        }
        
        if (xmlNode.HasElements)
        {
            foreach (XElement xmlElement in xmlNode.Elements())
            {
                node.Children.Add(ParseUxmlNode(xmlElement));
            }
        }

        return node;
    }
}
#endif
