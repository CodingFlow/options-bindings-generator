using Microsoft.Extensions.Options;

namespace TestLibrary;

internal partial class ValidateServiceStreamConsumerOptions : IValidateOptions<ServiceStreamConsumerOptions>
{
    public ValidateOptionsResult Validate(string? name, ServiceStreamConsumerOptions options)
    {
        throw new NotImplementedException();
    }
}