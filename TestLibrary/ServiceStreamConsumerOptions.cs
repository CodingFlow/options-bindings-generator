using CodingFlow.OptionsBindingsGenerator;

namespace TestLibrary;

[OptionsBindings(true)]
internal record ServiceStreamConsumerOptions
{
    public required string StreamName { get; init; }

    public required string ConsumerName { get; init; }
}
