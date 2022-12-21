# com.tactilegames.uielements-code-behind

## [1.5.0](https://github.com/tactilegames/uielements-code-behind/compare/v1.4.0...v1.5.0) (2022-12-21)

### Refactoring

Do not use Roslyn ([#5](https://github.com/tactilegames/uielements-code-behind/pull/5))  [Trond Glomnes](https://github.com/trondtactile)

#### Description
Refactored code generator to no longer uses Roslyn. 

The reason for this is that Roslyn imports certain Span<> based types, which crashes all our type lookups, including type lookups from external SDKs such as Facebook.

## [1.4.0](https://github.com/tactilegames/uielements-code-behind/compare/v1.3.1...v1.4.0) (2022-12-09)

### Feature

General improvements  ([#4](https://github.com/tactilegames/uielements-code-behind/pull/4))  [Trond Glomnes](https://github.com/trondtactile)

#### Description
Introduced 2 new root node attributes:
* `tactile-code-behind`: Only UXML documents with this attribute on the root node will have code behind generated
* `tactile-namespace`: Selects the namespace to generate the code behind in

`UIPropertyTypes` is no longer a dictionary of names->types. Instead, we cache a list of all types inheriting `ITransform` using `TypeCache`. This means all Unity and custom UIElement types will work going forwards.

#### Actions
> Any UXML with code behind already generated needs to use the new attribute in order to continue generating.

## [1.3.1](https://github.com/tactilegames/uielements-code-behind/compare/v1.3.0...v1.3.1) (2022-09-15)

### Bug Fix

Now also generates code-behind for packages ([#3](https://github.com/tactilegames/uielements-code-behind/pull/3))  [Trond Glomnes](https://github.com/trondtactile)

#### Description
Now also generates code-behind for packages.

Previously everything was anything not in the "assets" folder was filtered out.

## [1.3.0](https://github.com/tactilegames/uielements-code-behind/compare/v1.2.0...v1.3.0) (2022-08-30)

### Feature

Sync fork with origin ([#2](https://github.com/tactilegames/uielements-code-behind/pull/2))  [Trond Glomnes](https://github.com/trondtactile)

#### Description
Synced fork with origin. Pulled in support for UIToolkit.Image.

## [1.2.0](https://github.com/tactilegames/uielements-code-behind/compare/v1.1.0...v1.2.0) (2022-08-16)

### Feature

UI Elements code behind generator for Tactile Games ([#1](https://github.com/tactilegames/uielements-code-behind/pull/1))  [Trond Glomnes](https://github.com/trondtactile)

#### Description
This adds a code generator for the code-behind for uxml files, as in, UI Element definition files. The code-behind deals with generating the binding for the UI in a sibling C# file.
