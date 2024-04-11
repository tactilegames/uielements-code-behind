# com.tactilegames.uielements-code-behind

## [1.7.1](https://github.com/tactilegames/uielements-code-behind/compare/v1.7.0...v1.7.1) (2024-04-11)

### Bug Fix

Fixed issue where no named elements would cause a compiled class that does not compile ([#11](https://github.com/tactilegames/uielements-code-behind/pull/11))  [Trond Glomnes](https://github.com/trondtactile)

#### Description
Fixed issue where no named elements would cause a compiled class that does not compile

#### Ticket(s)
https://tactileentertainment.atlassian.net/browse/USM-1471

## [1.7.0](https://github.com/tactilegames/uielements-code-behind/compare/v1.6.1...v1.7.0) (2024-02-21)

### Feature

Now supports generating events for nodes ([#10](https://github.com/tactilegames/uielements-code-behind/pull/10))  [Trond Glomnes](https://github.com/trondtactile)

#### Description
Now supports generating events for nodes by adding the attribute `tactile-event-name`.

## [1.6.1](https://github.com/tactilegames/uielements-code-behind/compare/v1.6.0...v1.6.1) (2024-02-16)

### Bug Fix

Template support ([#9](https://github.com/tactilegames/uielements-code-behind/pull/9))  [Trond Glomnes](https://github.com/trondtactile)

#### Description
Now correctly handles cases where UXML has `Template` and `Instance` usage, by mapping those properties to `VisualElement`.

## [1.6.0](https://github.com/tactilegames/uielements-code-behind/compare/v1.5.2...v1.6.0) (2023-07-05)

### Feature

Add auto loading of UXML and USS in generated code ([#8](https://github.com/tactilegames/uielements-code-behind/pull/8))  [Trond Glomnes](https://github.com/trondtactile)

#### Description
Added new attributes `tactile-resource-path` and `tactile-stylesheet-path` to allow the generated code to automatically load UXML and USS.

## [1.5.2](https://github.com/tactilegames/uielements-code-behind/compare/v1.5.1...v1.5.2) (2023-07-05)

### Bug Fix

Update property assignment in CodeGenerator ([#7](https://github.com/tactilegames/uielements-code-behind/pull/7))  [Trond Glomnes](https://github.com/trondtactile)

#### Description
Fixed an issue where we did not use the correct property for generating the binding code. We used the Property name instead of the original name of the uxml. This caused the binding to fail.

## [1.5.1](https://github.com/tactilegames/uielements-code-behind/compare/v1.5.0...v1.5.1) (2022-12-22)

### Bug Fix

Fixed compiler error due to missing meta file ([#6](https://github.com/tactilegames/uielements-code-behind/pull/6))  [Trond Glomnes](https://github.com/trondtactile)

#### Description
Fixed compiler error due to missing meta file.

This passed the CI due to the fact that the way Unity imports the package in the CI, leaves the package mutable. However, when real projects import this package, it's immutable. 

So in other words, the CI was allowed to generate the missing meta file and import the class. Real projects were not allowed to generate the meta file, and therefore did not import the class, causing a compiler error.

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
