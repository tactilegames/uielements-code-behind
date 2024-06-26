using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TactileModules.UIElementsCodeBehind {

    public class RosalinaAssetProcessor : AssetPostprocessor
    {
        private const string UI_DOCUMENT_EXTENSION = ".uxml";

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromPath)
        {
            var uiFilesChanged = importedAssets
                .Where(x => x.StartsWith("Assets") || x.StartsWith("Packages"))
                .Where(x => Path.GetExtension(x) == UI_DOCUMENT_EXTENSION)
                .ToArray();

            if (uiFilesChanged.Length <= 0) {
                return;
            }

            for (var i = 0; i < uiFilesChanged.Length; i++)
            {
                var uiDocumentPath = uiFilesChanged[i];

                if (PackageSupport.IsFileInPackage(uiDocumentPath)) {
                    if (!PackageSupport.IsPackageEmbedded(uiDocumentPath)) {
                        continue;
                    }
                }
            
                var document = new UIDocumentAsset(uiDocumentPath);

                try
                {
                    EditorUtility.DisplayProgressBar("Rosalina", $"Generating {document.Name} bindings...", GeneratePercentage(i, uiFilesChanged.Length));
                    Debug.Log($"[Rosalina]: Generating UI bindings for {uiDocumentPath}");

                    var result = RosalinaGenerator.GenerateBindings(document, $"{document.Name}.g.cs");
                    if (result.Code == null) {
                        Debug.Log($"[Rosalina]: Skipping: {document.Name}");
                        continue;
                    }
                    result.Save();

                    Debug.Log($"[Rosalina]: Done generating: {document.Name} (output: {result.OutputFilePath})");
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }

            Debug.Log("[Rosalina]: Done.");
            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
        }

        private static int GeneratePercentage(int value, int total) => Mathf.Clamp((value / total) * 100, 0, 100);
    }

}