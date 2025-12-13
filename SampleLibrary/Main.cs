using CodingFlow.Generated.OptionsBindingsGenerator.GeneratedSampleLibrary;
using Microsoft.Extensions.Hosting;

namespace SampleLibrary;

public class Main
{
    public void Run()
    {
        var builder = Host.CreateApplicationBuilder();

        builder.Services.AddOptionsBindings(builder.Configuration);

        using IHost host = builder.Build();
    }
}
