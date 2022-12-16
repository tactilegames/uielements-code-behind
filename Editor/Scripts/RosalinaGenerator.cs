using Rosalina.Generator;

#if UNITY_EDITOR
internal static class RosalinaGenerator
{
    /// <summary>
    /// Generates a C# script containing the bindings of the given UI document.
    /// </summary>
    /// <param name="document">UI Document.</param>
    /// <param name="outputFileName">C# script output file.</param>
    /// <returns>Rosalina generation result.</returns>
    public static RosalinaGenerationResult GenerateBindings(UIDocumentAsset document, string outputFileName) {
        return new CodeGenerator().Generate(document, outputFileName);
    }

}
#endif