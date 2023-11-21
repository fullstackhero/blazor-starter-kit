using System.Reflection;

namespace CleanBlazor.Client;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
