# com.tactilegames.uielements-code-behind

UIElements Code Behind is a code generation tool for Unity's UI toolkit. It allows developers to generate C# UI bindings and
code-behind scripts based on a UXML template.

## Usage
In your UXML, add the attribute `tactile-code-behind="true"` to the root node to enable code behind generation. 

Optionally, add the attribute `tactile-namespace="MY_NAMESPACE"` to the root node to set what namespace the generated code should end up in.

Create a public partial class with the same name as the UI document. Whenever the UI is about to show, call the `InitializeBinding` method in the generated partial class, and pass in the root `VisualElement` of the UI you want to show. 

In order to get a hold of the root `VisualElement`, call `Instantiate` on your `VisualTreeAsset`, which is like the "prefab" for an UI document.

## How it works

Rosalina watches your changes related to all `*.uxml` files, parses its content and generates the C# UI
binding code based on the element's names.

Take for instance the following UXML template:

**`SampleDocument.uxml`**

```xml

<ui:UXML xmlns:ui="UnityEngine.UIElements" xmlns:uie="UnityEditor.UIElements"
         xsi="http://www.w3.org/2001/XMLSchema-instance"
         engine="UnityEngine.UIElements" editor="UnityEditor.UIElements"
         noNamespaceSchemaLocation="../../UIElementsSchema/UIElements.xsd" editor-extension-mode="False">
    <ui:VisualElement>
        <ui:Label text="Label" name="TitleLabel"/>
        <ui:Button text="Button" name="Button"/>
    </ui:VisualElement>
</ui:UXML>
```

Rosalina's `AssetProcessor` will automatically genearte the following C# UI bindings script:

**`SampleDocument.g.cs`**

```csharp
// <autogenerated />
using UnityEngine;
using UnityEngine.UIElements;

public partial class SampleDocument
{

    public Label TitleLabel { get; private set; }

    public Button Button { get; private set; }

    public VisualElement RootVisualElement { get; private set; }

    public InitializeBinding(VisualElement rootVisualElement)
    {
        RootVisualElement = rootVisualElement;
        TitleLabel = (Label)Root?.Q("TitleLabel");
        Button = (Button)Root?.Q("Button");
    }
}
```

> ⚠️ This script behing an auto-generated code based on the UXML template, **you should not** write code inside this
> file. It will be overwritten everytime you update your UXML template file.



## Notes

According to Unity's UI Builder warnings, a `VisualElement` name can only contains **letters**, **numbers**, **underscores** and **dashes**.
Since a name with **dashes** is not a valid name within a C# context, during the code generation process, Rosalina will automatically convert `dashed-names` into `PascalCase`.
Meaning that if you have the following UXML:
```xml
<ui:VisualElement>
    <ui:Button text="Button" name="confirm-button"/>
</ui:VisualElement>
```
Rosalina will generate the following property:
```csharp
public Button ConfirmButton { get; private set; }
```

In case you already have a `ConfirmButton` as a `VisualElement` name, do not worry, Rosalina will detect it for you during the code generation process and throw an error letting you know there is a duplicate property in your UXML document.