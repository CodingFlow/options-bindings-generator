using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using TestLibrary;
using VerifyCS = CodingFlow.OptionsBindingsGenerator.UnitTests.CSharpSourceGeneratorVerifier<CodingFlow.OptionsBindingsGenerator.Main>;

namespace CodingFlow.OptionsBindingsGenerator.UnitTests;

public class Tests
{
    private Assembly implementationAssembly;

    [SetUp]
    public void Setup()
    {
        implementationAssembly = GetAssembly("OptionsBindingsGenerator");
    }

    [Test]
    public async Task MixedBindings()
    {
        var sourceOne = await ReadCSharpFile<NatsServiceOptions>(true);
        var sourceTwo = await ReadCSharpFile<ServiceStreamConsumerOptions>(true);
        var generatedClass = await ReadCSharpFileByName(true, "OptionsBindings");

        await new VerifyCS.Test
        {
            CompilerDiagnostics = CompilerDiagnostics.None,
            TestState = {
                ReferenceAssemblies = ReferenceAssemblies.Net.Net90,
                AdditionalReferences =
                {
                    implementationAssembly,
                    GetAssembly("TestLibrary")
                },

                Sources = { sourceOne, sourceTwo },
                GeneratedSources =
                {
                    (typeof(Main), "OptionsBindings.generated.cs", SourceText.From(generatedClass, Encoding.UTF8, SourceHashAlgorithm.Sha256)),
                },
            },
        }.RunAsync();
    }

    private static Assembly GetAssembly(string name)
    {
        var implementationAssemblyName = Assembly.GetExecutingAssembly().GetReferencedAssemblies().First(a => a.FullName.Contains(name));
        return Assembly.Load(implementationAssemblyName);
    }

    private static async Task<string> ReadCSharpFile<T>(bool isTestLibrary)
    {
        var filenameWithoutExtension = typeof(T).Name;
        return await ReadCSharpFileByName(isTestLibrary, filenameWithoutExtension);
    }

    private static async Task<string> ReadCSharpFileByName(bool isTestLibrary, string filenameWithoutExtension)
    {
        var searchPattern = $"{filenameWithoutExtension}*.cs";
        return await ReadFile(isTestLibrary, searchPattern);
    }

    private static async Task<string> ReadFile(bool isTestLibrary, string searchPattern)
    {
        var currentDirectory = GetCurrentDirectory();

        var targetDirectory = isTestLibrary ? GetTestLibraryDirectory(currentDirectory) : currentDirectory;

        var file = targetDirectory.GetFiles(searchPattern).First();

        using var fileReader = new StreamReader(file.OpenRead());
        return await fileReader.ReadToEndAsync();
    }

    private static DirectoryInfo? GetCurrentDirectory()
    {
        return Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName);
    }

    private static DirectoryInfo GetTestLibraryDirectory(DirectoryInfo currentDirectory)
    {
        return currentDirectory.Parent.GetDirectories("TestLibrary").First();
    }
}