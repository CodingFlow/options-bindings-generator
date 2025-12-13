using Microsoft.Extensions.Options;

namespace TestLibrary;

[OptionsValidator]
internal partial class ValidateNatsServiceOptions : IValidateOptions<NatsServiceOptions>
{
}