using Microsoft.Extensions.Options;

namespace SampleLibrary;

[OptionsValidator]
internal partial class ValidateNatsServiceOptions : IValidateOptions<NatsServiceOptions>
{
}