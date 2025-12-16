using System.ComponentModel.DataAnnotations;

namespace TestLibrary;

internal record NoBindingsOptions
{
    [Required]
    public required string ServiceHost { get; init; }

    public required string Port { get; init; }
}
