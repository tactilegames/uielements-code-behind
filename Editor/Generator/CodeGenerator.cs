﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace TactileModules.UIElementsCodeBehind {

	public class CodeGenerator {

		private readonly Dictionary<Type, Type> eventTypes = new() {
			{typeof(Button), null},
			{typeof(Toggle), typeof(bool)},
			{typeof(TextField), typeof(string)},
			{typeof(Slider), typeof(float)},
			{typeof(ToolbarSearchField), typeof(string)}
		};
		
		private int indentation;
		
		internal RosalinaGenerationResult Generate(UIDocumentAsset uiDocumentAsset, string outputPath) {
			if (uiDocumentAsset is null) {
				throw new ArgumentNullException(nameof(uiDocumentAsset),
					"Cannot generate binding with an empty UI document definition.");
			}

			if (string.IsNullOrEmpty(outputPath)) {
				throw new ArgumentException("An output file name is required.", nameof(outputPath));
			}
			
			var uxml = RosalinaUxmlParser.ParseUIDocument(uiDocumentAsset.FullPath);
			if (!uxml.RootNode.GenerateCodeBehind) {
				return new RosalinaGenerationResult(null, null);
			}

			return new RosalinaGenerationResult(GenerateCode(uxml), CreateOutputFilePath(uiDocumentAsset, outputPath));
		}

		private string GenerateCode(UxmlDocument uxml) {
			return @$"//AUTO-GENERATED by com.tactilegames.uielements-code-behind
{GetImports(uxml)}

{GetNamespaceStart(uxml)}

{GetClassStart(uxml)}

{GenerateEvents(uxml)}

{GenerateProperties(uxml)}

{GenerateBindingsMethod(uxml)}

{GetClassEnd(uxml)}

{GetNamespaceEnd(uxml)}
";
		}

		private StringBuilder GetImports(UxmlDocument uxml) {
			var stringBuilder = new StringBuilder();
			
			stringBuilder.AppendLine("using System;");
			
			foreach (var @namespace in uxml.GetChildren().Select(node => node.Namespace).Distinct()) {
				if (string.IsNullOrWhiteSpace(@namespace)) {
					continue;
				}
				stringBuilder.AppendLine($"using {@namespace};");
			}

			return stringBuilder;
		}

		private string GetNamespaceStart(UxmlDocument uxmlDocument) {
			if (string.IsNullOrWhiteSpace(uxmlDocument.RootNode.Namespace)) {
				return "";
			}

			indentation++;
			return $"namespace {uxmlDocument.RootNode.Namespace}{{";
		}

		
		private string GetClassStart(UxmlDocument uxml) {
			var name = Path.GetFileNameWithoutExtension(uxml.Name);
			var classStart = $"{DoIndent()}public partial class {name} {{";
			indentation++;
			return classStart;
		}

		private string GenerateEvents(UxmlDocument uxml) {
			var stringBuilder = new StringBuilder();

			var nodes = uxml.GetChildren().Where(node => node.EventName != null).ToList();

			foreach (var node in nodes) {
				if(!eventTypes.TryGetValue(node.Type, out var eventType)) {
					throw new NotSupportedException($"Event generation for {node.Type} is not supported.");
				}

				stringBuilder.AppendLine(eventType == null
					? $"{DoIndent()}public event Action {node.EventName};"
					: $"{DoIndent()}public event Action<{eventType}> {node.EventName};");
			}
			
			return stringBuilder.ToString();	
		}
		
		private StringBuilder GenerateProperties(UxmlDocument uxml) {
			var stringBuilder = new StringBuilder();
			stringBuilder.Append(@$"{DoIndent()}public VisualElement RootVisualElement {{get; private set;}}");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			IEnumerable<UIProperty> properties = uxml.GetChildren().Select(x => new UIProperty(x.Type, x.Name)).ToList();

			foreach (var property in properties) {
				stringBuilder.AppendLine($"{DoIndent()}public {property.TypeName} {property.Name} {{get; private set;}}");
			}

			return stringBuilder;
		}
		
		private StringBuilder GenerateBindingsMethod(UxmlDocument uxml) {
            var stringBuilder = new StringBuilder();
			
            if (ShouldAutoLoad(uxml)) {
	            stringBuilder.AppendLine($"{DoIndent()}public VisualElement Initialize() {{");
                indentation++;
	            stringBuilder.AppendLine($"{DoIndent()}RootVisualElement = UnityEngine.Resources.Load<VisualTreeAsset>(\"{uxml.RootNode.ResourcePath}\").CloneTree();");
            } else {
				stringBuilder.AppendLine($"{DoIndent()}public void InitializeBindings(VisualElement rootVisualElement) {{");
				indentation++;
				stringBuilder.AppendLine($"{DoIndent()}RootVisualElement = rootVisualElement;");
			}

			stringBuilder.AppendLine();

			IEnumerable<UIProperty> properties = uxml.GetChildren().Select(x => new UIProperty(x.Type, x.Name)).ToList();
			foreach (var property in properties) {
				stringBuilder.AppendLine($"{DoIndent()}{property.Name} = RootVisualElement.Q<{property.TypeName}>(\"{property.OriginalName}\");");	
			}

			if (ShouldAutoApplyStyleSheets(uxml)) {
				stringBuilder.AppendLine();
				var styleSheets = uxml.RootNode.StylesheetsPath.Split(',').Select(s => s.Trim(' '));
				foreach (var styleSheet in styleSheets) {
					stringBuilder.AppendLine($"{DoIndent()}RootVisualElement.styleSheets.Add(UnityEngine.Resources.Load<StyleSheet>(\"{styleSheet}\"));");
				}
			}

			stringBuilder.AppendLine();
			foreach (var property in uxml.GetChildren().Where(node => node.EventName != null)) {
				if(!eventTypes.TryGetValue(property.Type, out var eventType)) {
					throw new NotSupportedException($"Event generation for {property.Type} is not supported.");
				}
 
				stringBuilder.AppendLine(eventType == null
					? $"{DoIndent()}{property.Name}.clicked += () => {property.EventName}?.Invoke();"
					: $"{DoIndent()}{property.Name}.RegisterValueChangedCallback(evt => {property.EventName}?.Invoke(evt.newValue));");
			}
			
			if (ShouldAutoLoad(uxml)) {
				stringBuilder.AppendLine();
				stringBuilder.AppendLine($"{DoIndent()}return RootVisualElement;");
			}
			
			indentation--;
			stringBuilder.Append($"{DoIndent()}}}");
			
			return stringBuilder;
		}

		private bool ShouldAutoApplyStyleSheets(UxmlDocument uxml) {
			return !string.IsNullOrWhiteSpace(uxml.RootNode.StylesheetsPath);
		}

		private static bool ShouldAutoLoad(UxmlDocument uxml) {
			return !string.IsNullOrWhiteSpace(uxml.RootNode.ResourcePath);
		}

		private string GenerateBindings(UxmlDocument uxml) {
			return "";
		}
		
		private string GetClassEnd(UxmlDocument uxml) {
			indentation--;
			return $"{DoIndent()}}}";
		}

		private string GetNamespaceEnd(UxmlDocument uxmlDocument) {
			if (string.IsNullOrWhiteSpace(uxmlDocument.RootNode.Namespace)) {
				return "";
			}

			indentation--;
			return "}";
		}
		
		private static string CreateOutputFilePath(UIDocumentAsset uiDocumentAsset, string outputPath) {
			return Path.Combine(uiDocumentAsset.Path, outputPath);
		}

		private string DoIndent() {
			var stringBuilder = new StringBuilder();
			for (int i = 0; i < indentation; i++) {
				stringBuilder.Append("\t");
			}

			return stringBuilder.ToString();
		}
	}

}