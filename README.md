# Options Bindings Generator


[![Nuget](https://img.shields.io/nuget/v/OptionsBindingsGenerator)](https://www.nuget.org/packages/OptionsBindingsGenerator)
![GitHub Workflow Status (with event)](https://img.shields.io/github/actions/workflow/status/CodingFlow/options-bindings-generator/pull-request.yml)
[![Nuget](https://img.shields.io/nuget/dt/OptionsBindingsGenerator)](https://www.nuget.org/packages/OptionsBindingsGenerator)
[![GitHub Sponsors](https://img.shields.io/github/sponsors/CodingFlow)](https://github.com/sponsors/CodingFlow)

C# source generator to automatically create boilerplate dependency injection registrations for configuration options.

When using the default dependency injection framework in .NET while following the [options pattern](https://learn.microsoft.com/en-us/dotnet/core/extensions/options), it requires specifying boilerplate configuration code per options type. This source generator takes care of the boilerplate by generating an extension method containing all the boilerplate code. You are only required to specify what matters via attributes on the options types.

## Usage

After installing the nuget package into the project you want to generate options bindings for, Add the `OptionsBindings` attribute to each of the options types (record or class) you want to generate DI registrations for.
`OptionsBindings` constructor accepts 2 parameters: `validateOnStart` indicating if option validation should occur on application start, and `configurationSection` to specify the configuration sub-section to bind to (Following same [conventions for nested properties](https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration#binding-hierarchies) as writing code manually):

```csharp
using System.ComponentModel.DataAnnotations;
using CodingFlow.OptionsBindingsGenerator;

namespace TestLibrary;

[OptionsBindings(false, "Services:Nats")]
internal record NatsServiceOptions
{
    [Required]
    public required string ServiceHost { get; init; }

    public required string Port { get; init; }
}
```

An extension method called `AddOptionsBindings` containing all the required DI code will be generated in the namespace `CodingFlow.Generated.OptionsBindingsGenerator.Generated{assemblyName}`, where `{assemblyName}` will be the name of the assembly of one of the option types, which will most likely be the name of the project the option type is in.
In this example, the fully qualified name will be `CodingFlow.Generated.OptionsBindingsGenerator.GeneratedTestLibrary.AddOptionsBindings`.

A nonexistent options validator class will also be registered per options type with the attribute. In this example it's called `ValidateNatsServiceOptions`. Create the missing classes using the official [source generator attribute](https://learn.microsoft.com/en-us/dotnet/core/extensions/options-validation-generator) for generating performant options validation based on the data annotations in the options type:

```csharp
using Microsoft.Extensions.Options;

namespace TestLibrary;

[OptionsValidator]
internal partial class ValidateNatsServiceOptions : IValidateOptions<NatsServiceOptions>
{
}
```

Finally, call the generated extension method in your application DI code:

```csharp
using CodingFlow.Generated.OptionsBindingsGenerator.GeneratedTestLibrary;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddOptionsBindings(builder.Configuration);

using IHost host = builder.Build();
```