using System.ComponentModel.DataAnnotations;
using CodingFlow.OptionsBindingsGenerator;

namespace SampleLibrary;

[OptionsBindings(false, "Services:Nats")]
internal record NatsServiceOptions
{
    [Required]
    public required string ServiceHost { get; init; }

    public required string Port { get; init; }
}
